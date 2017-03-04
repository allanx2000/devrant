using DevRant.Dtos;
using DevRant.Enums;
using DevRant.WPF.DataStore;
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

            LoginInfo login = ds.GetLoginInfo();
            if (login != null)
            {
                Password = login.Password;
                Username = login.Username;
            }

            ShowUsername = !ds.HideUsername;
            ShowCreateTime = ds.ShowCreateTime;
            FilterOutRead = ds.FilterOutRead;
            DBFolder = ds.DBFolder;

            ResultsLimit = ds.ResultsLimit;
            MinScore = ds.MinScore;
            MaxPages = ds.MaxPages;
        }

        public int ResultsLimit
        {
            get { return Get<int>(); }
            set
            {
                Set(value);
                RaisePropertyChanged();
            }
        }
        public int MaxPages
        {
            get { return Get<int>(); }
            set
            {
                Set(value);
                RaisePropertyChanged();
            }
        }
        public int MinScore
        {
            get { return Get<int>(); }
            set
            {
                Set(value);
                RaisePropertyChanged();
            }
        }
        
        public string Password { get; set; }
        public string Username { get; set; }

        public string DBFolder
        {
            get { return Get<string>(); }
            set
            {
                Set(value);
                RaisePropertyChanged();
            }
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

        public ICommand SelectDBFolderCommand
        {
            get { return new mvvm.CommandHelper(SelectDBFolder); }
        }

        private void SelectDBFolder()
        {
            var dlg = DialogsUtility.CreateFolderBrowser();
            dlg.ShowDialog();

            if (dlg.SelectedPath != null)
                DBFolder = dlg.SelectedPath;
        }

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
            
            Utilities.OpenProfile(SelectedUser, window, api);

            /*
            string url = Utilities.BaseURL + "/users/" + SelectedUser;
            Process.Start(url);
            */
        }

        public ICommand CancelCommand
        {
            get { return new mvvm.CommandHelper(() => window.Close()); }
        }

        public ICommand SaveCommand
        {
            get { return new mvvm.CommandHelper(Save); }
        }

        public List<string> AddedUsers { get; private set; }
        
        public bool FilterOutRead { get; set; }

        public bool ShowUsername {get; set; }
        public bool ShowCreateTime { get; set; }
        public bool LoginChanged { get; private set; }
        public bool DatabaseChanged { get; private set; }

        private void Save()
        {
            try
            {
                ds.SetDefaultRange(DefaultStoryRange);
                ds.SetDefaultFeed(DefaultFeed);

                AddedUsers = ds.SetFollowing(users);
                ds.SetUpdatesInterval(UpdateCheckInterval);

                if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
                    api.User.Login(Username, Password);

                ds.SetHideUsername(!ShowUsername);
                ds.SetShowCreateTime(ShowCreateTime);
                ds.SetFilterOutRead(FilterOutRead);

                ds.SetLimits(ResultsLimit, MinScore, MaxPages);

                //Check DBPath
                if (ds.DBFolder != DBFolder)
                {
                    ds.SetDBFolder(DBFolder);
                    DatabaseChanged = true;
                }

                //Check/Save Login
                if (Username != null && Password != null)
                {
                    var info = ds.GetLoginInfo();
                    if (info != null)
                    {
                        if (Username != info.Username || Password != info.Password)
                        {
                            ds.SetLogin(new LoginInfo(Username, Password));
                            LoginChanged = true;
                        }
                    }
                    else
                    {
                        ds.SetLogin(new LoginInfo(Username, Password));
                        LoginChanged = true;
                    }
                }

                Cancelled = false;
                window.Close();
            }
            catch (Exception e)
            {
                MessageBoxFactory.ShowError(e);
            }
        }
        

        #endregion
    }
}