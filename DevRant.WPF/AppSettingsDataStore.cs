using Innouvous.Utils.DataBucket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevRant.Dtos;

namespace DevRant.WPF
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

        public void SetFollowing(ICollection<string> users)
        {
            followedUsers.Clear();
            followedUsers.AddRange(users);
            SaveUsers();
        }
    }
}
