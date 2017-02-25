using DevRant.Enums;
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
    /// Interaction logic for DownVoteReasonWindow.xaml
    /// </summary>
    public partial class RantIDInputWindow : Window
    {
        private RantIDInputWindowViewModel vm;
        
        public long? RantId { get { return vm.RantID; } } 

        public RantIDInputWindow()
        {
            InitializeComponent();
            vm = new RantIDInputWindowViewModel(this);
            DataContext = vm;
        }
    }
}
