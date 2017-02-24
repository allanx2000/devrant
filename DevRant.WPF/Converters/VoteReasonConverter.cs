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
    public class VoteReasonConverter : IValueConverter
    {
        public object Convert(object parameter)
        {
            return Convert(null, null, parameter, null);  
        }
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;
            else
                return (parameter.ToString() == value.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;
            else
                return (bool)value ? parameter : null;
        }
    }
}
