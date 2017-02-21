using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevRant.WPF.ViewModels
{
    public class Draft : FeedItem
    {
        public Draft() : base(FeedItemType.Draft)
        {
        }
    }
}
