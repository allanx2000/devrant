using System;
using Innouvous.Utils.Merged45.MVVM45;
using mvvm = Innouvous.Utils.MVVM;
using System.Text;
using System.Windows.Input;
using System.Windows;

namespace DevRant.WPF
{
    internal class StatusViewerWindowViewModel : ViewModel
    {
        private MessageCollection messageCollection;
        private Window window;

        public string Messages
        {
            get { return Get<string>(); }
            private set
            {
                Set(value);
                RaisePropertyChanged();
            }
        }

        public StatusViewerWindowViewModel(MessageCollection messageCollection, Window window)
        {
            this.messageCollection = messageCollection;
            this.window = window;

            GenerateMessages();
        }

        private void GenerateMessages()
        {
            StringBuilder sb = new StringBuilder();

            foreach (string m in messageCollection.AllMessages)
            {
                sb.AppendLine(m);
            }

            Messages = sb.ToString();
        }

        public ICommand ClearCommand
        {
            get { return new mvvm.CommandHelper(ClearMessages); }
        }

        private void ClearMessages()
        {
            messageCollection.Clear();
            GenerateMessages();
        }

        public ICommand CloseCommand
        {
            get { return new mvvm.CommandHelper(() => window.Close()); }
        }
    }
}