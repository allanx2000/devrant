using DevRant.Enums;

namespace DevRant.WPF.ViewModels
{
    public interface Votable
    {
        int ID { get; }
        VoteState Voted { get; }
        void SetVoted(VoteState voted);
    }
}