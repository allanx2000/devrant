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
    /// Interaction logic for CreateRantWindow.xaml
    /// </summary>
    public partial class CreateRantWindow : Window
    {
        private const int MaxLength = 500;
        private readonly CreateRantWindowViewModel vm;

        public bool Cancelled
        {
            get { return vm.Cancelled; }
        }

        public CreateRantWindow(IDevRantClient api)
        {
            InitializeComponent();

            vm = new CreateRantWindowViewModel(this, api);
            DataContext = vm;
        }
        
    }
}
