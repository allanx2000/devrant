using Newtonsoft.Json;
using System;

namespace DevRant.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    public class Notification : DataObject
    {
        
        /// <summary>
        /// Type of notification
        /// </summary>
        [JsonProperty("type")]
        public string NotificationType { get { return Get<string>("type");  } }

        /// <summary>
        /// RantId
        /// </summary>
        public long RantId { get {

                object i = Get("rant_id");
                return Convert.ToInt64(i);
            }
        }


        /// <summary>
        /// If Read, it will be 1
        /// </summary>
        [JsonProperty("read")]
        public int Read
        {
            get { return Get<int>("read"); }
            set { base.Set("read", 1); }
        }

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
        /// The User ID of the user that performed the action that created this notification
        /// </summary>
        [JsonProperty("uid")]
        public long? ActionUser //Should be int
        {
            get {
                object i = Get("uid");

                if (i == null)
                    return null;
                else
                   return Convert.ToInt32(i);
            }
        }

        /// <summary>
        /// Action username, needs to be populated manually by API
        /// </summary>
        public string ActionUsername { get; set; }
    }
}