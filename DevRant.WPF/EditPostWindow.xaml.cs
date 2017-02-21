using DevRant.WPF.DataStore;
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
    /// Interaction logic for CreateRantWindow.xaml
    /// </summary>
    public partial class EditPostWindow : Window
    {
        private const int MaxLength = 500;
        private readonly EditPostWindowViewModel vm;

        public bool Cancelled
        {
            get { return vm.Cancelled; }
        }

        public enum Type
        {
            Comment,
            Rant
        }

        private EditPostWindow()
        {
        }

        public static EditPostWindow CreateForRant(IDevRantClient api, IPersistentDataStore db, Draft existing = null)
        {
            var window = new EditPostWindow(Type.Rant, api, db, existing);
            return window;
        }

        public static EditPostWindow CreateForComment(IDevRantClient api, Commentable parent)
        {
            var window = new EditPostWindow(Type.Comment, api, parent: parent);
            return window;
        }

        private EditPostWindow(Type type, IDevRantClient api, IPersistentDataStore db = null, Draft existing = null, Commentable parent = null)
        {
            InitializeComponent();

            vm = new EditPostWindowViewModel(this, type, api, db, existing, parent);
            DataContext = vm;
        }
        
    }
}
