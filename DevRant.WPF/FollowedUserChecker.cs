using DevRant.Dtos;
using DevRant.WPF.DataStore;
using DevRant.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace DevRant.WPF
{
    internal class FollowedUserChecker
    {
        private IDevRantClient api;
        private IDataStore ds;
        private Thread thread;


        public UpdateArgs GetFeedUpdate()
        {
            return new UpdateArgs(UpdateType.UpdateFeed, 0, null, Posts);
        }

        public ObservableCollection<Rant> Posts { get; private set; }

        public FollowedUserChecker(IDataStore ds, IDevRantClient api)
        {
            this.ds = ds;
            this.api = api;

            Posts = new ObservableCollection<Rant>();
        }


        public delegate void OnUpdatedHandler(UpdateArgs args);
        public event OnUpdatedHandler OnUpdate;
        
        public enum UpdateType
        {
            GetAllForUser,
            UpdatesCheck,
            UpdateFeed
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
        }

        public void Start()
        {            
            thread = new Thread(RunChecker);
            thread.Start();            
        }

        public void Stop()
        {
            thread.Abort();
        }

        private async void RunChecker()
        {
            while (true)
            {
                try
                {
                    long lastTime = ds.FollowedUsersLastChecked;

                    List<RantInfo> added = new List<RantInfo>();

                    var users = ds.FollowedUsers.ToList(); //Can be modified while checking

                    foreach (string user in users)
                    {
                        Profile profile = await api.GetProfileAsync(user);

                        foreach (var rant in profile.Rants)
                        {
                            if (rant.CreatedTime > lastTime)
                            {
                                Rant r = new Rant(rant);
                                Posts.Add(r);
                                added.Add(rant);
                            }
                        }
                    }

                    if (added.Count > 0)
                    {
                        long latest = added.Max(x => x.CreatedTime);
                        ds.FollowedUsersLastChecked = latest;
                    }

                    SendUpdate(UpdateType.UpdatesCheck, added.Count);

                    int millis = ds.FollowedUsersUpdateInterval * 60 * 1000;
                    Thread.Sleep(millis);
                }
                catch (Exception e)
                {
                    var ex = e.StackTrace;
                }
            }
        }

        internal void SendUpdate(UpdateType type, int added = 0, string users = null)
        {
            if (OnUpdate != null)
            {
                var update = new UpdateArgs(type, added, users, Posts);


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