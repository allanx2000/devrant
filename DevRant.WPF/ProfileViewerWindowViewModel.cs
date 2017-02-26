using System;
using Innouvous.Utils.Merged45.MVVM45;
using DevRant.Enums;
using System.Collections.ObjectModel;
using System.Collections;
using DevRant.Dtos;
using System.Windows.Input;
using mvvm = Innouvous.Utils.MVVM;
using DevRant.WPF.Controls;
using Innouvous.Utils;
using System.Threading.Tasks;
using DevRant.WPF.ViewModels;
using System.Windows.Media;

namespace DevRant.WPF
{
    public class ProfileViewerWindowViewModel : ViewModel
    {
        private IDevRantClient api;
        private ProfileViewerWindow window;
        private string username;
        
        private ObservableCollection<ProfileSection> items = new ObservableCollection<ProfileSection>();
        private bool firstLoad = true;

        public ImageSource Avatar
        {
            get
            {
                return Get<ImageSource>();
            }
            set
            {
                Set(value);
                RaisePropertyChanged();
            }
        }

        public ProfileViewerWindowViewModel(string username, ProfileViewerWindow profileViewerWindow, IDevRantClient api)
        {
            this.username = username;
            this.window = profileViewerWindow;
            this.api = api;

            LoadSection(0);
        }


        public async Task Vote(VoteClickedEventArgs args)
        {
            try
            {
                await Utilities.Vote(args, api);
            }
            catch (Exception e)
            {
                MessageBoxFactory.ShowError(e);
            }
        }
        

        #region Properties

        public bool LoggedIn { get { return api.User.LoggedIn; } }
        
        public string Username { get { return username; } }
        
        public FeedItem Selected
        {
            get { return Get<FeedItem>(); }
            set
            {
                Set(value);
                RaisePropertyChanged();
            }
        }

        #region Counts
        public int Favorites
        {
            get { return Get<int>(); }
            set
            {
                Set(value);
                RaisePropertyChanged();
            }
        }

        public int Viewed
        {
            get { return Get<int>(); }
            set
            {
                Set(value);
                RaisePropertyChanged();
            }
        }

        public int Comments
        {
            get { return Get<int>(); }
            set
            {
                Set(value);
                RaisePropertyChanged();
            }
        }

        public int Upvoted
        {
            get { return Get<int>(); }
            set
            {
                Set(value);
                RaisePropertyChanged();
            }
        }

        public int Rants
        {
            get { return Get<int>(); }
            set
            {
                Set(value);
                RaisePropertyChanged();
            }
        }
        #endregion

        public ICollection Items
        {
            get
            {
                return items;
            }
        }

        public int SelectedSection
        {
            get { return Get<int>(); }
            set
            {
                Set(value);
                RaisePropertyChanged();

                LoadSection(value);
            }
        }

        public int Score {
            get { return Get<int>(); }
            set
            {
                Set(value);
                RaisePropertyChanged();
            }
        }

        #endregion

        private void LoadSection(int idx)
        {
            LoadSection((ProfileContentType)idx);
        }

        int page;

        private async void LoadSection(ProfileContentType type)
        {
            items.Clear();
            page = 0;

            Profile profile = await api.GetProfileAsync(username, type);

            if (firstLoad)
            {
                Score = profile.Score;
                Rants = profile.RantsCount;
                Upvoted = profile.UpvotedCount;
                Comments = profile.CommentsCount;
                Viewed = profile.ViewedCount;
                Favorites = profile.FavoritesCount;

                if (profile.AvatarImage != null)
                    Avatar = api.GetAvatar(profile.AvatarImage);

                firstLoad = false;
            }

            AddItems(profile);

            if (items.Count > 0)
                window.ItemsListBox.ScrollIntoView(items[0]);
        }

        private void AddItems(Profile profile)
        {
            if (profile.Rants != null && profile.Rants.Count > 0)
            {
                //Check already loaded dupes
                foreach (var i in profile.Rants)
                {
                    items.Add(new ViewModels.Rant(i));
                }
            }
            else if (profile.Comments != null)
            {
                foreach (var i in profile.Comments)
                {
                    items.Add(new ViewModels.Comment(i));
                }
            }
        }


        #region Commands

        public ICommand ViewRantCommand
        {
            get
            {
                return new mvvm.CommandHelper(ViewRant);
            }
        }


        public void ViewRant()
        {
            Utilities.OpenFeedItem(Selected);
        }

        public ICommand CloseCommand
        {
            get { return new mvvm.CommandHelper(() => window.Close()); }
        }

        public ICommand OpenInBrowserCommand
        {
            get
            {
                return new mvvm.CommandHelper(OpenInBrowser);
            }
        }

        private void OpenInBrowser()
        {
            Utilities.OpenProfile(Username);
        }

        public ICommand LoadMoreCommand
        {
            get
            {
                return new mvvm.CommandHelper(LoadMore);
            }
        }

        private async void LoadMore()
        {
            page++;

            int skip = page * V1.Constants.ProfileDataSkip;

            ProfileContentType type = (ProfileContentType)SelectedSection;

            int total = GetTotal(type);

            if (total < skip)
                return;

            Profile profile = await api.GetProfileAsync(username, (ProfileContentType)SelectedSection, skip);
            AddItems(profile);
        }

        private int GetTotal(ProfileContentType type)
        {
            switch (type)
            {
                case ProfileContentType.Comments:
                    return Comments;
                case ProfileContentType.Favorites:
                    return Favorites;
                case ProfileContentType.Rants:
                    return Rants;
                case ProfileContentType.Upvoted:
                    return Upvoted;
                case ProfileContentType.Viewed:
                    return Viewed;
                default:
                    throw new NotImplementedException();
            }
        }

        #endregion
    }
}