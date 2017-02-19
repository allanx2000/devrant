using DevRant.Dtos;

namespace DevRant.WPF.ViewModels
{
    internal class Collab : FeedItem
    {
        private Dtos.Collab collab;

        public Collab(Dtos.Collab collab) : base(FeedItemType.Collab)
        {
            this.collab = collab;
        }
    }
}