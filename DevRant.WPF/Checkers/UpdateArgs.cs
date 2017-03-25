using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevRant.WPF.Checkers
{
    public class UpdateArgs
    {
        public UpdateArgs(UpdateType type, int added, string users, ICollection<ViewModels.Rant> posts)
        {
            Added = added;
            Type = type;

            if (posts != null)
            {
                Total = posts.Count;
                TotalUnread = posts.Count(x => !x.Read);
            }

            if (users != null)
                Users = users;
        }

        public UpdateType Type { get; private set; }
        public int Total { get; private set; }
        public int TotalUnread { get; private set; }
        public int Added { get; private set; }
        public string Users { get; private set; }
        public Exception Error { get; internal set; }
    }

}
