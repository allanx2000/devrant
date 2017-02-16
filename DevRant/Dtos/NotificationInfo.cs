using Newtonsoft.Json;

namespace DevRant.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    public class NotificationInfo
    {
        /// <summary>
        /// Create Time
        /// </summary>
        [JsonProperty("created_time")]
        public long CreateTime { get; set; }
        
        /// <summary>
        /// Type of notification
        /// </summary>
        [JsonProperty("type")]
        public string NotificationType { get; set; }

        /// <summary>
        /// If Read, it will be 1
        /// </summary>
        [JsonProperty("read")]
        public int Read { get; set; }

        /// <summary>
        /// Whether this is read or not
        /// </summary>
        public bool IsRead
        {
            get
            {
                return Read == 1;
            }
        }

        /// <summary>
        /// The ID of the rant this notification is for
        /// </summary>
        [JsonProperty("rant_id")]
        public long RantId { get; set; }

        /// <summary>
        /// The User ID of the user that performed the action that created this notification
        /// </summary>
        [JsonProperty("uid")]
        public long? ActionUser { get; set; }

        /// <summary>
        /// Action username, needs to be populated manually by API
        /// </summary>
        public string ActionUsername { get; set; }
    }
}