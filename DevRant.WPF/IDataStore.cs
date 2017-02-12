using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevRant.Dtos;

namespace DevRant.WPF
{
    interface IDataStore
    {
        IReadOnlyList<string> FollowedUsers { get; }
        long FollowedUsersLastChecked { get; set; }
        int FollowedUsersUpdateInterval { get; }

        StoryRange StoryRange { get; }
        RantSort StorySort { get;}

        void Unfollow(string user);
        void Follow(string user);

        //string GeneralSortOrder { get; }
        //int SizeGeneral { get; }

        /*
        string Username { get; }
        string Password { get; }
        */
    }
}
