using System;
using System.Collections.Generic;
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
        RantSort DefaultSort { get;}

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
