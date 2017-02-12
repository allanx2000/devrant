using DevRant.Dtos;
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Threading;

namespace DevRant.WPF.ViewModels
{
    public class Rant : FeedItem
    {

        private RantInfo rant;
        
        public string Text { get { return rant.Text; } }
        public int Votes { get { return rant.NrOfUpvotes - rant.NrOfDownvotes; } }

        public string Username { get { return rant.Username; } }

        public int UserScore { get { return rant.UserScore; } }

        public string CreateTime { get; private set; }


        public const string BaseURL = "https://www.devrant.io/";
        public string PostURL { get { return BaseURL + "rants/" + rant.Id; } }

        public string ProfileURL { get { return BaseURL + "users/" + rant.Username; } }

        public Visibility TagsVisibility { get { return string.IsNullOrEmpty(TagsString) ? Visibility.Hidden : Visibility.Visible; } }

        public int CommentsCount { get { return rant.NrOfComments; } }

        public string TagsString { get { return string.Join(", ", rant.Tags);  } }

        public BitmapImage Picture { get; private set; }
        
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

        public Rant(RantInfo rant) : base(FeedItemType.Post)
        {
            this.rant = rant;
            DateTime dt = Utilities.FromUnixTime(rant.CreatedTime);
            //CreateTime = dt.ToString("M/d/yyyy");

            Thread th = new Thread(() => LoadImage());
            th.Start();
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