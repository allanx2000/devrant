using DevRant.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace DevRant.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel vm;

        public MainWindow()
        {
            vm = new MainWindowViewModel(this);
            this.DataContext = vm;

            InitializeComponent();
        }


        private async void SectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = (ListBox)sender;
            var item = (ListBoxItem)listBox.SelectedItem;

            if (item == null)
                return;

            IsEnabled = false;
            await vm.LoadSection(item.Name);
            IsEnabled = true;
        }

        private void FeedListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Rant rant = (Rant) FeedListBox.SelectedItem;

            if (rant != null)
            {
                Process.Start(rant.PostURL);
            }
        }
    }
}
