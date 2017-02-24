using DevRant.Dtos;
using DevRant.Dtos;
using DevRant.Enums;
using DevRant.Exceptions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DevRant.V1
{
    /// <summary>
    /// Represents a class for the devrant public api. 
    /// </summary>
    public class DevRantClient : IDevRantClient, IDisposable
    {
        private readonly IFeeds feeds;
        private readonly IUserCommands user;

        private HttpClient client;
        
        /// <summary>
        /// Initializes a new <see cref="DevRantClient"/>.
        /// </summary>
        public DevRantClient()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(Constants.BaseAddress);

            feeds = new FeedCommands(this, client);
            user = new UserCommands(this, client);
        }

        /// <summary>
        /// 
        /// </summary>
        public IFeeds Feeds
        {
            get
            {
                return feeds;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IUserCommands User
        {
            get
            {
                return user;
            }
        }


        /// <summary>
        /// Requests profile details to the rest-api.
        /// </summary>
        /// <param name="username">Username of the profile to request.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="username"/> is empty.</exception>
        /// <inheritdoc />
        public async Task<Profile> GetProfileAsync(string username, ProfileContentType type = ProfileContentType.Rants, int skip = 0)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentException("Must be non-empty.", nameof(username));
    
            var userId = await GetUserId(username);

            if (userId == null)
            {
                return null;
            }

            Parameters paramz = new Parameters();
            paramz.Add("content", type.ToString().ToLower());
            paramz.Add("skip", skip.ToString());

            string url = MakeUrl($"/api/users/{userId}", paramz);
            var response = await client.GetAsync(url);
            var responseText = await response.Content.ReadAsStringAsync();

            JObject obj = JObject.Parse(responseText);
            Profile profile = obj["profile"].ToObject<Profile>();

            //Add Data
            JObject content = obj["profile"]["content"]["content"] as JObject;

            string sectionName = type.ToString().ToLower();

            JObject counts = obj["profile"]["content"]["counts"] as JObject;
            profile.RantsCount = GetCount(counts, "rants");
            profile.CommentsCount = GetCount(counts, "comments");
            profile.UpvotedCount = GetCount(counts, "upvoted");
            profile.FavoritesCount = GetCount(counts, "favorites");
            profile.ViewedCount = GetCount(counts, "viewed");

            //TODO: Collab

            JArray data = content[sectionName] as JArray;

            if (data != null)
            {
                List<Rant> rantsList = new List<Rant>();
                List<Comment> commentsList = new List<Comment>();
                
                foreach (JObject i in data)
                {
                    switch (type)
                    {
                        case ProfileContentType.Rants:
                        case ProfileContentType.Favorites:
                        case ProfileContentType.Upvoted:
                        case ProfileContentType.Viewed:
                            Rant rant = DataObject.Parse<Rant>(i);
                            rantsList.Add(rant);
                            break;
                        case ProfileContentType.Comments:
                            Comment comment = DataObject.Parse<Comment>(i);
                            commentsList.Add(comment);
                            break;
                    }
                }

                profile.Rants = rantsList;
                profile.Comments = commentsList;
            }

            return profile;
        }

        private int GetCount(JObject obj, string name)
        {
            if (obj == null || obj[name] == null)
                return 0;
            else
                return Convert.ToInt32(obj[name].ToString());
        }

        #region API Utils 

        private const string InvalidCredentials = "Invalid user credentials.";
        
        /// <summary>
        /// Check if the response is a success, otherwise throws and exception
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CheckSuccess(JObject obj)
        {
            bool success = obj["success"].ToObject<bool>();
            if (!success)
            {
                var error = obj["error"];
                if (error == null)
                    throw new Exception("Failed with no error details.");

                string reason = error.ToString();
                if (reason == InvalidCredentials)
                {
                    throw new InvalidCredentialsException();
                }
                else
                    throw new Exception(reason);
            }
            else
                return true;
        }

        internal string MakeUrl(string path, Parameters otherParams = null)
        {
            Parameters paramz = new Parameters();
            paramz.Add("app", Constants.AppVersion);

            if (otherParams != null)
                paramz.AddRange(otherParams);

            if (user.LoggedIn)
            {   
                paramz.Add(Constants.TokenKey, user.Token.Key);
                paramz.Add(Constants.TokenId, user.Token.ID);
                paramz.Add(Constants.UserId, user.Token.UserID);
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(path);
            sb.Append("?" + paramz.ToQueryString());

            var url = sb.ToString();
            return url;
        }

        internal FormUrlEncodedContent CreatePostBody(Parameters otherParams)
        {            
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("app", "3");
            data.Add("plat", "3");

            foreach (var kv in otherParams)
            {
                data.Add(kv.Key, kv.Value);
            }

            if (User.LoggedIn)
            {
                var token = User.Token;
                data.Add(Constants.TokenId, token.ID);
                data.Add(Constants.TokenKey, token.Key);
                data.Add(Constants.UserId, token.UserID);
            }

            FormUrlEncodedContent content = new FormUrlEncodedContent(data);
            return content;
        }

        internal T ParseProperty<T>(string jsonText, string propertyName)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        internal async Task<int?> GetUserId(string username)
        {
            var response = await client.GetAsync($"/api/get-user-id?app={Constants.AppVersion}&username={username}");
            var responseText = await response.Content.ReadAsStringAsync();

            return ParseProperty<int?>(responseText, "user_id");
        }

        #endregion
    }
}
