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
    internal class RantViewerViewModel
    {
        private IDevRantClient api;
        private Window window;

        public RantViewerViewModel(Window window, Rant rant, IDevRantClient api)
        {
            Rant = rant;
            this.api = api;
            this.window = window;
            
            GetComments();
            Comments = new ObservableCollection<Comment>();
        }

        public long Top
        {
            get
            {
                return 10;
            }
        }
        
        public bool LoggedIn
        {
            get { return api.User.LoggedIn; }
        }

        public ICommand CloseCommand
        {
            get
            {
                return new mvvm.CommandHelper(() => window.Close());
            }
        }

        internal async Task ButtonClicked(ButtonClickedEventArgs args)
        {
            try
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
                            editPost = EditPostWindow.CreateForComment(AppManager.Instance.API, args.SelectedItem as Commentable);
                            editPost.Owner = window;
                            editPost.ShowDialog();
                        }
                        break;
                    case ButtonType.Delete:
                        break;
                    case ButtonType.Edit:
                        editPost = EditPostWindow.CreateForEdit(AppManager.Instance.API, args.SelectedItem);
                        editPost.Owner = window;
                        editPost.ShowDialog();
                        break;
                }
            }
            catch (Exception e)
            {
                MessageBoxFactory.ShowError(e);
            }
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
            Utilities.OpenFeedItem(Rant);
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

            if (Comments.Count > 0)
                window.Top = Top;
        }
    }
}