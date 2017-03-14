using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System;

namespace DevRant.Dtos
{
    /// <summary>
    /// Represents a data-transfer-object used for basic information about a rant.
    /// </summary>
    public class Rant : ContentObject, HasAvatar
    {
        /// <summary>
        /// Number of Comments
        /// </summary>
        public int NrOfComments {
            get { return Get<int>("num_comments"); }
            set { Set("num_comments", value); }
        }

        /// <summary>
        /// Is Favorite
        /// </summary>
        public bool Favorited
        {
            get
            {
                return Get<int>("favorited") == 1;
            }
            set
            {
                Set("favorited", value? 1 : 0);
            }
        }

        /// <summary>
        /// Comments, only if returning full Rant
        /// </summary>
        public List<Comment> Comments { get; set; }

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
        /// 
        /// </summary>
        public string AvatarImage
        {
            get
            {
                return base.GetAvatarImage();
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
