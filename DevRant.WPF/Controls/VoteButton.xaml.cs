﻿using System;
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
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register("Type", typeof(ButtonType), typeof(VoteButton), null);
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(VoteButton), null);
        
        public enum ButtonType
        {
            Up,
            Down
        }

        public delegate void OnClick(object sender, ButtonType type);
        public event OnClick Clicked;
        
        public Visibility PlusPlusVisibility
        {
            get { return Type == ButtonType.Up ? Visibility.Visible : Visibility.Collapsed; }
        }

        public Visibility MinusMinusVisibility
        {
            get { return Type == ButtonType.Down ? Visibility.Visible : Visibility.Collapsed; }
        }


        public bool IsSelected
        {
            get
            {
                return (bool)GetValue(IsSelectedProperty);
            }
            set
            {
                SetValue(IsSelectedProperty, value);
                RaisePropertyChange();
                RaisePropertyChange("BackgroundColor");
            }
        }

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

        private static SolidColorBrush Red = new SolidColorBrush(Color.FromRgb(213, 81, 97));
        private static SolidColorBrush Gray = new SolidColorBrush(Color.FromRgb(170,170,184));
        

        public SolidColorBrush BackgroundColor
        {
            get
            {
                return IsSelected || IsMouseOver ? Red : Gray;
            }
        }
            

        public string ButtonText
        {
            get { return Type == VoteButton.ButtonType.Up ? "++" : "--"; }
        }
        
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
