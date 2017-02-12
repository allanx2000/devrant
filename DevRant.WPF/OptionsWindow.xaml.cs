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
    /// Interaction logic for OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow : Window
    {
        private readonly OptionsWindowViewModel vm;
        public OptionsWindow(IDataStore ds, IDevRantClient api)
        {
            vm = new OptionsWindowViewModel(ds, api, this);
            DataContext = vm;

            InitializeComponent();
        }

        public bool Cancelled { get { return vm.Cancelled; } }
    }
}
