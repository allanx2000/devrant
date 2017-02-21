using Innouvous.Utils.Merged45.MVVM45;
using System.Windows;
using System.Windows.Input;
using mvvm = Innouvous.Utils.MVVM;
using System;
using Innouvous.Utils;
using DevRant.Dtos;
using System.Linq;
using System.IO;
using DevRant.WPF.DataStore;
using DevRant.WPF.ViewModels;
using System.Text;

namespace DevRant.WPF
{
    internal class EditPostWindowViewModel : ViewModel
    {
        private Window window;
        private EditPostWindow.Type type;

        private IDevRantClient api;
        private IPersistentDataStore db;
        private Draft existing;
        private Commentable parent;

        public EditPostWindowViewModel(Window window, EditPostWindow.Type type, IDevRantClient api, IPersistentDataStore db = null, Draft existing = null, Commentable parent = null)
        {
            this.window = window;
            this.api = api;
            this.type = type;
            this.db = db;
            this.existing = existing;
            this.parent = parent;

            Cancelled = true;
        }

        public bool Cancelled { get; private set; }

        public Visibility TagsVisibility
        {
            get { return type == EditPostWindow.Type.Rant ? Visibility.Visible : Visibility.Collapsed; }
        }

        public Visibility SaveDraftVisibility
        {
            get { return type == EditPostWindow.Type.Rant && db != null ? Visibility.Visible : Visibility.Collapsed; }
        }

        public string TextType
        {
            get { return type.ToString(); }
        }

        public int Remaining
        {
            get { return MaxCharacters - (Text == null ? 0 : Text.Length); }
        }

        public int MaxCharacters
        {
            get
            {
                switch (type)
                {
                    case EditPostWindow.Type.Comment:
                        return 1000;
                    case EditPostWindow.Type.Rant:
                        return 5000;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public string WindowTitle
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                if (type == EditPostWindow.Type.Rant)
                {
                    if (existing != null)
                        sb.Append("Edit ");
                    else 
                        sb.Append("Create New");

                    sb.Append(" Rant");
                }
                else if (type == EditPostWindow.Type.Comment)
                {
                    sb.AppendLine("Add a Comment");
                }

                string str = sb.ToString();
                return str;
            }
        }

        public string Text
        {
            get { return Get<string>(); }
            set
            {
                Set(value);
                RaisePropertyChanged();
                RaisePropertyChanged("Remaining");
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