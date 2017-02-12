using DevRant.Dtos;
using Innouvous.Utils;
using Innouvous.Utils.Merged45.MVVM45;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using mvvm = Innouvous.Utils.MVVM;


namespace DevRant.WPF
{
    internal class OptionsWindowViewModel : ViewModel
    {
        private IDataStore ds;
        private ObservableCollection<string> users;
        private CollectionViewSource usersView;
        private IDevRantClient api;
        private Window window;

        public OptionsWindowViewModel(IDataStore ds, IDevRantClient api, Window window)
        {
            this.ds = ds;
            this.api = api;
            this.window = window;

            Cancelled = true;

            InitializeValues();
        }

        private void InitializeValues()
        {
            UpdateCheckInterval = ds.FollowedUsersUpdateInterval;

            RantSorts = new List<RantSort>();
            foreach (RantSort i in Enum.GetValues(typeof(RantSort)))
            {
                RantSorts.Add(i);
            }
            DefaultFeed = ds.DefaultFeed;

            StoryRanges = new List<StoryRange>();
            foreach (StoryRange i in Enum.GetValues(typeof(StoryRange)))
            {
                StoryRanges.Add(i);
            }

            DefaultStoryRange = ds.DefaultRange;

            users = new ObservableCollection<string>();
            foreach (string user in ds.FollowedUsers)
            {
                users.Add(user);
            }

            usersView = new CollectionViewSource();
            usersView.SortDescriptions.Add(new SortDescription(null, ListSortDirection.Ascending));
            usersView.Source = users;
        }

        public bool Cancelled { get; private set; }

        public int UpdateCheckInterval { get; set; }

        public RantSort DefaultFeed
        {
            get { return Get<RantSort>(); }
            set
            {
                Set(value);
                RaisePropertyChanged();
            }
        }
        public List<RantSort> RantSorts { get; private set; }

        public ICollectionView Users
        {
            get { return usersView.View; }
        }

        public List<StoryRange> StoryRanges { get; private set; }
        public StoryRange DefaultStoryRange
        {
            get { return Get<StoryRange>(); }
            set
            {
                Set(value);
                RaisePropertyChanged();
            }
        }

        public string SelectedUser
        {
            get { return Get<string>(); }
            set
            {
                Set(value);
                RaisePropertyChanged();
                RaisePropertyChanged("UserSelected");
            }
        }

        public bool UserSelected
        {
            get { return SelectedUser != null; }
        }


        public string UserToFollow
        {
            get { return Get<string>(); }
            set
            {
                Set(value);
                RaisePropertyChanged();
            }
        }
        
        #region Commands

        public ICommand AddUserCommand
        {
            get { return new mvvm.CommandHelper(AddUser); }
        }

        private async void AddUser()
        {
            if (string.IsNullOrEmpty(UserToFollow))
                return;
            else if (users.Contains(UserToFollow))
            {
                UserToFollow = null;
                return;
            }

            bool valid = await api.IsValidUser(UserToFollow);
            if (valid)
            {
                users.Add(UserToFollow);
                UserToFollow = null;
            }
            else
                MessageBoxFactory.ShowError(UserToFollow + " does not exist.");
        }

        public ICommand RemoveUserCommand
        {
            get { return new mvvm.CommandHelper(RemoveUser); }
        }

        private void RemoveUser()
        {
            if (SelectedUser == null)
                return;

            users.Remove(SelectedUser);
        }

        public ICommand OpenProfileCommand
        {
            get { return new mvvm.CommandHelper(OpenProfile); }
        }

        private void OpenProfile()
        {
            if (SelectedUser == null)
                return;

            string url = Utilities.BaseURL + "/users/" + SelectedUser;
            Process.Start(url);
        }

        public ICommand CancelCommand
        {
            get { return new mvvm.CommandHelper(() => window.Close()); }
        }

        public ICommand SaveCommand
        {
            get { return new mvvm.CommandHelper(Save); }
        }

        private void Save()
        {

            ds.SetDefaultRange(DefaultStoryRange);
            ds.SetDefaultFeed(DefaultFeed);

            ds.SetFollowing(users);

            Cancelled = false;
            window.Close();
        }
        

        #endregion
    }
}