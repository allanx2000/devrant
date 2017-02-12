using DevRant.Dtos;
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
        private DevRantClient api;
        private IDataStore ds;
        private Thread thread;

        public ObservableCollection<Rant> Posts { get; private set; }

        public FollowedUserChecker(IDataStore ds, DevRantClient api)
        {
            this.ds = ds;
            this.api = api;

            Posts = new ObservableCollection<Rant>();
        }


        public delegate void OnUpdatedHandler(UpdateArgs args);
        public event OnUpdatedHandler OnUpdate;

        internal class UpdateArgs
        {
            public int Total { get; set; }
            public int TotalUnread { get; set; }
            public int Added { get; set; }
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

                        SendUpdate(added.Count);
                    }

                    int millis = ds.FollowedUsersUpdateInterval * 60 * 1000;
                    Thread.Sleep(millis);
                }
                catch (Exception e)
                {
                    var ex = e.StackTrace;
                }
            }
        }

        internal void SendUpdate(int added = 0)
        {
            if (OnUpdate != null)
            {
                OnUpdate.Invoke(new UpdateArgs()
                {
                    Added = added,
                    Total = Posts.Count,
                    TotalUnread = Posts.Count(x => !x.Read)
                });
            }
        }
    }
}