using DevRant.Dtos;
using DevRant.Dtos;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DevRant
{
    /// <summary>
    /// Represents a class for the devrant public api. 
    /// </summary>
    public class DevRantClient : IDevRantClient, IDisposable
    {
        private const int appVersion = 3;
        private const string baseAddress = "https://www.devrant.io/";

        private HttpClient client;

        /// <summary>
        /// Initializes a new <see cref="DevRantClient"/>.
        /// </summary>
        public DevRantClient()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(baseAddress);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<NotificationInfo>> GetNotificationsAsync()
        {
            if (!LoggedIn)
                throw new Exception("User not logged in.");
            

            var response = await client.GetAsync($"/api/users/me/notif-feed?app={appVersion}&user_id={token.UserID}&token_id={token.ID}&token_key={token.Key}");
            var responseText = await response.Content.ReadAsStringAsync();

            JObject obj = JObject.Parse(responseText);

            if (Success(obj))
            {
                var notifs = obj["data"]["items"].AsJEnumerable();
                Dictionary<long, string> users = GetUsernameMap(obj["data"]["username_map"]);

                List<NotificationInfo> list = new List<NotificationInfo>();
                foreach (JObject n in notifs)
                {
                    var notif = n.ToObject<NotificationInfo>();

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
         
            /*
            JObject obj = JObject.Parse(responseText);
            Profile profile = obj["profile"].ToObject<Profile>();

            //Add Rants
            JObject content = obj["profile"]["content"]["content"] as JObject;

            JArray rants = content["rants"] as JArray;

            if (rants != null)
            {
                List<RantInfo> rantsList = new List<RantInfo>();
                foreach (var r in rants)
                {
                    RantInfo info = r.ToObject<RantInfo>();
                    rantsList.Add(info);
                }

                profile.Rants = rantsList;
            }

            return profile;
            */
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

        private bool Success(JObject obj)
        {
            return obj["success"].ToObject<bool>();
        }

        /// <summary>
        /// Requests profile details to the rest-api.
        /// </summary>
        /// <param name="username">Username of the profile to request.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="username"/> is empty.</exception>
        /// <inheritdoc />
        public async Task<Profile> GetProfileAsync(string username)
        {
            if (string.IsNullOrEmpty(username)) throw new ArgumentException("Must be non-empty.", nameof(username));

            var userId = await GetUserId(username);

            if (userId == null)
            {
                return null;
            }

            string url = MakeUrl($"/api/users/{userId}");
            var response = await client.GetAsync(url);
            var responseText = await response.Content.ReadAsStringAsync();

            JObject obj = JObject.Parse(responseText);
            Profile profile = obj["profile"].ToObject<Profile>();

            //Add Rants
            JObject content = obj["profile"]["content"]["content"] as JObject;

            JArray rants = content["rants"] as JArray;

            if (rants != null)
            {
                List<RantInfo> rantsList = new List<RantInfo>();
                foreach (var r in rants)
                {
                    RantInfo info = r.ToObject<RantInfo>();
                    rantsList.Add(info);
                }

                profile.Rants = rantsList;
            }

            return profile;
        }


        private const string TokenId = "token_id";
        private const string TokenKey = "token_key";
        
        private string MakeUrl(string path, Parameters otherParams = null)
        {
            Parameters paramz = new Parameters();
            paramz.Add("app", appVersion.ToString());

            if (otherParams != null)
                paramz.AddRange(otherParams);

            if (LoggedIn)
            {
                paramz.Add(TokenKey, token.Key);
                paramz.Add(TokenId, token.ID);
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(path);
            sb.Append("?" + paramz.ToQueryString());

            var url = sb.ToString();
            return url;
        }


        /// <summary>
        /// Requests a collection of stories sorted and selected by the arguments from the rest-api.
        /// </summary>
        /// <inheritdoc />
        public async Task<IReadOnlyCollection<RantInfo>> GetStoriesAsync(RantSort sort = RantSort.Top, StoryRange range = StoryRange.Day, int limit = 50, int skip = 0)
        {
            var sortText = sort.ToString().ToLower();
            var rangeText = range.ToString().ToLower();

            string url = MakeUrl("/api/devrant/story-rants", new Parameters()
            {
                {"range", rangeText},
                {"sort", sortText},
                {"limit", limit.ToString()},
                {"skip", skip.ToString()},                
            });

            var response = await client.GetAsync(url);
            var responseText = await response.Content.ReadAsStringAsync();

            return ParseProperty<List<RantInfo>>(responseText, "rants");
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

            FormUrlEncodedContent content = CreatePostBody(new Parameters() {
                { "username", username},
                { "password", password}
            });

            var response = await client.PostAsync(url, content);
            var responseText = await response.Content.ReadAsStringAsync();

            JObject obj = JObject.Parse(responseText);

            if (obj["success"].Value<bool>())
            {
                token = ParseProperty<AccessInfo>(responseText, "auth_token");
                token.Username = username;
            }
            else
                throw new Exception("Unable to login");
        }

        private static FormUrlEncodedContent CreatePostBody(Parameters otherParams)
        {            
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("app", "3");
            data.Add("plat", "3");

            foreach (var kv in otherParams)
            {
                data.Add(kv.Key, kv.Value);
            }

            FormUrlEncodedContent content = new FormUrlEncodedContent(data);
            return content;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task Logout()
        {
            if (LoggedIn)
            {
                //TODO: Logout
                token = null;
            }
        }


        /// <summary>
        /// Requests a collection of rants sorted and selected by the arguments from the rest-api.
        /// </summary>
        /// <param name="sort">Sorting of the rant collection.</param>
        /// <param name="limit">Maximal rants to return.</param>
        /// <param name="skip">Number of rants to skip.</param>
        /// <inheritdoc />
        public async Task<IReadOnlyCollection<RantInfo>> GetRantsAsync(RantSort sort = RantSort.Algo, int limit = 50, int skip = 0)
        {
            var sortText = sort.ToString().ToLower();

            string url = MakeUrl("/api/devrant/rants", new Parameters()
            {
                {"sort", sortText},
                {"limit", limit.ToString()},
                {"skip", skip.ToString()},
            });

            var response = await client.GetAsync(url);
            var responseText = await response.Content.ReadAsStringAsync();

            return ParseProperty<List<RantInfo>>(responseText, "rants");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private async Task<int?> GetUserId(string username)
        {
            var response = await client.GetAsync($"/api/get-user-id?app={appVersion}&username={username}");
            var responseText = await response.Content.ReadAsStringAsync();

            return ParseProperty<int?>(responseText, "user_id");
        }

        private T ParseProperty<T>(string jsonText, string propertyName)
        {
            var obj = JObject.Parse(jsonText);
            var prop = obj.GetValue(propertyName);

            return prop == null
                ? default(T)
                : prop.ToObject<T>();
        }

        /// <summary>
        /// Releases all used resources by the <see cref="DevRantClient"/> instance.
        /// </summary>
        public void Dispose()
        {
            client?.Dispose();
            client = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<bool> IsValidUser(string username)
        {
            int? id = await GetUserId(username);
            return id != null;
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
        public string LoggedInUser {
            get
            {
                return token == null ? null : token.Username;
            }
        }
    }
}
