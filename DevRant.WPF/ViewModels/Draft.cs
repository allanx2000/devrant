using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevRant.WPF.DataStore;
using System.Windows;
using DevRant.WPF.Converters;

namespace DevRant.WPF.ViewModels
{
    public class Draft : FeedItem
    {
        private SavedPostContent draft;

        public string Text
        {
            get { return draft.Text; }
        }

        private static readonly VisibilityConverter converter = new VisibilityConverter();

        public Visibility ImagePathVisibility
        {
            get { return (Visibility) converter.Convert(!string.IsNullOrEmpty(draft.ImagePath)); }
        }

        public Visibility TagsVisibility

        {
            get { return (Visibility) converter.Convert(!string.IsNullOrEmpty(draft.Tags)); }
        }

        public string Tags
        {
            get { return draft.Tags; }
        }

        public string ImagePath
        {
            get { return draft.ImagePath; }
        }

        public Draft() : base(FeedItemType.Draft)
        {
        }

        public Draft(SavedPostContent draft) : this()
        {
            this.draft = draft;
        }

        //TODO: Need to have DB update as well? and return updated?
        public void Update(SavedPostContent draft)
        {
            throw new NotImplementedException();
        }
    }
}
