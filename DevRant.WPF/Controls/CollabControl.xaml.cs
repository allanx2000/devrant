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
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class CollabControl : MyUserControl
    {
        public static DependencyProperty LoggedInProperty = DependencyProperty.Register("LoggedIn", typeof(bool), typeof(CollabControl));
        public static DependencyProperty DateVisibilityProperty = DependencyProperty.Register("DateVisibility", typeof(Visibility), typeof(CollabControl));

        public event VoteButton.OnClick VoteClicked;

        public bool LoggedIn
        {
            get
            {
                return (bool)GetValue(LoggedInProperty);
            }
            set
            {
                SetValue(LoggedInProperty, value);
                RaisePropertyChange();
            }
        }
        
        public Visibility DateVisibility
        {
            get
            {
                return (Visibility) GetValue(DateVisibilityProperty);
            }
            set
            {
                SetValue(DateVisibilityProperty, value);
                RaisePropertyChange();
            }
        }

        public CollabControl()
        {
            InitializeComponent();
        }


        private void VoteControl_DownClicked(object sender, VoteClickedEventArgs args)
        {
            VoteControl_Clicked(sender, args);
        }

        private void VoteControl_Clicked(object sender, VoteClickedEventArgs args)
        {

            if (VoteClicked != null)
                VoteClicked.Invoke(sender, args);
        }

        private void VoteControl_UpClicked(object sender, VoteClickedEventArgs args)
        {

            VoteControl_Clicked(sender, args);
        }
    }
}
