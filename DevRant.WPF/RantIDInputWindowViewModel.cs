using Innouvous.Utils;
using Innouvous.Utils.Merged45.MVVM45;
using System.Windows.Input;
using mvvm = Innouvous.Utils.MVVM;

namespace DevRant.WPF
{
    internal class RantIDInputWindowViewModel : ViewModel
    {
        private RantIDInputWindow rantIDInputWindow;

        public RantIDInputWindowViewModel(RantIDInputWindow rantIDInputWindow)
        {
            this.rantIDInputWindow = rantIDInputWindow;
        }
        
        public long? RantID
        {
            get { return Get<long?>(); }
            set
            {
                Set(value);
                RaisePropertyChanged();
            }
        }

        public ICommand CancelCommand
        {
            get { return new mvvm.CommandHelper(Cancel); }
        }

        private void Cancel()
        {
            RantID = null;
            rantIDInputWindow.Close();
        }

        public ICommand SelectCommand
        {
            get { return new mvvm.CommandHelper(Select); }
        }

        private void Select()
        {
            if (RantID == null)
            {
                MessageBoxFactory.ShowError("A RantId is required.");
            }
            else
            {
                rantIDInputWindow.Close();
            }
        }
    }
}