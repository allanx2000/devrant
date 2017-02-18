using DevRant.Dtos;
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Threading;
using DevRant.Enums;

namespace DevRant.WPF.ViewModels
{
    public class Rant : FeedItem
    {

        private Dtos.Rant rant;
        
        public string Text { get { return rant.Text; } }
        public int Votes {
            get {
                return rant.Score;
                //return rant.NrOfUpvotes - rant.NrOfDownvotes;
            }
        }

        public string VotesString
        {
            get
            {
                return Votes.ToString("N0");
            }
        }

        public string Username { get { return rant.Username; } }

        public int UserScore { get { return rant.UserScore; } }
        public string UserScoreString {
            get {
                return UserScore.ToString("N0");
            }
        }

        public string CreateTime { get; private set; }
        
        public string PostURL { get { return Utilities.BaseURL + "rants/" + rant.Id; } }

        public string ProfileURL { get { return Utilities.BaseURL + "users/" + rant.Username; } }

        public Visibility TagsVisibility { get { return string.IsNullOrEmpty(TagsString) ? Visibility.Hidden : Visibility.Visible; } }

        public int CommentsCount { get { return rant.NrOfComments; } }

        public string TagsString { get { return string.Join(", ", rant.Tags);  } }

        public VoteState Voted
        {
            get
            {
                return rant.Voted;
            }
        }

        public void SetVoted(VoteState voted)
        {
            rant.Voted = voted;
            RaisePropertyChanged("Voted");
        }

        public BitmapImage Picture {
            get {
                return Get<BitmapImage>(); }
            private set
            {
                Set(value);
                RaisePropertyChanged();
            }
        }
        
        public bool Followed
        {
            get
            {
                return Get<bool>();
            }
            set
            {
                Set(value);
                RaisePropertyChanged();
            }
        }

        public int ID { get { return rant.Id; } }

        public long RawCreateTime { get { return rant.CreatedTime; } }

        public Rant(Dtos.Rant rant) : base(FeedItemType.Post)
        {
            this.rant = rant;
            DateTime dt = Utilities.FromUnixTime(rant.CreatedTime);
            CreateTime = dt.ToLocalTime().ToString("M/d/yyyy h:mm tt");

            if (rant.Image != null)
            {
                Thread th = new Thread(() => LoadImage());
                th.Start();
            }
        }

        private void LoadImage()
        {
            if (rant.Image != null)
            {
                var request = WebRequest.Create(rant.Image.Url);

                try
                {
                    using (var response = request.GetResponse())
                    using (var stream = response.GetResponseStream())
                    {
                        Bitmap bitmap = new Bitmap(stream);

                        App.Current.Dispatcher.Invoke(() => Picture = BitmapToImageSource(bitmap));
                    }
                }
                catch (Exception e)
                {
                    //Timeout?
                }
            }
        }
        
        BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }

       
    }
}