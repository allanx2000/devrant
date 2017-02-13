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
    /// Interaction logic for StatusViewerWindow.xaml
    /// </summary>
    public partial class StatusViewerWindow : Window
    {
        private readonly StatusViewerWindowViewModel vm;
        public StatusViewerWindow(MessageCollection messages)
        {
            vm = new StatusViewerWindowViewModel(messages, this);
            DataContext = vm;

            InitializeComponent();
        }
    }
}
