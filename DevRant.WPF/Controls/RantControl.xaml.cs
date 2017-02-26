using DevRant.WPF.ViewModels;
using Innouvous.Utils.DataBucket;
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
    public partial class RantControl : MyUserControl
    {
        public static DependencyProperty LoggedInProperty = DependencyProperty.Register("LoggedIn", typeof(bool), typeof(RantControl));
        public static DependencyProperty DateVisibilityProperty = DependencyProperty.Register("DateVisibility", typeof(Visibility), typeof(RantControl));
        public static DependencyProperty UsernameVisibilityProperty = DependencyProperty.Register("UsernameVisibility", typeof(Visibility), typeof(RantControl));

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

        public Visibility UsernameVisibility
        {
            get
            {
                return (Visibility)GetValue(UsernameVisibilityProperty);
            }
            set
            {
                SetValue(UsernameVisibilityProperty, value);
                RaisePropertyChange();
            }
        }

        private DataBucket bucket = new DataBucket();

        public Visibility TagsVisibility
        {
            get
            {
                return bucket.Get<Visibility>("TagsVisibility");
            }
            private set
            {
                bucket.Set("TagsVisibility", value);
                RaisePropertyChange();
            }
        }

        public ImageSource Avatar
        {
            get
            {
                return  bucket.Get<ImageSource>("Avatar");
            }
            set
            {
                bucket.Set("Avatar", value);
                RaisePropertyChange();
            }
        }

        public Visibility CommentsVisibility
        {
            get
            {
                return bucket.Get<Visibility>("CommentsVisibility");
            }
            private set
            {
                bucket.Set("CommentsVisibility", value);
                RaisePropertyChange();
            }
        }

        public RantControl()
        {
            InitializeComponent();

            TagsVisibility = Visibility.Visible;
            CommentsVisibility = Visibility.Visible;

            DataContextChanged += RantControl_DataContextChanged;

        }

        private void RantControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                FeedItem item = e.NewValue as FeedItem;
                TagsVisibility = item is Rant? Visibility.Visible : Visibility.Collapsed; //&& ((Rant)e.NewValue).tag
                CommentsVisibility = item is Rant ? Visibility.Visible : Visibility.Collapsed;

                var hasAvatar = item as Dtos.HasAvatar;
                if (API != null && UsernameVisibility == Visibility.Visible && hasAvatar != null)
                {
                    Avatar = API.GetAvatar(hasAvatar.AvatarImage);
                }       
            }
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
