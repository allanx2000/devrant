using Innouvous.Utils.Merged45.MVVM45;
using mvvm = Innouvous.Utils.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Innouvous.Utils;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.ComponentModel;
using DevRant.WPF.ViewModels;
using System.Diagnostics;

namespace DevRant.WPF
{
    class MainWindowViewModel : ViewModel
    {
        private Window window;
        private IDataStore ds;
        private DevRantClient api;

        public bool IsLoading
        {
            get { return Get<bool>(); }
            private set
            {
                Set(value);
                RaisePropertyChanged();
            }
        }

        public MainWindowViewModel(Window window)
        {
            this.window = window;
            ds = new AppSettingsDataStore();
            api = new DevRantClient();

            feedView = new CollectionViewSource();
            feedView.Source = feeds;
        }

        public FeedItem SelectedPost { get; set; }

        #region Sections

        public const string SectionGeneral = "GeneralFeed";
        public const string SectionNotifications = "MyNotifications";
        
        public const string SectionFollowed = "FollowedUsers";
        public async Task LoadSection(string section)
        {
            IsLoading = true;

            switch (section)
            {
                case SectionGeneral:
                    await LoadFeed();
                    ListType = FeedItem.FeedItemType.Post;
                    break;
                case SectionNotifications:
                    await LoadNotifications();
                    ListType = FeedItem.FeedItemType.Notification;
                    break;
            }

            IsLoading = false;
        }

        private async Task LoadNotifications()
        {
            //TODO: Add get notifications

            feeds.Clear();

            feeds.Add(new Notification());
            feeds.Add(new Notification());
            feeds.Add(new Notification());
        }

        private async Task LoadFeed()
        {
            var rants = await api.GetRantsAsync();

            feeds.Clear();

            foreach (var rant in rants)
            {
                Rant r = new Rant(rant);
                feeds.Add(r);
            }

            UpdateFollow();
        }

        public Visibility PostVisibility
        {
            get { return ListType == FeedItem.FeedItemType.Post ? Visibility.Visible : Visibility.Collapsed; }

        }

        public ICommand OpenPostCommand
        {
            get { return new mvvm.CommandHelper(OpenPost); }
        }

        public ICommand FollowUserCommand
        {
            get { return new mvvm.CommandHelper(FollowUser); }
        }

        private void FollowUser()
        {
            if (SelectedPost == null)
                return;

            ds.Follow(SelectedPost.AsRant().Username);

            UpdateFollow();
        }

        private void UpdateFollow()
        {
            var followed = ds.FollowedUsers;

            foreach (Rant rant in feeds)
            {
                if (followed.Contains(rant.Username))
                    rant.Followed = true;
            }
        }

        public ICommand ViewProfileCommand
        {
            get { return new mvvm.CommandHelper(ViewProfile); }
        }

        private void ViewProfile()
        {
            if (SelectedPost == null)
                return;

            Process.Start(((Rant)SelectedPost).ProfileURL);
        }

        public void OpenPost()
        {
            if (SelectedPost == null)
                return;
            else if (SelectedPost is Rant)
                Process.Start(((Rant)SelectedPost).PostURL);
            //TODO: Add Notification
        }

        private ObservableCollection<FeedItem> feeds = new ObservableCollection<FeedItem>();
        private CollectionViewSource feedView;

        //TODO: Add listbox and feed, 2 types, notification and post
        //Convert DevRantFeed to actual rant post with contents and pictures?
        public ICollectionView FeedView
        {
            get
            {
                return feedView.View;
            }
        }
        #endregion

        public ICommand TestCommand
        {
            get { return new mvvm.CommandHelper(Test); }
        }

        public FeedItem.FeedItemType ListType
        {
            get { return Get<FeedItem.FeedItemType>(); }
            private set
            {
                Set(value);
                RaisePropertyChanged("ListType");
                RaisePropertyChanged("PostVisibility");
            }
        }

        private async void Test()
        {
            var profile = await GetProfile();
            MessageBoxFactory.ShowInfo(profile.Skills, "Test");
        }

        public async Task<Dtos.Profile> GetProfile()
        {
            var profile = await api.GetProfileAsync("allanx2000");
            return profile;
        }
    }
}
