using DevRant.Dtos;
using DevRant.Exceptions;
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
                throw new NotLoggedInException();

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

        //TODO: Return new score
        public async Task<Rant> VoteRant(long rantId, Vote vote)
        {
            string url = string.Concat(Constants.BaseAddress, "api/devrant/rants/", rantId, "/vote");
            
            var paramz = new Parameters();
            paramz.Add("vote", vote.StateAsString());

            if (vote.State == Enums.VoteState.Down)
            {
                paramz.Add("reason", vote.ReasonAsString());
            }

            var body = owner.CreatePostBody(paramz);
            
            var response = await client.PostAsync(url, body);
            var responseText = await response.Content.ReadAsStringAsync();

            JObject obj = JObject.Parse(responseText);

            if (owner.CheckSuccess(obj))
            {
                var rant = DataObject.Parse<Rant>((JObject) obj["rant"]);
                return rant;
            }
            else
                return null;
        }

        public Task<Comment> VoteComment(long commentId, Vote vote)
        {
            throw new NotImplementedException();
        }

        public async Task UploadRant(PostContent post)
        {
            string url = string.Concat(Constants.BaseAddress, "api/devrant/rants");

            MultipartFormDataContent data = new MultipartFormDataContent();
            data.Add(new StringContent(token.ID), Constants.TokenId);
            data.Add(new StringContent(token.Key), Constants.TokenKey);
            data.Add(new StringContent(token.UserID), Constants.UserId);
            data.Add(new StringContent(post.Text), "rant");
            data.Add(new StringContent(Constants.AppVersion), "app");
            data.Add(new StringContent(Constants.PlatformVersion), "plat");

            if (post.Image != null)
                data.Add(new ByteArrayContent(post.Image), "image", post.GenerateImageName());

            if (!string.IsNullOrEmpty(post.Tag))
                data.Add(new StringContent(post.Tag), "tags");

            var response = await client.PostAsync(url, data);
            var responseText = await response.Content.ReadAsStringAsync();

            JObject obj = JObject.Parse(responseText);

            if (owner.CheckSuccess(obj))
            {
            }
        }
    }
}