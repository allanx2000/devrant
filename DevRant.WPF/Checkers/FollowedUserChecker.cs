using DevRant.Dtos;
using DevRant.WPF.DataStore;
using DevRant.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Permissions;
using System.Threading;
using DevRant.Enums;

namespace DevRant.WPF.Checkers
{
    internal class FollowedUserChecker
    {
        private Thread checkerThread;

        private IDevRantClient api;
        private IDataStore ds;
        private IPersistentDataStore history;

        public UpdateArgs GetFeedUpdate()
        {
            return new UpdateArgs(UpdateType.UpdateFeed, 0, null);
        }


        public FollowedUserChecker(IDataStore ds, IDevRantClient api, IPersistentDataStore history)
        {
            this.ds = ds;
            this.api = api;
            this.history = history;            
        }

        public FollowedUserChecker() : this(AppManager.Instance.Settings, AppManager.Instance.API, AppManager.Instance.DB)
        {

        }

        public delegate void OnUpdatedHandler(UpdateArgs args);
        public event OnUpdatedHandler OnUpdate;

        
        
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

                    AppManager.Instance.RemoveReadUpdates();

                    long lastTime = ds.FollowedUsersLastChecked;

                    List<ViewModels.Rant> added = new List<ViewModels.Rant>();

                    var users = ds.FollowedUsers.ToList(); //Can be modified while checking

                    foreach (string user in users)
                    {
                        Profile profile = await api.GetProfileAsync(user, ProfileContentType.Rants);
                        AddTo(lastTime, user, ProfileContentType.Rants, profile.Rants, added);

                        profile = await api.GetProfileAsync(user, ProfileContentType.Upvoted);
                        AddTo(lastTime, user, ProfileContentType.Upvoted, profile.Rants, added);                        
                    }

                    if (!IsLatest(version))
                        break;

                    if (added.Count > 0)
                    {
                        long latest = added.Max(x => x.RawCreateTime);

                        foreach (var r in added)
                        {
                            AppManager.Instance.AddUpdate(r);
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
                SendUpdate(UpdateType.Error, error: ex);
            }
        }

        //TODO: NEed lastTime? IsRead should be enough?
        //Check if isRead is stored even if Filter is false
        private void AddTo(long lastTime, string username, ProfileContentType type, List<Dtos.Rant> rants, List<ViewModels.Rant> added)
        {
            foreach (var rant in rants)
            {
                if (rant.CreatedTime > lastTime)
                {
                    if (!history.IsRead(rant.Id))
                    {
                        ViewModels.Rant r = new ViewModels.Rant(rant);
                        r.UpdateText = MakeMessage(type, username);

                        added.Add(r);
                    }
                }
            }
        }
        
        internal void SendUpdate(UpdateType type, int added = 0, string users = null, Exception error = null)
        {
            if (OnUpdate != null)
            {
                var update = new UpdateArgs(type, added, users);
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
            List<ViewModels.Rant> added = new List<ViewModels.Rant>();

            foreach (string user in users)
            {
                try
                {
                    Profile profile = await api.GetProfileAsync(user);

                    foreach (var rant in profile.Rants)
                    {
                        ViewModels.Rant r = new ViewModels.Rant(rant);
                        r.UpdateText = MakeMessage(Enums.ProfileContentType.Rants, user);

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

        private string MakeMessage(ProfileContentType type, string user)
        {
            switch (type)
            {
                case ProfileContentType.Upvoted:
                    return user + " +1'd a rant";
                case ProfileContentType.Rants:
                    return "New rant by " + user;
                default:
                    return null;
            }
        }
    }
}