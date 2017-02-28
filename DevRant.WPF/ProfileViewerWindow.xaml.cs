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
using System.Windows.Shapes;

namespace DevRant.WPF
{
    /// <summary>
    /// Interaction logic for ProfileViewerWindow.xaml
    /// </summary>
    public partial class ProfileViewerWindow : Window
    {
        private readonly ProfileViewerWindowViewModel vm;
        public ProfileViewerWindow(string username, IDevRantClient api)
        {
            InitializeComponent();

            vm = new ProfileViewerWindowViewModel(username, this, api);
            DataContext = vm;
        }

        private async void RantControl_VoteClicked(object sender, Controls.ButtonClickedEventArgs args)
        {
            await vm.Vote(args);
        }

        private void ViewRant(object sender, MouseButtonEventArgs e)
        {
            vm.ViewRant();
        }
    }
}
