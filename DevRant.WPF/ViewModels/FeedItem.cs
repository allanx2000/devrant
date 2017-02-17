using System;

namespace DevRant.WPF.ViewModels
{
    public abstract class FeedItem : Innouvous.Utils.Merged45.MVVM45.ViewModel
    {
        
        public FeedItem(FeedItemType type)
        {
            Type = type;
        }

        public enum FeedItemType
        {
            Post,
            Notification,

        }

        public FeedItemType Type { get; private set; }

        public virtual bool Read { get; set; }

        public Notification AsNotification()
        {
            return this as Notification;
        }
        public Rant AsRant()
        {
            return this as Rant;
        }
    }
}