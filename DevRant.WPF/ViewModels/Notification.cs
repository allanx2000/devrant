namespace DevRant.WPF.ViewModels
{
    public class Notification: FeedItem
    {
        public string NotificationURL { get; set; }
        public string Text { get; set; }

        public Notification() : base(FeedItemType.Notification)
        {
            Text = "test";
        }
    }
}