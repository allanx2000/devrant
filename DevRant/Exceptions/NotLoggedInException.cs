using System;
using System.Runtime.Serialization;

namespace DevRant.Exceptions
{
    /// <summary>
    /// Exception if login credentials are invalid
    /// </summary>
    [Serializable]
    public class NotLoggedInException : Exception
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public NotLoggedInException() : base("A user is not logged in currently. This function cannot be executed.")
        {
        }
    }
}