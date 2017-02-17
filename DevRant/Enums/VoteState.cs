namespace DevRant.Enums
{
    /// <summary>
    /// Represents an enum which of how the user votes on a Rant or Comment 
    /// </summary>
    public enum VoteState
    {
        /// <summary>
        /// No vote
        /// </summary>
        None = 0,

        /// <summary>
        /// Upvote
        /// </summary>
        Up = 1,

        /// <summary>
        /// Downvote
        /// </summary>
        Down = -1
    }
}
