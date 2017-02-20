using System;
using DevRant.Enums;

namespace DevRant.Dtos
{
    /// <summary>
    /// Object that represents a Vote
    /// </summary>
    public class Vote
    {
        private Vote()
        {

        }

        /// <summary>
        /// Make an UpVote
        /// </summary>
        /// <returns></returns>
        public static Vote UpVote()
        {
            return new Vote()
            {
                State = VoteState.Up
            };
        }

        /// <summary>
        /// Make a vote that cancels the current vote
        /// </summary>
        /// <returns></returns>
        public static Vote ClearVote()
        {
            return new Vote()
            {
                State = VoteState.None
            };
        }

        /// <summary>
        /// Make a Down Vote
        /// </summary>
        /// <param name="reason"></param>
        /// <returns></returns>
        public static Vote DownVote(VoteParam reason)
        {
            return new Vote()
            {
                State = VoteState.Down,
                Reason = reason
            };
        }

        /// <summary>
        /// State
        /// </summary>
        public VoteState State {get; private set;}

        /// <summary>
        /// Reason for down vote
        /// </summary>
        public VoteParam Reason { get; private set; }

        /// <summary>
        /// Returns the State value as a string
        /// </summary>
        /// <returns></returns>
        internal string StateAsString()
        {
            return ((int)State).ToString();
        }

        internal string ReasonAsString()
        {
            return ((int)Reason).ToString();
        }
    }
}