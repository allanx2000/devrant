using Innouvous.Utils;
using Innouvous.Utils.Merged45.MVVM45;
using System;
using System.Windows.Input;
using mvvm = Innouvous.Utils.MVVM;

namespace DevRant.WPF
{
    public class IDInputWindowViewModel : ViewModel
    {
        private IDInputWindow IDInputWindow;
        private Type type;

        public enum Type
        {
            Rant,
            Profile
        }
        
        public IDInputWindowViewModel(Type type, IDInputWindow rantIDInputWindow)
        {
            this.type = type;
            this.IDInputWindow = rantIDInputWindow;
        }
        
        public string IDLabel
        {
            get
            {
                switch (type)
                {
                    case Type.Profile:
                        return "Username: ";
                    case Type.Rant:
                        return "Rant ID: ";
                    default:
                        throw new NotImplementedException(type.ToString());
                }
            }
        }

        public string ID
        {
            get { return Get<string>(); }
            set
            {
                Set(value);
                RaisePropertyChanged();
            }
        }

        /*
        public long? RantID
        {
            get { return Get<long?>(); }
            set
            {
                Set(value);
                RaisePropertyChanged();
            }
        }
        */

        public ICommand CancelCommand
        {
            get { return new mvvm.CommandHelper(Cancel); }
        }

        private void Cancel()
        {
            ID = null;
            IDInputWindow.Close();
        }

        public ICommand SelectCommand
        {
            get { return new mvvm.CommandHelper(Select); }
        }

        private void Select()
        {
            long tmp;

            if (string.IsNullOrEmpty(ID))
            {
                MessageBoxFactory.ShowError("A ID is required.");
            }
            else if (type == Type.Rant && !long.TryParse(ID, out tmp))
            {
                MessageBoxFactory.ShowError("The ID is invalid.");
            }
            else
            {
                IDInputWindow.Close();
            }
        }
    }
}