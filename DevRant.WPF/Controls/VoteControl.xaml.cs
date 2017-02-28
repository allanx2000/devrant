using DevRant.Enums;
using DevRant.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DevRant.WPF.Controls
{
    /// <summary>
    /// Interaction logic for VoteControl.xaml
    /// </summary>
    public partial class VoteControl : MyUserControl
    {
        public static DependencyProperty IsLoggedInProperty = DependencyProperty.Register("IsLoggedIn", typeof(bool), typeof(VoteControl));
        public static DependencyProperty VotesStringProperty = DependencyProperty.Register("VotesString", typeof(string), typeof(VoteControl));
        public static DependencyProperty VotedProperty = DependencyProperty.Register("Voted", typeof(VoteState), typeof(VoteControl));

        public event VoteButton.OnClick UpClicked;
        public event VoteButton.OnClick DownClicked;

        public VoteControl()
        {
            InitializeComponent();

            //DataContextChanged += ContextChanged;
        }
        /*
        private void ContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            INotifyPropertyChanged dc = DataContext as INotifyPropertyChanged;
            if (dc != null)
            {
                dc.PropertyChanged += DataContext_PropertyChanged;
            }
        }

        private void DataContext_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Voted":
                    RaisePropertyChange("DownSelected");
                    RaisePropertyChange("UpSelected");
                    break;
                default:
                    break;
            }
        }
        */

        
        public Visibility VotingVisibility
        {
            get
            {
                return IsLoggedIn ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public bool CanVote
        {
            get
            {
                return Voted != VoteState.Disabled;
            }
        }

        public string VotesString
        {
            get
            {
                return (string)GetValue(VotesStringProperty);
            }
            set
            {
                SetValue(VotesStringProperty, value);
                RaisePropertyChange();
            }
        }

        public VoteState Voted
        {
            get
            {
                return (VoteState)GetValue(VotedProperty);
            }
            set
            {
                SetValue(VotedProperty, value);
                RaisePropertyChange();
                UpdateSelected();
            }
        }

        public bool DownSelected
        {
            get { return Voted == VoteState.Down; }
        }

        public bool UpSelected
        {
            get { return Voted == VoteState.Up; }
        }

        public bool IsLoggedIn
        {
            get
            {
                return (bool) GetValue(IsLoggedInProperty);
            }
            set
            {
                SetValue(IsLoggedInProperty, value);
                RaisePropertyChange();
                RaisePropertyChange("VotingVisibility");
            }
        }

        private void Button_Clicked(object sender, ButtonClickedEventArgs args)
        {
            args.Callback += UpdateSelected;
            args.SelectedItem = DataContext as FeedItem;

            switch (args.Type)
            {
                case ButtonType.Down:
                    if (DownClicked != null)
                        DownClicked.Invoke(sender, args);
                    break;
                case ButtonType.Up:
                    if (UpClicked != null)
                        UpClicked.Invoke(sender, args);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void UpdateSelected()
        {
            RaisePropertyChange("DownSelected");
            RaisePropertyChange("UpSelected");
            RaisePropertyChange("CanVote");
        }
        
    }
}
