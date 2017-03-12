using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media;

namespace DevRant.WPF.ViewModels
{
    public abstract class FeedItem : Innouvous.Utils.Merged45.MVVM45.ViewModel
    {
        
        public FeedItem(FeedItemType type, long createTime)
        {
            Type = type;
            RawCreateTime = createTime;

        }

        public enum FeedItemType
        {
            NA,
            Post,
            Notification,
            Collab,
            Draft,
            Comment,
        }

        public FeedItemType Type { get; private set; }

        public virtual bool Read { get; set; }
        public long RawCreateTime { get; private set; }
        
        protected void LoadImage(Dtos.ImageInfo imageInfo, Action<ImageSource,Visibility> callback)
        {
            if (imageInfo != null)
            {

                try
                {
                    Image bmp = Utilities.GetImage(imageInfo.Url);

                    App.Current.Dispatcher.Invoke(() => {

                        var picture = Utilities.GetImageSource(bmp);
                        var animate = Utilities.ConvertToVisibility(imageInfo.Frame != null);
                        
                        callback.Invoke(picture, animate);

                        //PictureUrl = rant.Image.Url;
                    });



                    //App.Current.Dispatcher.Invoke(() => Picture = Utilities.GetImageSource(bmp));
                }
                catch (Exception e)
                {
                    //Timeout?
                }
            }
        }


        public Notification AsNotification()
        {
            return this as Notification;
        }
        public Rant AsRant()
        {
            return this as Rant;
        }

        public Collab AsCollab()
        {
            return this as Collab;
        }

        internal Comment AsComment()
        {
            return this as Comment;
        }
    }
}