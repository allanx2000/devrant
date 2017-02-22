using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevRant.WPF.DataStore;

namespace DevRant.WPF.ViewModels
{
    public class Draft : FeedItem
    {
        private SavedPostContent draft;

        public Draft() : base(FeedItemType.Draft)
        {
        }

        public Draft(SavedPostContent draft) : this()
        {
            this.draft = draft;
        }
    }
}
