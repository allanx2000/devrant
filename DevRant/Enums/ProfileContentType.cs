using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevRant.Enums
{
    /// <summary>
    /// Content to return with profile request
    /// </summary>
    public enum ProfileContentType
    {
        /// <summary>
        /// Upvoted Rants (not comments)
        /// </summary>
        Upvoted = 1,
        /// <summary>
        /// 
        /// </summary>
        Rants = 0,
        /// <summary>
        /// 
        /// </summary>
        Comments = 2,

        /// <summary>
        /// Viewed Rants
        /// (Only for logged in user)
        /// </summary>
        Viewed = 3,
        /// <summary>
        /// Favorited Rants
        /// (Only for logged in user)
        /// </summary>
        Favorites = 4
    }
}
