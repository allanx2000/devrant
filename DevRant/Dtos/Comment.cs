using Newtonsoft.Json;
using System.Collections.Generic;

namespace DevRant.Dtos
{
    /// <summary>
    /// Represents a comment.
    /// </summary>
    public class Comment : ContentObject
    {
        /// Represents the rantId of the commend.
        public int RantId { get { return Get<int>("rant_id"); } }

        /// <summary>
        /// Content of Command
        /// </summary>
        public override string Text { get { return Get<string>("body"); } }

    }
}
