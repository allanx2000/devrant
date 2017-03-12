using DevRant.Dtos;
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Threading;
using DevRant.Enums;
using System.Windows.Media;

namespace DevRant.WPF.ViewModels
{
    public class Comment : FeedItem, Votable, ProfileSection, HasAvatar, HasUsername, Commentable
    {

        private Dtos.Comment comment;

        //ID of the comment itself
        public long RantId { get { return comment.RantId; } }
        public string Text { get { return comment.Text; } }
        public int Votes {
            get {
                return comment.Score;
            }
        }

        public string VotesString
        {
            get
            {
                return Votes.ToString("N0");
            }
        }

        public string Username { get { return comment.Username; } }

        public int UserScore { get { return comment.UserScore; } }
        public string UserScoreString {
            get {
                return UserScore.ToString("N0");
            }
        }

        public string CreateTime { get; private set; }
        
        public string PostURL { get { return Utilities.BaseURL + "rants/" + comment.Id; } }
        
        public VoteState Voted
        {
            get
            {
                return comment.Voted;
            }
        }

        public void SetVoted(VoteState voted)
        {
            comment.Voted = voted;
            RaisePropertyChanged("Voted");
        }

        public ImageSource Picture {
            get {
                return Get<ImageSource>(); }
            private set
            {
                Set(value);
                RaisePropertyChanged();
            }
        }

        public Visibility CanAnimate
        {
            get { return Get<Visibility>(); }
            private set
            {
                Set(value);
                RaisePropertyChanged();
            }
        }

        public int ID { get { return comment.Id; } }

        public long RawCreateTime { get { return comment.CreatedTime; } }

        public string AvatarImage
        {
            get
            {
                return comment.AvatarImage;
            }
        }
        
        public Comment(Dtos.Comment comment) : base(FeedItemType.Comment, comment.CreatedTime)
        {
            this.comment = comment;
            DateTime dt = Utilities.FromUnixTime(comment.CreatedTime);
            CreateTime = dt.ToLocalTime().ToString("M/d/yyyy h:mm tt");
            CanAnimate = Visibility.Collapsed;

            if (comment.Image != null)
            {
                Action<ImageSource, Visibility> cb = (img, vis) =>
                {
                    Picture = img;
                    CanAnimate = vis;
                };

                Thread th = new Thread(() => LoadImage(comment.Image, cb));
                th.Start();
            }
        }
        
        internal void Update(Dtos.Comment updated)
        {
            comment = updated;
            RaisePropertyChanged("Text");
            RaisePropertyChanged("Votes");
            RaisePropertyChanged("VotesString");
            RaisePropertyChanged("UserScore");
            RaisePropertyChanged("UserScoreString");
            RaisePropertyChanged("Voted");
        }

        //TODO: Put in Utilities
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
        }
    }
}