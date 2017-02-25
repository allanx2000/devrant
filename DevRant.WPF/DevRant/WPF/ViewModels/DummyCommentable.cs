using System;

namespace DevRant.WPF.ViewModels
{
    internal class DummyCommentable : Commentable
    {
        public DummyCommentable(long rantId)
        {
            RantId = rantId;
        }

        public long RantId { get; private set; }

        public void IncrementComments()
        {
        }
    }
}