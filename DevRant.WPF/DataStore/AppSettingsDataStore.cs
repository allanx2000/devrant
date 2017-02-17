using Innouvous.Utils.DataBucket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevRant.Dtos;
using DevRant.Enums;

namespace DevRant.WPF.DataStore
{
    class AppSettingsDataStore : IDataStore
    {
        private Properties.Settings Settings = Properties.Settings.Default;

        private DataBucket bucket = new DataBucket();

        private List<string> followedUsers;

        public AppSettingsDataStore()
        {
            followedUsers = new List<string>();

            if (Settings.FollowedUsers != null)
            {
                foreach (var user in Settings.FollowedUsers)
                    followedUsers.Add(user);
            }
        }
        
        public IReadOnlyList<string> FollowedUsers
        {
            get
            {
                return followedUsers.AsReadOnly();
            }
        }

        public long FollowedUsersLastChecked
        {
            get
            {
                //return Utilities.ToUnixTime(DateTime.UtcNow.AddHours(-12));
                return Settings.LastChecked;
            }
            set
            {
                Settings.LastChecked = value;
                Settings.Save();
            }
        }

        public int FollowedUsersUpdateInterval
        {
            get
            {
                return Settings.FollowedUsersUpdateInterval;
            }
        }

        public StoryRange DefaultRange
        {
            get
            {
                return Settings.StoryRange;
            }
        }

        public RantSort DefaultFeed
        {
            get
            {
                return Settings.StorySort;
            }
        }

        public bool HideUsername
        {
            get
            {
                return Settings.HideUsername;
            }
        }

        public bool ShowCreateTime
        {
            get
            {
                return Settings.ShowCreateTime;
            }
        }

        public bool FilterOutRead
        {
            get
            {
                return Settings.FilterOutRead;
            }
        }

        public void Follow(string user)
        {
            if (!followedUsers.Contains(user))
            {
                followedUsers.Add(user);
                SaveUsers();
            }
        }

        private void SaveUsers()
        {
            if (Settings.FollowedUsers == null)
            {
                Settings.FollowedUsers = new System.Collections.Specialized.StringCollection();
            }
            else
                Settings.FollowedUsers.Clear();

            foreach (var user in followedUsers)
                Settings.FollowedUsers.Add(user);

            Settings.Save();
        }

        public void Unfollow(string user)
        {
            if (followedUsers.Contains(user))
            {
                followedUsers.Remove(user);
                SaveUsers();
            }
        }

        public void SetDefaultRange(StoryRange defaultStoryRange)
        {
            Settings.StoryRange = defaultStoryRange;
            Settings.Save();
        }

        public void SetDefaultFeed(RantSort defaultFeed)
        {
            Settings.StorySort = defaultFeed;
            Settings.Save();
        }

        public List<string> SetFollowing(ICollection<string> users)
        {
            var same = users.Intersect(followedUsers);

            var added = from i in users where !same.Contains(i) select i;
            var list = added.ToList();

            followedUsers.Clear();
            followedUsers.AddRange(users);
            SaveUsers();

            return list;
        }

        public void SetUpdatesInterval(int updateCheckInterval)
        {
            Settings.FollowedUsersUpdateInterval = updateCheckInterval;
            Settings.Save();
        }

        public LoginInfo GetLoginInfo()
        {
            if (!string.IsNullOrEmpty(Settings.Username) && !string.IsNullOrEmpty(Settings.Password))
            {
                return new LoginInfo(Settings.Username, Settings.Password);
            }
            else
                return null;
        }

        public void SetHideUsername(bool hide)
        {
            Settings.HideUsername = hide;
            Settings.Save();
        }

        public void SetShowCreateTime(bool showCreateTime)
        {
            Settings.ShowCreateTime = showCreateTime;
            Settings.Save();
        }

        public void SetLogin(LoginInfo info)
        {
            Settings.Username = info.Username;
            Settings.Password = info.Password;
            Settings.Save();
        }

        public void SetFilterOutRead(bool filterOutRead)
        {
            Settings.FilterOutRead = filterOutRead;
            Settings.Save();
        }
    }
}
