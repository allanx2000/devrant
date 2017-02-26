using DevRant.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace DevRant.Dtos
{
    /// <summary>
    /// Represents the base items common to content such as rants and comments
    /// </summary>
    public abstract class ContentObject : DataObject
    {
        
        /// <summary>
        /// Represents the identity of the object.
        /// </summary>
        public int Id { get { return Get<int>("id"); } }
        
        #region User Info
        /// <summary>
        /// Username
        /// </summary>
        public string Username { get { return Get<string>("user_username"); } }

        /// <summary>
        /// User ID
        /// </summary>
        [JsonProperty("user_id")]
        public int UserId { get { return Get<int>("user_id"); } }

        /// <summary>
        /// User Score
        /// </summary>
        [JsonProperty("user_score")]
        public int UserScore { get { return Get<int>("user_score"); } }

        #endregion
        
        private ImageInfo image = null;
        /// <summary>
        /// Attached Image
        /// </summary>
        [JsonProperty("attached_image")]
        public ImageInfo Image {
            get {
                if (image == null)
                {
                    string tmp = Get<string>("attached_image");

                    if (!string.IsNullOrEmpty(tmp))
                    {
                        image = JObject.Parse(tmp).ToObject<ImageInfo>();
                    }
                }

                return image;
            }
        }

        /// <summary>
        /// Represents the text of the object.
        /// </summary>
        public virtual string Text { get { return Get<string>("text"); } }

        /// <summary>
        /// Vote State
        /// </summary>
        public VoteState Voted
        {
            get
            {
                int voted = Get<int>("vote_state");
                return (VoteState)voted;
            }
            set
            {
                Set("vote_state", (int)value);
            }
        }

        /// <summary>
        /// Represents the number of upvotes of the rant.
        /// </summary>
        public int NrOfUpvotes { get { return Get<int>("num_upvotes"); } }

        /// <summary>
        /// Represents the number of downvotes of the rant.
        /// </summary>
        public int NrOfDownvotes { get { return Get<int>("num_downvotes"); } }

        /// <summary>
        /// Score
        /// </summary>
        public int Score { get { return Get<int>("score"); } }
        
        /// <summary>
        /// Edited
        /// </summary>
        public bool Edited { get { return Get<bool>("edited"); } }
    }
}
