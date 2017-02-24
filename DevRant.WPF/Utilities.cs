using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DevRant.WPF
{
    public static class Utilities
    {
        public const string BaseURL = "https://www.devrant.io/";
        
        public static long ToUnixTime(DateTime time)
        {
            long unixTimestamp = (long)(time.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            return unixTimestamp;
        }

        public static DateTime FromUnixTime(long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }
        
        public static string GetProfileUrl(string name)
        {
            return string.Concat(BaseURL, "/users/", name);
        }
        
        public static void OpenProfile(string username, Window owner, IDevRantClient api)
        {
            ProfileViewerWindow window = new ProfileViewerWindow(username, api);
            window.Owner = owner;
            window.ShowDialog();
        }

        public static void OpenProfile(string username)
        {
                string url = Utilities.GetProfileUrl(username);
                Process.Start(url);
        }
    }
}
