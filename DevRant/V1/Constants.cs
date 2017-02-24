using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevRant.V1
{
    /// <summary>
    /// Constants for V1 API
    /// </summary>
    public class Constants
    {
        internal const string AppVersion = "3";
        internal const string PlatformVersion = "3";
        internal const string BaseAddress = "https://www.devrant.io/";

        internal const string UserId = "user_id";
        internal const string TokenId = "token_id";
        internal const string TokenKey = "token_key";

        /// <summary>
        /// Number of items returned in Profile
        /// </summary>
        public const int ProfileDataSkip = 30; //Returns 30 results per request
    }
}
