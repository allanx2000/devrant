using DevRant.Dtos;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DevRant.V1
{
    internal class UserCommands : IUserCommands
    {
        private HttpClient client;
        private DevRantClient owner;

        public UserCommands(DevRantClient owner, HttpClient client)
        {
            
            this.owner = owner;
            this.client = client;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<Notification>> GetNotificationsAsync()
        {
            if (!LoggedIn)
                throw new Exception("User not logged in.");

            var response = await client.GetAsync($"/api/users/me/notif-feed?app={Constants.AppVersion}&user_id={token.UserID}&token_id={token.ID}&token_key={token.Key}");
            var responseText = await response.Content.ReadAsStringAsync();

            JObject obj = JObject.Parse(responseText);

            if (owner.CheckSuccess(obj))
            {
                var notifs = obj["data"]["items"].AsJEnumerable();
                Dictionary<long, string> users = GetUsernameMap(obj["data"]["username_map"]);

                List<Notification> list = new List<Notification>();
                foreach (JObject n in notifs)
                {
                    var notif = ContentObject.Parse<Notification>(n);

                    if (notif.ActionUser != null)
                    {
                        notif.ActionUsername = users[notif.ActionUser.Value];
                    }

                    list.Add(notif);
                }

                return list;
            }
            else
                return null;
        }

        private Dictionary<long, string> GetUsernameMap(JToken jToken)
        {
            Dictionary<long, string> map = new Dictionary<long, string>();

            foreach (JProperty child in jToken)
            {
                long id = Convert.ToInt64(child.Name);
                string userId = child.Value.ToString();

                map.Add(id, userId);
            }

            return map;
        }
        

        /// <summary>
        /// Checks the credentials are valid
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public async Task Login(string username, string password)
        {
            if (LoggedIn)
            {
                await Logout();
            }

            string url = "/api/users/auth-token";

            FormUrlEncodedContent content = owner.CreatePostBody(new Parameters() {
                { "username", username},
                { "password", password}
            });

            var response = await client.PostAsync(url, content);
            var responseText = await response.Content.ReadAsStringAsync();

            JObject obj = JObject.Parse(responseText);

            if (owner.CheckSuccess(obj))
            {
                token = owner.ParseProperty<AccessInfo>(responseText, "auth_token");
                token.Username = username;
            }
            else
                throw new Exception("Unable to login");
        }

        private AccessInfo token;

        /// <summary>
        /// Whether the client is logged in or not
        /// </summary>
        public bool LoggedIn
        {
            get { return token != null; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string LoggedInUser
        {
            get
            {
                return token == null ? null : token.Username;
            }
        }

        public AccessInfo Token
        {
            get
            {
                return token;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task Logout()
        {
            if (LoggedIn)
            {
                token = null;
            }
        }

    }
}