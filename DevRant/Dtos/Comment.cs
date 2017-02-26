using Newtonsoft.Json;
using System.Collections.Generic;
using System;

namespace DevRant.Dtos
{
    /// <summary>
    /// Represents a comment.
    /// </summary>
    public class Comment : ContentObject, HasAvatar
    {
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

        /// Represents the rantId of the commend.
        public int RantId { get { return Get<int>("rant_id"); } }

        /// <summary>
        /// Content of Command
        /// </summary>
        public override string Text { get { return Get<string>("body"); } }

    }
}
