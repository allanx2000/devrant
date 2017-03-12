using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DevRant.WPF.Controls;
using DevRant.Dtos;
using DevRant.WPF.ViewModels;
using DevRant.Enums;
using DevRant.WPF.DataStore;
using System.Net;
using System.Drawing;
using System.Windows.Media;
using System.IO;
using System.Windows.Media.Imaging;

namespace DevRant.WPF
{
    public static class Utilities
    {
        public const string BaseURL = "https://www.devrant.io/";

        public static double GetLeft(double windowWidth)
        {
            double screenWidth = SystemParameters.PrimaryScreenWidth;

            double left = (screenWidth - windowWidth) / 2;
            return left;
        }

        public static long ToUnixTime(DateTime time)
        {
            long unixTimestamp = (long)(time.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            return unixTimestamp;
        }

        public static DateTime FromUnixTime(long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }

        public static string GetProfileUrl(string name)
        {
            return string.Concat(BaseURL, "/users/", name);
        }

        /// <summary>
        /// Open profile in ProfileViewer
        /// </summary>
        /// <param name="username"></param>
        /// <param name="owner"></param>
        /// <param name="api"></param>
        public static void OpenProfile(string username)
        {
            if (AppManager.Instance.Settings.OpenInProfileViewer)
            {
                ProfileViewerWindow window = new ProfileViewerWindow(username, AppManager.Instance.API);
                window.ShowDialog();
            }
            else
            {
                string url = Utilities.GetProfileUrl(username);
                Process.Start(url);
            }
        }

        internal static bool OpenFeedItem(Commentable item)
        {
            string url = GetRantUrl(item.RantId);
            Process.Start(url);
            return true;
        }

        internal static async Task<bool> OpenFeedItem(FeedItem item, IDevRantClient api, Window owner)
        {
            if (item is ViewModels.Notification)
            {
                var notif = item.AsNotification();
                var raw = await api.GetRant(item.AsNotification().RantId);
                item = new ViewModels.Rant(raw);
            }
            else if (item is ViewModels.Comment)
            {
                var raw = await api.GetRant(item.AsComment().RantId);
                item = new ViewModels.Rant(raw);
            }


            if (item is ViewModels.Rant)
            {
                Window dlg;
                dlg = new RantViewerWindow((ViewModels.Rant)item, api);

                dlg.Owner = owner;
                dlg.ShowDialog();
            }
            else if (item is ViewModels.Collab)
            {
                string url = GetRantUrl(item.AsCollab().RantId);
                Process.Start(url);
            }
            else
                return false;

            return true;
        }

        internal static async Task<bool> HandleButtons(Window window, ButtonClickedEventArgs args)
        {


            EditPostWindow editPost;

            switch (args.Type)
            {
                case ButtonType.Up:
                case ButtonType.Down:
                    await Utilities.Vote(args);
                    break;
                case ButtonType.Reply:
                    if (args.SelectedItem is Commentable)
                    {
                        Commentable ctbl = args.SelectedItem as Commentable;
                        editPost = EditPostWindow.CreateForComment(AppManager.Instance.API, ctbl);
                        editPost.Owner = window;
                        editPost.ShowDialog();

                        if (!editPost.Cancelled)
                        {
                            ctbl.IncrementComments();
                        }
                    }
                    break;
                case ButtonType.Delete:
                    switch (args.SelectedItem.Type)
                    {
                        case FeedItem.FeedItemType.Post:
                            AppManager.Instance.API.User.DeleteRant(args.SelectedItem.AsRant().ID);
                            break;
                        case FeedItem.FeedItemType.Comment:
                            AppManager.Instance.API.User.DeleteComment(args.SelectedItem.AsComment().ID);
                            break;
                    }
                    break;
                case ButtonType.Edit:
                    TimeSpan timespan = DateTime.Now.ToUniversalTime() - FromUnixTime(args.SelectedItem.RawCreateTime);
                    if (timespan > MaxModifyMinutes)
                        throw new Exception(args.SelectedItem.Type + " can no longer be edited.");

                    editPost = EditPostWindow.CreateForEdit(AppManager.Instance.API, args.SelectedItem);
                    editPost.Owner = window;
                    editPost.ShowDialog();
                    break;
            }

            return true;
        }

        internal static Task Vote(ButtonClickedEventArgs args)
        {
            return Vote(args, AppManager.Instance.API, AppManager.Instance.DB);
        }

        internal static string ReplaceNewLines(string text)
        {
            if (string.IsNullOrEmpty(text))
                return "";
            else
                return text.Replace(Environment.NewLine, "\n");
        }

        private static string GetRantUrl(long rantId)
        {
            string url = BaseURL + "/rants/" + rantId;
            return url;
        }

        public static Image GetImage(string url)
        {
            var request = WebRequest.Create(url);
            using (var response = request.GetResponse())
            using (var stream = response.GetResponseStream())
            {
                Bitmap bitmap = new Bitmap(stream);
                return bitmap;
            }
        }

        private static System.Windows.Controls.BooleanToVisibilityConverter bool2Vis = new System.Windows.Controls.BooleanToVisibilityConverter();
        private static readonly TimeSpan MaxModifyMinutes = new TimeSpan(0, 5, 0);

        public static Visibility ConvertToVisibility(bool v)
        {
            return (Visibility) bool2Vis.Convert(v, null, null, null);
        }

        public static ImageSource GetImageSource(Image bitmap)
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

        /// <summary>
        /// Casts a vote
        /// </summary>
        /// <param name="args"></param>
        /// <param name="api">API to use to vote</param>
        /// <param name="db">Optional DB to mark as Read</param>
        /// <returns>Throws exception on errors</returns>
        public static async Task Vote(ButtonClickedEventArgs args, IDevRantClient api, IPersistentDataStore db = null)
        {
            Vote vote = null;

            Votable votable = args.SelectedItem as Votable;

            if (votable.Voted == VoteState.Disabled)
                return;
                        
            if (votable != null)
            {
                switch (args.Type)
                {
                    case ButtonType.Down:
                        if (votable.Voted == VoteState.Down)
                            vote = Dtos.Vote.ClearVote();
                        else
                        {
                            var dlg = new DownvoteReasonWindow();
                            dlg.Topmost = true;
                            dlg.ShowDialog();

                            if (dlg.Reason != null)
                            {
                                vote = Dtos.Vote.DownVote(dlg.Reason.Value);
                            }
                            else
                                return;
                        }
                        break;
                    case ButtonType.Up:
                        if (votable.Voted == VoteState.Up)
                            vote = Dtos.Vote.ClearVote();
                        else
                            vote = Dtos.Vote.UpVote();
                        break;
                }

                FeedItem item = args.SelectedItem as FeedItem;

                switch (args.SelectedItem.Type)
                {
                    case FeedItem.FeedItemType.Post:
                        var rant = item.AsRant();

                        var r1 = await api.User.VoteRant(rant.ID, vote);

                        rant.Update(r1);
                        rant.Read = true;

                        if (db != null)
                            db.MarkRead(rant.ID);

                        break;
                    case FeedItem.FeedItemType.Collab:
                        var collab = item.AsCollab();

                        var r2 = await api.User.VoteCollab(collab.ID, vote);
                        collab.Update(r2);
                        break;
                    case FeedItem.FeedItemType.Comment:
                        var comment = item.AsComment();
                        var r3 = await api.User.VoteComment(comment.ID, vote);
                        comment.Update(r3);
                        break;
                }

                args.InvokeCallback();
            }
        }
    }
}
