using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace DevRant.Dtos
{
    /// <summary>
    /// Represents a data-transfer-object used for basic information about a rant.
    /// </summary>
    public class Rant : ContentObject
    {
        /// <summary>
        /// Number of Comments
        /// </summary>
        public int NrOfComments { get { return Get<int>("num_comments"); } }

        private TagsCollection tags = null;
        private bool tagsLoaded;

        /// <summary>
        /// Tags
        /// </summary>
        public TagsCollection Tags
        {
            get
            {
                if (!tagsLoaded)
                {
                    string str = Get<string>("tags");
                    if (str != null)
                    {
                        tags = JArray.Parse(str).ToObject<TagsCollection>();
                    }
                    tagsLoaded = true;
                }

                return tags;
            }
        }

        /// <summary>
        /// Tags Collection
        /// </summary>
        public class TagsCollection : List<string>
        {
        }
    }
}
