using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DevRant.WPF
{
    public class ScrollHandler
    {
        private enum Type
        {
            ScrollViewer
        }

        private Type type;
        private object control;

        public const string PageUp = "PageUp";
        public const string PageDown = "PageDown";
        public const string Up = "Up";
        public const string Down = "Down";


        public ScrollHandler(ScrollViewer scrollViewer)
        {
            type = Type.ScrollViewer;
            control = scrollViewer;
        }

        public void Scroll(string direction)
        {
            if (string.IsNullOrEmpty(direction))
                return;

            if (type == Type.ScrollViewer)
            {
                var sv = (ScrollViewer)control;
                sv.Focus();

                switch (direction)
                {
                    case PageUp:
                    case Up:
                        sv.PageUp();
                        break;
                    case Down:
                    case PageDown:
                        sv.PageDown();
                        break;
                }
            }
        }
    }
}
