using DevRant.Dtos;
using Innouvous.Utils.Merged45.MVVM45;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevRant.WPF.DataStore
{
    public class SavedPostContent : ViewModel
    {
        public SavedPostContent(string text, long? id = null, string tags = null, string imagePath = null)
        {
            ID = id;
            Text = text;
            Tags = tags;
            ImagePath = imagePath;
        }
        
        public PostContent ToPostContent()
        {
            var pc = new PostContent(Text);

            if (!string.IsNullOrEmpty(Tags))
                pc.SetTag(Tags);

            if (!string.IsNullOrEmpty(ImagePath))
            {
                byte[] bytes = File.ReadAllBytes(ImagePath);
                pc.AddImage(bytes, ImagePath);
            }

            return pc;
        }

        public long? ID
        {
            get { return Get<long?>(); }
            private set
            {
                Set(value);
                RaisePropertyChanged();
            }
        }

        public string Text
        {
            get { return Get<string>(); }
            private set
            {
                Set(value);
                RaisePropertyChanged();
            }
        }

        public string Tags
        {
            get { return Get<string>(); }
            set
            {
                Set(value);
                RaisePropertyChanged();
            }
        }

        public string ImagePath
        {
            get { return Get<string>(); }
            set
            {
                Set(value);
                RaisePropertyChanged();
            }
        }

        public void SetId(long id)
        {
            ID = id;
        }
    }
}
