namespace DevRant.WPF
{
    public class LoginInfo
    {
        public LoginInfo(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public string Password { get; internal set; }
        public string Username { get; internal set; }
    }
}