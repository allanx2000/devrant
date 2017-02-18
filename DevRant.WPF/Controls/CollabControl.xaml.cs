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
        public static DependencyProperty LoggedInProperty = DependencyProperty.Register("LoggedIn", typeof(bool), typeof(RantControl));
        public static DependencyProperty DateVisibilityProperty = DependencyProperty.Register("DateVisibility", typeof(Visibility), typeof(RantControl));
        public static DependencyProperty UsernameVisibilityProperty = DependencyProperty.Register("UsernameVisibility", typeof(Visibility), typeof(RantControl));

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


        public CollabControl()
        {
            InitializeComponent();


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
                default:
                    RaisePropertyChange(e.PropertyName);
                    break;
            }
        }
    }
}
