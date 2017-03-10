using DevRant.WPF.ViewModels;
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
    /// Interaction logic for RantViewerWindow.xaml
    /// </summary>
    public partial class RantViewerWindow : Window
    {
        private readonly RantViewerViewModel vm;
        private readonly ScrollHandler scroller;

        public RantViewerWindow(Rant rant, IDevRantClient api)
        {
            InitializeComponent();

            vm = new RantViewerViewModel(this, rant, api, Scroll);
            DataContext = vm;

            scroller = new ScrollHandler(ScrollViewer);

            //TODO: Check if this works
            Top = 10;
            Left = Utilities.GetLeft(Width);

            //ScrollViewer.Focus();
        }

        private void Scroll(string direction)
        {
            scroller.Scroll(direction);
        }

        //TODO: Need to change this to handle reply...
        private async void Button_Clicked(object sender, Controls.ButtonClickedEventArgs args)
        {
           await vm.ButtonClicked(args);
        }
        
    }
}
