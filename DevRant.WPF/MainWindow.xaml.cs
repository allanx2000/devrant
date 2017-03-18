using DevRant.WPF.Controls;
using Innouvous.Utils;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DevRant.WPF
{

    //TODO: Move libs into project/GitHub

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static MainWindowViewModel vm;
        
        public MainWindow()
        {
            vm = new MainWindowViewModel(this);
            this.DataContext = vm;

            InitializeComponent();

            //SectionsListBox.se
        }

        private async void RefreshFeed(object sender, RoutedEventArgs e)
        {
            var item = (ListBoxItem)SectionsListBox.SelectedItem;
            if (item != null)
            {
                await LoadFeed((SectionType) item.Tag);
            }
        }

        private async void SectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var listBox = (ListBox)sender;
                var item = (ListBoxItem)listBox.SelectedItem;

                if (item == null || item.Tag == null)
                    return;
                
                await LoadFeed((SectionType) item.Tag, true);                
            }
            catch (Exception ex)
            {
                MessageBoxFactory.ShowError(ex.Message);
            }
        }

        int windowEnabled = 0;

        public enum State
        {
            Enable,
            Disable
        }
        public void SetIsEnabled(bool enabled)
        {
            if (enabled)
                windowEnabled++;
            else
                windowEnabled--;

            if (windowEnabled >= 0)
                IsEnabled = true;
            else
                IsEnabled = false;
        }

        private async Task LoadFeed(SectionType section, bool resetOffset = false)
        {
            SetIsEnabled(false);

            try
            {
                await vm.LoadSection(section, resetOffset);
                
                if (FeedListBox.Items.Count > 0)
                    FeedListBox.ScrollIntoView(FeedListBox.Items[0]);
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                SetIsEnabled(true);
            }
        }
        
        private void FeedListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                vm.OpenPost();
            }
            catch (Exception ex)
            {
                MessageBoxFactory.ShowError(ex, owner: this);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            App.Current.Shutdown();
        }

        private void StatusBar_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            vm.ShowStatusHistory();
        }

        private void Button_Clicked(object sender, ButtonClickedEventArgs args)
        {
            vm.HandleButton(args);
        }
        
    }
}
