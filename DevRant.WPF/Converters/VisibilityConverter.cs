using DevRant.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace DevRant.WPF.Converters
{
    public class VisibilityConverter : IValueConverter
    {
        public class AppState
        {
            //public FeedItem SelectedItem { get; private set; }

            private FeedItem SelectedItem;
            
            public bool Following { get { return SelectedItem is Rant ? SelectedItem.AsRant().Followed : false; }}
            public bool IsRant { get { return SelectedItem != null && SelectedItem.Type == FeedItem.FeedItemType.Post; } }
            public bool IsNotification { get { return SelectedItem != null && SelectedItem.Type == FeedItem.FeedItemType.Notification; } }
            
            public void SetSelectedItem(FeedItem value)
            {
                SelectedItem = value;
            }
        }

        public static readonly AppState State = new AppState();
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                return Visibility.Collapsed;
            else if (parameter is string)
            {
                switch ((string) parameter)
                {
                    case "Follow":
                        return State.IsRant && !State.Following? Visibility.Visible : Visibility.Collapsed;
                    case "Unfollow":
                        return State.IsRant && State.Following ? Visibility.Visible : Visibility.Collapsed;
                    case "IsRant":
                        return State.IsRant ? Visibility.Visible : Visibility.Collapsed;
                    default:
                        return Visibility.Visible;
                }
            }
            else
                throw new NotImplementedException();
                
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
