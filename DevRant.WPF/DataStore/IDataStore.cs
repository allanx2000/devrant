using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevRant.Dtos;
using DevRant.Enums;

namespace DevRant.WPF.DataStore
{
    public interface IDataStore
    {
        IReadOnlyList<string> FollowedUsers { get; }
        long FollowedUsersLastChecked { get; set; }
        int FollowedUsersUpdateInterval { get; }

        string DBFolder { get; }

        RantRange DefaultRange { get; }
        RantSort DefaultFeed { get;}
        bool HideUsername { get; }
        bool ShowCreateTime { get; }
        bool FilterOutRead { get; }
        bool OpenInProfileViewer { get; }
        int ResultsLimit { get; }
        int MaxPages { get; }
        int MinScore { get; set; }

        void Unfollow(string user);
        void Follow(string user);

        void SetDBFolder(string path);

        void SetDefaultRange(RantRange defaultStoryRange);
        void SetDefaultFeed(RantSort defaultFeed);
        List<string> SetFollowing(ICollection<string> users);
        void SetUpdatesInterval(int updateCheckInterval);
        LoginInfo GetLoginInfo();
        void SetHideUsername(bool hideUsername);
        void SetShowCreateTime(bool showCreateTime);
        void SetLogin(LoginInfo loginInfo);
        void SetFilterOutRead(bool filterOutRead);
        void SetLimits(int resultsLimit, int minScore, int maxPages);


        /*
        string Username { get; }
        string Password { get; }
        */
    }
}
