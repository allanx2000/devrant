using System;
using System.Runtime.Serialization;

namespace DevRant.Exceptions
{
    /// <summary>
    /// Exception if login credentials are invalid
    /// </summary>
    [Serializable]
    public class InvalidCredentialsException : Exception
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public InvalidCredentialsException() : base("Credentials are invalid")
        {
        }
    }
}