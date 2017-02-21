using Innouvous.Utils.Merged45.MVVM45;
using System.Windows;
using System.Windows.Input;
using mvvm = Innouvous.Utils.MVVM;
using System;
using Innouvous.Utils;
using DevRant.Dtos;
using System.Linq;
using System.IO;

namespace DevRant.WPF
{
    internal class CreateRantWindowViewModel : ViewModel
    {
        private IDevRantClient api;
        private Window window;
        
        public CreateRantWindowViewModel(Window window, IDevRantClient api)
        {
            this.window = window;
            this.api = api;

            Cancelled = true;
        }

        public bool Cancelled { get; private set; }

        public string Text
        {
            get { return Get<string>(); }
            set
            {
                Set(value);
                RaisePropertyChanged();
            }
        }

        public string TagsString
        {
            get { return Get<string>(); }
            set
            {
                Set(value);
                RaisePropertyChanged();
            }
        }

        public string ImagePath
        {
            get { return Get<string>(); }
            private set
            {
                Set(value);
                RaisePropertyChanged();
            }
        }

        public ICommand BrowseCommand
        {
            get { return new mvvm.CommandHelper(Browse); }
        }

        private void Browse()
        {
            var dlg = DialogsUtility.CreateOpenFileDialog("Select an image");
            
            var types = from i in PostContent.GetSupportedTypes() select "*." + i;
            string supported = string.Join(";", types);
            DialogsUtility.AddExtension(dlg, "Images", supported);

            dlg.ShowDialog();

            if (!string.IsNullOrEmpty(dlg.FileName))
                ImagePath = dlg.FileName;
        }

        public ICommand ClearCommand
        {
            get { return new mvvm.CommandHelper(() => ImagePath = null); }
        }
        
        public ICommand PostCommand
        {
            get { return new mvvm.CommandHelper(Post); }
        }
        private async void Post()
        {
            try
            {
                if (string.IsNullOrEmpty(Text) || Text.Length < 5)
                    throw new Exception("Rant must be more than 5 characters long.");
                
                PostContent data = new PostContent(Text);

                if (!string.IsNullOrEmpty(TagsString))
                    data.SetTag(TagsString);

                if (!string.IsNullOrEmpty(ImagePath))
                {
                    byte[] bytes = System.IO.File.ReadAllBytes(ImagePath);
                    data.AddImage(bytes, ImagePath);
                }

                await api.User.UploadRant(data);

                Cancelled = false;
                window.Close();
            }
            catch (Exception e)
            {
                MessageBoxFactory.ShowError(e, owner: window);
            }
        }

        public ICommand CancelCommand
        {
            get { return new mvvm.CommandHelper(() => window.Close()); }
        }
    }
}