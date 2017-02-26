using DevRant.WPF.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using mvvm = Innouvous.Utils.MVVM;

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
        
        public System.Windows.Input.ICommand OpenInBrowserCommand
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

            window.Top = Top;
        }
    }
}