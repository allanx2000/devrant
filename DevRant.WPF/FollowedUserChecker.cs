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
            public int TotalUnread { get; set; }
            public int Added { get; set; }
        }

        public void Start()
        {
            //TODO: Timer

            Thread th = new Thread(DoCheck);
            th.Start();
        }

        private async void DoCheck()
        {
            long lastTime = ds.FollowedUsersLastChecked;


            List<RantInfo> added = new List<RantInfo>(); 

            foreach (string user in ds.FollowedUsers)
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

                if (OnUpdate != null)
                {
                    OnUpdate.Invoke(new UpdateArgs()
                    {
                        Added = added.Count,
                        TotalUnread = Posts.Count
                    });
                }
            }
        }
    }
}