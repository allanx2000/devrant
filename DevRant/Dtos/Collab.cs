using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace DevRant.Dtos
{
    /// <summary>
    /// Represents a Collab.
    /// </summary>
    public class Collab : Rant
    {
        /// <summary>
        /// ???
        /// </summary>
        public int RT
        {
            get { return Get<int>("rt"); }
        }

        /// <summary>
        /// ???
        /// </summary>
        public int CollabType
        {
            get { return Get<int>("c_type"); }
        }

        /// <summary>
        /// 
        /// </summary>
        public string CollabTypeString
        {
            get { return Get<string>("c_type_long"); }
        }


    }
}
