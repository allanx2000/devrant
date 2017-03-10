using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using Newtonsoft.Json.Linq;

namespace DevRant.Dtos
{
    /// <summary>
    /// Holds the Settings information
    /// </summary>

    public class SettingsCollection : Dictionary<string, object>
    {
        /// <summary>
        /// 
        /// </summary>
        public const string NumNotifs = "num_notifs";

        /// <summary>
        /// 
        /// </summary>
        public const string Set = "set";

        internal void AddAll(JObject obj = null)
        {
            if (obj != null)
            {
                foreach (JProperty prop in obj.Children())
                {
                    this.Add(prop.Name, Utilities.GetValue(prop.Value));
                }
            }
        }
        
        internal void AddAll(SettingsCollection other)
        {
            foreach (var kv in other)
            {
                this.Add(kv.Key, kv.Value);
            }
        }
    }
}