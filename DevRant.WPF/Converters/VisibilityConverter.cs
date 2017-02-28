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
    /// <summary>
    /// This is used to generate the context menu in MainWindow
    /// </summary>
    public class VisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Describes the SelectedItem
        /// </summary>
        public class FeedListState
        {
            private FeedItem SelectedItem;
            
            public bool Following { get { return SelectedItem is Rant ? SelectedItem.AsRant().Followed : false; }}
            public bool IsRant { get { return SelectedItem != null && SelectedItem.Type == FeedItem.FeedItemType.Post; } }
            public bool IsNotification { get { return SelectedItem != null && SelectedItem.Type == FeedItem.FeedItemType.Notification; } }
            

            public bool IsCommentable {
                get {
                    return SelectedItem != null && SelectedItem is Commentable;
                }
            }

            public bool IsDraft { get { return SelectedItem != null && SelectedItem is Draft; } }

            public void SetSelectedItem(FeedItem value)
            {
                SelectedItem = value;
            }
        }

        public object Convert(object parameter)
        {
            return Convert(null, null, parameter, null);  
        }

        public static readonly FeedListState State = new FeedListState();
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                return Visibility.Collapsed;
            else if (parameter is bool)
                return BoolToVisibilility((bool)parameter);
            else if (parameter is string)
            {
                switch ((string)parameter)
                {
                    case "IsNotification":
                        return BoolToVisibilility(State.IsNotification);
                    case "IsDraft":
                        return BoolToVisibilility(State.IsDraft);
                    case "IsCommentable":
                        return BoolToVisibilility(State.IsCommentable);
                    case "Follow":
                        return BoolToVisibilility(State.IsRant && !State.Following);
                    case "Unfollow":
                        return BoolToVisibilility(State.IsRant && State.Following);
                    case "IsRant":
                        return BoolToVisibilility(State.IsRant);
                    default:
                        return Visibility.Visible;
                }
            }
            else
                throw new NotImplementedException();
                
        }

        private Visibility BoolToVisibilility(bool value)
        {
             return value? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
