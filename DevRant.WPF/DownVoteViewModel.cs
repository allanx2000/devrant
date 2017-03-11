using DevRant.Enums;
using Innouvous.Utils.Merged45.MVVM45;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;
using mvvm = Innouvous.Utils.MVVM;
using System;
using Innouvous.Utils;

namespace DevRant.WPF
{
    internal class DownVoteViewModel : ViewModel
    {
        private DownvoteReasonWindow downvoteReasonWindow;

        public DownVoteViewModel(DownvoteReasonWindow downvoteReasonWindow)
        {
            this.downvoteReasonWindow = downvoteReasonWindow;
        }

        public VoteParam? Reason
        {
            get { return Get<VoteParam?>(); }
            set
            {
                Set(value);
                RaisePropertyChanged();
            }
        }

        public ICommand SetReasonCommand
        {
            get { return new mvvm.CommandHelper(SetReason); }
        }

        private void SetReason(object val)
        {
            try
            {
                Reason = (VoteParam) Convert.ToInt32(val);
            }
            catch (Exception e)
            {

            }
        }

        public ICommand CancelCommand
        {
            get { return new mvvm.CommandHelper(Cancel); }
        }

        private void Cancel()
        {
            Reason = null;
            downvoteReasonWindow.Close();
        }

        public ICommand SelectCommand
        {
            get { return new mvvm.CommandHelper(Select); }
        }

        private void Select()
        {
            if (Reason == null)
            {
                MessageBoxFactory.ShowError("A reason must be selected.");
            }
            else
                downvoteReasonWindow.Close();
        }

    }
}