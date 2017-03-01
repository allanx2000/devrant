using DevRant.Dtos;
using DevRant.Enums;
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;

namespace DevRant.WPF.ViewModels
{
    public class Collab : FeedItem, Commentable, Votable
    {
        private Dtos.Collab collab;


        public Collab(Dtos.Collab collab) : base(FeedItemType.Collab, collab.CreatedTime)
        {
            this.collab = collab;

            DateTime dt = Utilities.FromUnixTime(collab.CreatedTime);
            CreateTime = dt.ToLocalTime().ToString("M/d/yyyy h:mm tt");

            if (collab.Image != null)
            {
                Thread th = new Thread(() => LoadImage());
                th.Start();
            }

        }


        public long RantId { get { return collab.Id; } }

        public string Text { get { return collab.Text; } }
        public int Votes
        {
            get
            {
                return collab.Score;
            }
        }

        public string VotesString
        {
            get
            {
                return Votes.ToString("N0");
            }
        }

        public string Username { get { return collab.Username; } }

        public int UserScore { get { return collab.UserScore; } }
        public string UserScoreString
        {
            get
            {
                return UserScore.ToString("N0");
            }
        }

        public string CreateTime { get; private set; }

        public string PostURL { get { return Utilities.BaseURL + "rants/" + collab.Id; } }

        public string ProfileURL { get { return Utilities.BaseURL + "users/" + collab.Username; } }

        public Visibility TagsVisibility { get { return string.IsNullOrEmpty(TagsString) ? Visibility.Hidden : Visibility.Visible; } }

        public int CommentsCount { get { return collab.NrOfComments; } }

        public string TagsString { get { return string.Join(", ", collab.Tags); } }

        public string TypeString { get { return collab.CollabTypeString; } }

        public VoteState Voted
        {
            get
            {
                return collab.Voted;
            }
        }

        public void SetVoted(VoteState voted)
        {
            collab.Voted = voted;
            RaisePropertyChanged("Voted");
        }

        public BitmapImage Picture
        {
            get
            {
                return Get<BitmapImage>();
            }
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

        public int ID { get { return collab.Id; } }

        public long RawCreateTime { get { return collab.CreatedTime; } }
        
        private void LoadImage()
        {
            if (collab.Image != null)
            {
                var request = WebRequest.Create(collab.Image.Url);

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

        internal void Update(Dtos.Collab updated)
        {
            collab = updated;
            RaisePropertyChanged("Text");
            RaisePropertyChanged("Votes");
            RaisePropertyChanged("VotesString");
            RaisePropertyChanged("UserScore");
            RaisePropertyChanged("UserScoreString");
            RaisePropertyChanged("CommentsCount");
            RaisePropertyChanged("Voted");
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

        public void IncrementComments()
        {
            collab.NrOfComments += 1;
            RaisePropertyChanged("CommentsCount");
        }
    }
}