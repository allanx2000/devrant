using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevRant.WPF
{
    class MessageCollection
    {
        public delegate void OnMessageChanged();

        public event OnMessageChanged Changed;

        private List<string> messages = new List<string>();
        
        public MessageCollection()
        {
        }

        public string LastMessage
        {
            get
            {
                if (messages.Count == 0)
                    return null;
                else
                    return messages.Last();
            }
        }

        public void AddMessage(string message)
        {
            messages.Add(message);
            Notify();
        }

        public void Clear()
        {
            messages.Clear();
            Notify();
        }

        private void Notify()
        {
            if (Changed != null)
                Changed.Invoke();
        }
    }
}
