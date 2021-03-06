﻿using DevRant.WPF.DataStore;
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

        public SavedPostContent AddedDraft { get { return vm.AddedDraft; } }

        private EditPostWindow()
        {
        }

        private EditPostWindow(FeedItem existing) : this(ToType(existing.Type), edit: existing)
        {
        }

        private static Type ToType(FeedItem.FeedItemType type)
        {
            switch (type)
            {
                case FeedItem.FeedItemType.Post:
                    return Type.Rant;
                case FeedItem.FeedItemType.Comment:
                    return Type.Comment;
                default:
                    throw new NotSupportedException();
            }
        }

        private EditPostWindow(Type type, Draft existing = null, Commentable parent = null, FeedItem edit = null)
        {
            InitializeComponent();

            vm = new EditPostWindowViewModel(this, type, existing, parent, edit);
            DataContext = vm;
        }



        public static EditPostWindow CreateForRant(Draft existing = null)
        {
            var window = new EditPostWindow(Type.Rant, existing);
            return window;
        }


        public static EditPostWindow CreateForComment(IDevRantClient api, long rantId)
        {
            Commentable dummy = new ViewModels.DummyCommentable(rantId);
            return CreateForComment(api, dummy);
        }

        public static EditPostWindow CreateForComment(IDevRantClient api, Commentable parent)
        {
            var window = new EditPostWindow(Type.Comment, parent: parent);
            return window;
        }
        
        public static EditPostWindow CreateForEdit(IDevRantClient api, FeedItem existing)
        {
            var window = new EditPostWindow(existing);
            return window;
        }
        
    }
}
