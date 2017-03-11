using System;

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