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
    public class Rant : FeedItem, Commentable, Votable, ProfileSection, HasAvatar, HasUsername
    {

        private Dtos.Rant rant;
        
        public string AvatarImage { get { return rant.AvatarImage; } }

        public long RantId { get { return rant.Id; } }
        public string Text { get { return rant.Text; } }
        public int Votes {
            get {
                return rant.Score;
                //return rant.NrOfUpvotes - rant.NrOfDownvotes;
            }
        }

        public string FavoriteText
        {
            get { return rant.Favorited ? "Unfavorite" : "Favority"; }
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

        public ImageSource Picture {
            get {
                return Get<ImageSource>(); }
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
        
        public Rant(Dtos.Rant rant) : base(FeedItemType.Post, rant.CreatedTime)
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

                try
                {
                    Image bmp = Utilities.GetImage(rant.Image.Url);
                    App.Current.Dispatcher.Invoke(() => Picture = Utilities.GetImageSource(bmp));
                }
                catch (Exception e)
                {
                    //Timeout?
                }
            }
        }

        internal void Update(Dtos.Rant updated)
        {
            rant = updated;
            RaisePropertyChanged("Text");
            RaisePropertyChanged("Votes");
            RaisePropertyChanged("VotesString");
            RaisePropertyChanged("UserScore");
            RaisePropertyChanged("UserScoreString");
            RaisePropertyChanged("CommentsCount");
            RaisePropertyChanged("Voted");
            RaisePropertyChanged("FavoriteText");
        }
        
        public void IncrementComments()
        {
            rant.NrOfComments+=1;
            RaisePropertyChanged("CommentsCount");
        }
    }
}