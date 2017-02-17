using System;
using DevRant.Dtos;

namespace DevRant.WPF.ViewModels
{
    public class Notification : FeedItem
    {
        private NotificationInfo notif;

        public enum Type
        {
            NA
        }

        public Type NotificationType {get; private set;}
        public string URL { get
            {
                return null;
            }
        }

        public bool Read
        {
            get { return notif.IsRead; }
        }

        public string Text { get; private set; }
        
        public Notification(NotificationInfo notif) : base(FeedItemType.Notification)
        {
            this.notif = notif;
            
        }

        private void SetText()
        {
            throw new NotImplementedException();
        }
    }
}