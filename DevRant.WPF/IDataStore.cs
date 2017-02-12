using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevRant.WPF
{
    interface IDataStore
    {
        IReadOnlyList<string> FollowedUsers { get; }
        long FollowedUsersLastChecked { get; set; }

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
