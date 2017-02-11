using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevRant.WPF
{
    class AppSettingsDataStore : IDataStore
    {
        public List<string> FollowedUsers
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Follow(string user)
        {
            throw new NotImplementedException();
        }

        public void Unfollow(string user)
        {
            throw new NotImplementedException();
        }
    }
}
