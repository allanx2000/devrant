using System;
using DevRant.Dtos;
using System.Windows;

namespace DevRant.WPF.ViewModels
{
    public class Notification : FeedItem
    {
        private NotificationInfo notif;

        public enum Type
        {
            NA
        }

        public FontWeight TextWeight
        {
            get { return notif.IsRead ? FontWeights.Normal : FontWeights.Bold; }
        }

        public Type NotificationType {get; private set;}
        public string URL { get
            {
                return Utilities.BaseURL + "/rants/" + notif.RantId;
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

            SetText();
        }

        private void SetText()
        {
            switch (notif.NotificationType)
            {
                case "content_vote":
                    Text = notif.ActionUsername + " +1'd your rant!";
                    break;
                case "comment_vote":
                    Text = notif.ActionUsername + " +1'd your comment!";
                    break;
                case "comment_content":
                    Text = notif.ActionUsername + " commented on your rant!";
                    break;
                case "comment_discuss":
                    Text = "New comment on a rant you commented on!";
                    break;
                case "comment_mention":
                    Text = notif.ActionUsername + " mentioned you in a comment!";
                    break;
                default:
                    Text = notif.NotificationType;
                    break;
            }
        }
    }
}