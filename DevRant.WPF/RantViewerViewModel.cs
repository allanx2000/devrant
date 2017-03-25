using DevRant.WPF.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using mvvm = Innouvous.Utils.MVVM;
using DevRant.WPF.Controls;
using DevRant.Exceptions;
using System.Threading.Tasks;
using Innouvous.Utils;

namespace DevRant.WPF
{
    internal class RantViewerViewModel : Innouvous.Utils.Merged45.MVVM45.ViewModel
    {
        private IDevRantClient api;
        private Window window;
        private Action<string> onScroll;
        private const string Favorite = "_Favorite";
        private const string Unfavorite = "Un_favorite";

        private const string UnfollowUser = "Unfollow _User";
        private const string FollowUser = "Follow _User";

        public RantViewerViewModel(Window window, Rant rant, IDevRantClient api, Action<string> onScroll)
        {
            Rant = rant;
            this.api = api;
            this.window = window;
            this.onScroll = onScroll;
            
            Comments = new ObservableCollection<Comment>();
            GetComments();
        }
        
        public bool LoggedIn
        {
            get { return api.User.LoggedIn; }
        }


        public ICommand ScrollCommand
        {
            get
            {
                return new mvvm.CommandHelper(Scroll);
            }
        }

        private void Scroll(object arg)
        {
            try
            {
                if (Comments.Count > 0 && onScroll != null)
                {

                    string param = arg as string;

                    onScroll.Invoke(param);
                }
            }
            catch (Exception e)
            {
                MessageBoxFactory.ShowError(e);
            }
        }
        
        /// <summary>
        /// Handles the link buttons in the screen
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        internal async Task ButtonClicked(ButtonClickedEventArgs args)
        {
            try
            {
                bool handled = await Utilities.HandleButtons(window, args);

                if (handled)
                    GetComments();
            }
            catch (Exception e)
            {
                MessageBoxFactory.ShowError(e);
            }
        }

        #region Command

        public ICommand OpenInBrowserCommand
        {
            get
            {
                return new mvvm.CommandHelper(OpenInBrowser);
            }
        }

        private void OpenInBrowser()
        {
            Utilities.OpenFeedItem(Rant);
        }

        
        public ICommand FollowUserCommand
        {
            get
            {
                return new mvvm.CommandHelper(ToggleFollowUser);
            }
        }

        private void ToggleFollowUser()
        {
            try
            {
                var settings = AppManager.Instance.Settings;

                if (settings.IsFollowing(Rant.Username))
                {
                    settings.Unfollow(Rant.Username);
                }
                else
                {
                    settings.Follow(Rant.Username);
                }

                RaisePropertyChanged("FollowUserString");
                Rant.Followed = settings.IsFollowing(Rant.Username);
            }
            catch (Exception e)
            {
                MessageBoxFactory.ShowError(e);
            }
        }

        public ICommand ToggleFavoriteCommand
        {
            get
            {
                return new mvvm.CommandHelper(ToggleFavorite);
            }
        }

        private async void ToggleFavorite()
        {
            try
            {
                await api.User.ToggleFavorite(Rant.ID);
                Rant.IsFavorite = !Rant.IsFavorite;
                RaisePropertyChanged("FavoriteString");

            }
            catch (Exception e)
            {
                MessageBoxFactory.ShowError(e);
            }
        }

        public ICommand CloseCommand
        {
            get
            {
                return new mvvm.CommandHelper(() => window.Close());
            }
        }

        #endregion

        #region Properties
        
        public string FavoriteString
        {
            get { return Rant.IsFavorite ? Unfavorite : Favorite; }
        }

        public string FollowUserString
        {
            get { return AppManager.Instance.Settings.IsFollowing(Rant.Username)? UnfollowUser : FollowUser; }
        }

        public Rant Rant { get; set; } //TODO: Make private, check DP

        public ObservableCollection<Comment> Comments { get; set; }

        private async void GetComments()
        {
            Dtos.Rant rant = await api.GetRant(this.Rant.ID);

            if (rant.Comments != null)
            {
                Comments.Clear();

                foreach (var c in rant.Comments)
                {
                    Comments.Add(new Comment(c));
                }
            }
        }

        #endregion
    }
}