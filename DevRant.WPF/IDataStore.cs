using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevRant.WPF
{
    interface IDataStore
    {
        List<string> FollowedUsers { get; }
        void Unfollow(string user);
        void Follow(string user);
        
        //TODO: Login, current user

    }
}
