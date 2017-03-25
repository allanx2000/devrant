using Innouvous.Utils.DataBucket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevRant.Dtos;
using DevRant.Enums;
using System.IO;

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
        
        public bool OpenInProfileViewer
        {
            //TODO: Implement
            get { return true; }
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

        public void SetLogin(LoginInfo info)
        {
            Settings.Username = info.Username;
            Settings.Password = info.Password;
            Settings.Save();
        }

        public bool IsFollowing(string username)
        {
            if (FollowedUsers != null && followedUsers.Contains(username))
                return true;
            else
                return false;
        }


        #region Followed Users
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
        #endregion

        #region Section Options
        public RantRange DefaultRange
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

        public void SetDefaultRange(RantRange defaultStoryRange)
        {
            Settings.StoryRange = defaultStoryRange;
            Settings.Save();
        }

        public void SetDefaultFeed(RantSort defaultFeed)
        {
            Settings.StorySort = defaultFeed;
            Settings.Save();
        }
        #endregion

        #region Display Options
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
        #endregion

        #region Query Options
        public bool FilterOutRead
        {
            get
            {
                return Settings.FilterOutRead;
            }
        }

        public string DBFolder
        {
            get
            {
                return Settings.DataStoreFolder;
            }
        }

        public int ResultsLimit
        {
            get
            {
                return Settings.ResultsLimit;
            }
        }

        public int MaxPages
        {
            get
            {
                return Settings.MaxPages;
            }
        }

        public int MinScore
        {
            get
            {
                return Settings.MinScore;
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        public void SetFilterOutRead(bool filterOutRead)
        {
            Settings.FilterOutRead = filterOutRead;
            Settings.Save();
        }

        public void SetDBFolder(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                Settings.DataStoreFolder = null;
            }
            else
            {
                if (!Directory.Exists(path))
                {
                    throw new DirectoryNotFoundException();
                }

                Settings.DataStoreFolder = path;
            }

            Settings.Save();
        }

        public void SetLimits(int resultsLimit, int minScore, int maxPages)
        {
            if (resultsLimit < 10)
                throw new Exception("Results limit must be 10 or greater.");
            else if (maxPages > 20)
                throw new Exception("Maximum pages to search must be 20 or less.");
            else if (minScore < 0)
                minScore = 0;

            Settings.ResultsLimit = resultsLimit;
            Settings.MinScore = minScore;
            Settings.MaxPages = maxPages;
            Settings.Save();
        }

        #endregion

        #region Followed Rants
        public long FollowedRantsLastChecked
        {
            get
            {
                return Settings.FollowedRantsLastChecked;
            }

            set
            {
                Settings.LastChecked = value;
                Settings.Save();
            }
        }

        private const int followedRantsInterval = 60 * 60 * 1000; //1hr
        public int FollowedRantsInterval
        {
            get
            {
                return followedRantsInterval;
            }

            set
            {
                throw new NotImplementedException();
            }
        }
        #endregion

    }
}
