using Newtonsoft.Json;

namespace DevRant.Dtos
{
    /// <summary>
    /// Holds the info for successful login
    /// </summary>
    public class AccessInfo
    {
        /// <summary>
        /// ID
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; }

        /// <summary>
        /// Key
        /// </summary>
        [JsonProperty("key")]
        public string Key { get; set; }

        /// <summary>
        /// Expire Time
        /// </summary>
        [JsonProperty("expire_time")]
        public string ExpireTime { get; set; }

        /// <summary>
        /// User ID
        /// </summary>
        [JsonProperty("user_id")]
        public string UserID { get; set; }

        /// <summary>
        /// Username, populated by DevRantClient on successful login
        /// </summary>
        public string Username { get; set; }
    }
}