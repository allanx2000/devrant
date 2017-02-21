namespace DevRant.WPF
{
    public interface Commentable
    {
        long RantId { get; }
        void IncrementComments();
    }
}