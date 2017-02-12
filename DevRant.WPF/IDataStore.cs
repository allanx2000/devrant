using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevRant.Dtos;

namespace DevRant.WPF
{
    public interface IDataStore
    {
        IReadOnlyList<string> FollowedUsers { get; }
        long FollowedUsersLastChecked { get; set; }
        int FollowedUsersUpdateInterval { get; }

        StoryRange DefaultRange { get; }
        RantSort DefaultFeed { get;}

        void Unfollow(string user);
        void Follow(string user);

        void SetDefaultRange(StoryRange defaultStoryRange);
        void SetDefaultFeed(RantSort defaultFeed);
        void SetFollowing(ICollection<string> users);
        void SetUpdatesInterval(int updateCheckInterval);


        /*
        string Username { get; }
        string Password { get; }
        */
    }
}
