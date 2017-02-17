using DevRant.Dtos;
using DevRant.WPF.DataStore;
using DevRant.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Permissions;
using System.Threading;

namespace DevRant.WPF
{
    internal class FollowedUserChecker
    {
        private Thread checkerThread;

        private IDevRantClient api;
        private IDataStore ds;
        private IHistoryStore history;

        public UpdateArgs GetFeedUpdate()
        {
            return new UpdateArgs(UpdateType.UpdateFeed, 0, null, Posts);
        }

        public ObservableCollection<Rant> Posts { get; private set; }

        public FollowedUserChecker(IDataStore ds, IDevRantClient api, IHistoryStore history)
        {
            this.ds = ds;
            this.api = api;
            this.history = history;
            
            Posts = new ObservableCollection<Rant>();
        }
        
        public delegate void OnUpdatedHandler(UpdateArgs args);
        public event OnUpdatedHandler OnUpdate;

        public enum UpdateType
        {
            GetAllForUser,
            UpdatesCheck,
            UpdateFeed,
            Error
        }

        public class UpdateArgs
        {
            public UpdateArgs(UpdateType type, int added, string users, ICollection<Rant> posts)
            {
                Added = added;
                Type = type;

                if (posts != null)
                {
                    Total = posts.Count;
                    TotalUnread = posts.Count(x => !x.Read);
                }

                if (users != null)
                    Users = users;
            }

            public UpdateType Type { get; private set; }
            public int Total { get; private set; }
            public int TotalUnread { get; private set; }
            public int Added { get; private set; }
            public string Users { get; private set; }
            public string Error { get; internal set; }
        }

        private void SafeStart(object paramz)
        {
        }

        private int latestVersion = 0;

        public void Start()
        {
            latestVersion++;

            int v = latestVersion;
            checkerThread = new Thread(() => RunChecker(v));
            checkerThread.Start();
        }

        public void Stop()
        {
            checkerThread.Abort();
            latestVersion++;
        }

        private bool IsLatest(int version)
        {
            return version == latestVersion;
        }
        private async void RunChecker(int version)
        {
            try
            {
                while (true)
                {
                    if (!IsLatest(version))
                        break;

                    RemoveRead();

                    long lastTime = ds.FollowedUsersLastChecked;

                    List<Rant> added = new List<Rant>();

                    var users = ds.FollowedUsers.ToList(); //Can be modified while checking

                    foreach (string user in users)
                    {
                        Profile profile = await api.GetProfileAsync(user);

                        foreach (var rant in profile.Rants)
                        {
                            if (rant.CreatedTime > lastTime)
                            {
                                if (!history.IsRead(rant.Id))
                                {
                                    Rant r = new Rant(rant);
                                    added.Add(r);
                                }
                            }
                        }
                    }

                    if (!IsLatest(version))
                        break;

                    if (added.Count > 0)
                    {
                        long latest = added.Max(x => x.RawCreateTime);

                        foreach (var r in added)
                        {
                            Posts.Add(r);
                        }

                        ds.FollowedUsersLastChecked = latest;
                    }

                    SendUpdate(UpdateType.UpdatesCheck, added.Count);

                    int millis = ds.FollowedUsersUpdateInterval * 60 * 1000;
                    Thread.Sleep(millis);
                }
            }
            catch (Exception ex)
            {
                SendUpdate(UpdateType.Error, error: ex.Message);
            }
        }

        private void RemoveRead()
        {
            var read = Posts.Where(x => x.Read).ToList();

            foreach (var r in read)
                Posts.Remove(r);
        }

        internal void SendUpdate(UpdateType type, int added = 0, string users = null, string error = null)
        {
            if (OnUpdate != null)
            {
                var update = new UpdateArgs(type, added, users, Posts);
                update.Error = error;

                OnUpdate.Invoke(update);
            }
        }

        public void Restart()
        {
            Stop();

            Start();
        }

        public void GetAll(string username)
        {
            var list = new List<string>();
            list.Add(username);
            GetAll(list);
        }

        public void GetAll(IEnumerable<string> users)
        {
            Thread th = new Thread(() => GetAllForUsers(users));
            th.Start();
        }

        private async void GetAllForUsers(IEnumerable<string> users)
        {
            List<Rant> added = new List<Rant>();

            foreach (string user in users)
            {
                try
                {
                    Profile profile = await api.GetProfileAsync(user);

                    foreach (var rant in profile.Rants)
                    {
                        Rant r = new Rant(rant);
                        Posts.Add(r);
                        added.Add(r);
                    }
                }
                catch (Exception e)
                {
                    var st = e.StackTrace;
                }
            }

            SendUpdate(UpdateType.GetAllForUser, added.Count, string.Join(", ", users));
        }
    }
}