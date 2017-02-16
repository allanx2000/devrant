using Newtonsoft.Json;
using System.Collections.Generic;

namespace DevRant.Dtos
{
    /// <summary>
    /// Represents a data-transfer-object used for basic information about a rant.
    /// </summary>
    public class RantInfo : BaseInfo
    {
        /// <summary>
        /// Comments
        /// </summary>
        [JsonProperty("num_comments")]
        public int NrOfComments { get; set; }

        /// <summary>
        /// Tags
        /// </summary>
        public TagsCollection Tags { get; set; }

        /// <summary>
        /// Tags Collection
        /// </summary>
        [JsonArray("tags")]
        public class TagsCollection : List<string>
        {
        }
    }
}
