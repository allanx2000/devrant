using System;

namespace DevRant.WPF.ViewModels
{
    internal class DummyCommentable : FeedItem, Commentable
    {
        public DummyCommentable(long rantId) : base(FeedItemType.NA)
        {
            RantId = rantId;
        }

        public long RantId { get; private set; }

        public void IncrementComments()
        {
        }
    }
}