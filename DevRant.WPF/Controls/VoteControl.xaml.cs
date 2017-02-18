using DevRant.Enums;
using System;
using System.Collections.Generic;
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

        public VoteControl()
        {
            InitializeComponent();
        }

        public event VoteButton.OnClick UpClicked;
        public event VoteButton.OnClick DownClicked;

        public Visibility VotingVisibility
        {
            get
            {
                return IsLoggedIn ? Visibility.Visible : Visibility.Collapsed;
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
                RaisePropertyChange("DownSelected");
                RaisePropertyChange("UpSelected");
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

        private void VoteButton_Clicked(object sender, VoteButton.ButtonType type)
        {
            switch (type)
            {
                case VoteButton.ButtonType.Down:
                    if (DownClicked != null)
                        DownClicked.Invoke(sender, type);
                    break;
                case VoteButton.ButtonType.Up:
                    if (UpClicked != null)
                        UpClicked.Invoke(sender, type);
                    break;
            }
        }
    }
}
