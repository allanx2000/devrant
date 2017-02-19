using DevRant.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DevRant.WPF
{
    class FeedListItemTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement elemnt = container as FrameworkElement;
            var dc = elemnt.DataContext;

            /*
             * RantTemplate
             * NotificationTemplate
             * CollabTemplate
             */

            DataTemplate dt = null;

            if (item is Rant)
                dt = elemnt.FindResource("RantTemplate") as DataTemplate;
            else if (item is Notification)
                dt = elemnt.FindResource("NotificationTemplate") as DataTemplate;
            else if (item is Collab)
                dt = elemnt.FindResource("CollabTemplate") as DataTemplate;

            return dt;
        }
    }
}
