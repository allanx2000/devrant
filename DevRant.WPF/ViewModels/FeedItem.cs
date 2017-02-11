namespace DevRant.WPF.ViewModels
{
    public abstract class FeedItem
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
    }
}