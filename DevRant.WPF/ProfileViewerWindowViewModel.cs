using System;
using Innouvous.Utils.Merged45.MVVM45;
using DevRant.Enums;
using System.Collections.ObjectModel;
using System.Collections;
using DevRant.Dtos;

namespace DevRant.WPF
{
    public class ProfileViewerWindowViewModel : ViewModel
    {
        private IDevRantClient api;
        private ProfileViewerWindow window;
        private string username;
        
        
        private ObservableCollection<ProfileSection> items = new ObservableCollection<ProfileSection>();
        private bool firstLoad = true;

        public ProfileViewerWindowViewModel(string username, ProfileViewerWindow profileViewerWindow, IDevRantClient api)
        {
            this.username = username;
            this.window = profileViewerWindow;
            this.api = api;

            LoadSection(0);
        }

        public string Username { get { return username; } }
        
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

        private void LoadSection(int idx)
        {
            LoadSection((ProfileContentType)idx);
        }

        private async void LoadSection(ProfileContentType type)
        {
            items.Clear();

            Profile profile = await api.GetProfileAsync(username, type);
            
            if (firstLoad)
            {
                Score = profile.Score;
                Rants = profile.RantsCount;
                Upvoted = profile.UpvotedCount;
                Comments = profile.CommentsCount;
                Viewed = profile.ViewedCount;
                Favorites = profile.FavoritesCount;
                           
                firstLoad = false;
            }
            
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
                }
            }
        }

        
    }
}