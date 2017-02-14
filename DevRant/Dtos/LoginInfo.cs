namespace DevRant.Dtos
{
    /// <summary>
    /// Holds info used to login and the current state
    /// </summary>
    public class LoginInfo
    {
        /// <summary>
        /// Creates a login holder
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public LoginInfo(string username, string password)
        {
            Username = username;
            Password = password;
        }

        /// <summary>
        /// Password, used by WPF
        /// </summary>
        public string Password { get; internal set; }

        /// <summary>
        /// Username
        /// </summary>
        public string Username { get; internal set; }
    }
}