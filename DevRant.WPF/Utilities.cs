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

namespace DevRant.WPF
{
    public static class Utilities
    {
        public const string BaseURL = "https://www.devrant.io/";

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
        public static void OpenProfile(string username, Window owner, IDevRantClient api)
        {
            ProfileViewerWindow window = new ProfileViewerWindow(username, api);
            window.Owner = owner;
            window.ShowDialog();
        }

        /// <summary>
        /// Open profile in browser
        /// </summary>
        /// <param name="username"></param>
        public static void OpenProfile(string username)
        {
            string url = Utilities.GetProfileUrl(username);
            Process.Start(url);
        }

        internal static bool OpenFeedItem(FeedItem item)
        {
            if (item is Commentable)
            {
                /*
                var dlg = new RantViewerWindow((Rant)SelectedPost, api);
                dlg.Owner = window;
                dlg.ShowDialog();
                */

                string url = GetRantUrl(((Commentable)item).RantId);

                Process.Start(url);


                return true;
            }
            else if (item is ViewModels.Comment)
            {
                string url = GetRantUrl(item.AsComment().RantId);
                Process.Start(url);
                return true;
            }
            else
                return false;
        }

        internal static string ReplaceNewLines(string text)
        {
            if (string.IsNullOrEmpty(text))
                return null;
            else
                return text.Replace(Environment.NewLine, "\n");
        }

        private static string GetRantUrl(long rantId)
        {
            string url = BaseURL + "/rants/" + rantId;
            return url;
        }

        /// <summary>
        /// Casts a vote
        /// </summary>
        /// <param name="args"></param>
        /// <param name="api">API to use to vote</param>
        /// <param name="db">Optional DB to mark as Read</param>
        /// <returns>Throws exception on errors</returns>
        public static async Task Vote(VoteClickedEventArgs args, IDevRantClient api, IPersistentDataStore db = null)
        {
            Vote vote = null;

            Votable votable = args.SelectedItem as Votable;

            if (votable.Voted == VoteState.Disabled)
                return;
                        
            if (votable != null)
            {
                switch (args.Type)
                {
                    case VoteButton.ButtonType.Down:
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
                    case VoteButton.ButtonType.Up:
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
