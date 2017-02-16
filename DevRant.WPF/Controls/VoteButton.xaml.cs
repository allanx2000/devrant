using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace DevRant.WPF.Controls
{
    /// <summary>
    /// Interaction logic for VoteButton.xaml
    /// </summary>
    public partial class VoteButton : MyUserControl
    {
        
        public enum ButtonType
        {
            Up,
            Down
        }

        public delegate void OnClick(object sender, ButtonType type);
        public event OnClick Clicked;
        
        public ButtonType Type
        {
            get
            {
                return (ButtonType) GetValue(TypeProperty);
            }
            set
            {
                SetValue(TypeProperty, value);
            }
        }

        private static SolidColorBrush Red = new SolidColorBrush(Colors.Red);
        private static SolidColorBrush Gray = new SolidColorBrush(Colors.Gray);

        public SolidColorBrush BackgroundColor
        {
            get
            {
                return IsMouseOver ? Red : Gray;
            }
        }
            

        public string ButtonText
        {
            get { return Type == VoteButton.ButtonType.Up ? "++" : "--"; }
        }


        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register("Type", typeof(ButtonType), typeof(VoteButton), null);
        
        public VoteButton()
        {
            InitializeComponent();
        }

        private void voteButton_MouseEnter(object sender, MouseEventArgs e)
        {
            RaisePropertyChange("BackgroundColor");
        }

        private void voteButton_MouseLeave(object sender, MouseEventArgs e)
        {
            RaisePropertyChange("BackgroundColor");
        }
    }
}
