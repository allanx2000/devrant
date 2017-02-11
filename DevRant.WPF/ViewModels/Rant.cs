using DevRant.Dtos;
using System;
using System.Drawing;
using System.Net;
using System.Windows;

namespace DevRant.WPF.ViewModels
{
    public class Rant : FeedItem
    {

        private RantInfo rant;
        
        public string Text { get { return rant.Text; } }
        public int Votes { get { return rant.NrOfUpvotes - rant.NrOfDownvotes; } }

        public string Username { get { return rant.Username; } }

        public string CreateTime { get; private set; }


        public const string BaseURL = "https://www.devrant.io/";
        public string PostURL { get { return BaseURL + "rants/" + rant.Id; } }

        public string ProfileURL { get { return BaseURL + "users/" + rant.Username; } }

        public Visibility TagsVisibility { get { return string.IsNullOrEmpty(TagsString) ? Visibility.Hidden : Visibility.Visible; } }

        public int CommentsCount { get { return rant.NrOfComments; } }

        public string TagsString { get { return string.Join(", ", rant.Tags);  } }

        public Image Picture { get; private set; }

        public Rant(RantInfo rant) : base(FeedItemType.Post)
        {
            this.rant = rant;
            DateTime dt = FromUnixTime(rant.CreatedTime);
            CreateTime = dt.ToString("M/d/yyyy");

            if (rant.Image != null)
            {
                var request = WebRequest.Create(rant.Image.Url);

                using (var response = request.GetResponse())
                using (var stream = response.GetResponseStream())
                {
                     this.Picture = Image.FromStream(stream);
                }
            }
        }

        public static DateTime FromUnixTime(long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }
    }
}