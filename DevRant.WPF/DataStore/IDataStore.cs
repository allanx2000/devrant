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

        StoryRange DefaultRange { get; }
        RantSort DefaultFeed { get;}
        bool HideUsername { get; }
        bool ShowCreateTime { get; }
        bool FilterOutRead { get; }

        void Unfollow(string user);
        void Follow(string user);

        void SetDBFolder(string path);

        void SetDefaultRange(StoryRange defaultStoryRange);
        void SetDefaultFeed(RantSort defaultFeed);
        List<string> SetFollowing(ICollection<string> users);
        void SetUpdatesInterval(int updateCheckInterval);
        LoginInfo GetLoginInfo();
        void SetHideUsername(bool hideUsername);
        void SetShowCreateTime(bool showCreateTime);
        void SetLogin(LoginInfo loginInfo);
        void SetFilterOutRead(bool filterOutRead);


        /*
        string Username { get; }
        string Password { get; }
        */
    }
}
