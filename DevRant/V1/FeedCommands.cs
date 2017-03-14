using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevRant.Enums;
using DevRant.Dtos;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace DevRant.V1
{

    /// <summary>
    /// 
    /// </summary>
    public class FeedCommands : IFeeds
    {
        private readonly DevRantClient owner;
        private readonly HttpClient client;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner"></param>
        public FeedCommands(DevRantClient owner, HttpClient client)
        {
            this.owner = owner;
            this.client = client;
        }

        /// <summary>
        /// Requests a collection of stories sorted and selected by the arguments from the rest-api.
        /// </summary>
        /// <inheritdoc />
        public async Task<IReadOnlyCollection<Rant>> GetStoriesAsync(RantSort sort = RantSort.Top, StoryRange range = StoryRange.Day, int limit = 50, int skip = 0)
        {
            var sortText = sort.ToString().ToLower();
            var rangeText = range.ToString().ToLower();

            string url = owner.MakeUrl("/api/devrant/story-rants", new Parameters()
            {
                {"range", rangeText},
                {"sort", sortText},
                {"limit", limit.ToString()},
                {"skip", skip.ToString()},
            });

            var response = await client.GetAsync(url);
            var responseText = await response.Content.ReadAsStringAsync();

            JObject tmp = JObject.Parse(responseText);

            if (owner.CheckSuccess(tmp))
            {
                List<Rant> rants = new List<Rant>();

                foreach (JObject obj in tmp["rants"].Children())
                {
                    var r = ContentObject.Parse<Rant>(obj);
                    rants.Add(r);
                }

                return rants;
            }
            else
                return null;
        }

        /// <summary>
        /// Returns a list of Collabs
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="skip"></param>
        /// <returns></returns>
        public async Task<IReadOnlyCollection<Collab>> GetCollabsAsync(int limit = 50, int skip = 0)
        {
            string url = owner.MakeUrl("/api/devrant/collabs", new Parameters()
            {
                {"limit", limit.ToString()},
                {"skip", skip.ToString()},
            });

            var response = await client.GetAsync(url);
            var responseText = await response.Content.ReadAsStringAsync();

            JObject tmp = JObject.Parse(responseText);

            if (owner.CheckSuccess(tmp))
            {
                List<Collab> results = new List<Collab>();

                foreach (JObject obj in tmp["rants"].Children())
                {
                    var r = ContentObject.Parse<Collab>(obj);
                    results.Add(r);
                }

                return results;
            }
            else
                return null;
        }

        /// <summary>
        /// Requests a collection of rants sorted and selected by the arguments from the rest-api.
        /// </summary>
        /// <param name="sort">Sorting of the rant collection.</param>
        /// <param name="limit">Maximal rants to return.</param>
        /// <param name="skip">Number of rants to skip.</param>
        /// <inheritdoc />
        public async Task<IReadOnlyCollection<Rant>> GetRantsAsync(RantSort sort = RantSort.Algo, int limit = 50, int skip = 0, ValuesCollection settings = null)
        {
            var sortText = sort.ToString().ToLower();

            string url = owner.MakeUrl("/api/devrant/rants", new Parameters()
            {
                {"sort", sortText},
                {"limit", limit.ToString()},
                {"skip", skip.ToString()},
            });

            var response = await client.GetAsync(url);
            var responseText = await response.Content.ReadAsStringAsync();

            List<Rant> rants = new List<Rant>();

            JObject tmp = JObject.Parse(responseText);
            foreach (JObject obj in tmp["rants"].Children())
            {
                var r = ContentObject.Parse<Rant>(obj);
                rants.Add(r);
            }

            if (settings != null)
            {
                var num = tmp[ValuesCollection.NumNotifs];
                if (tmp[ValuesCollection.NumNotifs] != null)
                {
                    var notifs = Convert.ToInt32(num.ToString());
                    settings[ValuesCollection.NumNotifs] = notifs;
                }

                if (tmp[ValuesCollection.Set] != null)
                {
                    settings[ValuesCollection.Set] = tmp[ValuesCollection.Set];
                }
            }

            return rants;
        }
    }
}
