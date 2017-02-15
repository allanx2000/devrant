using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevRant.WPF.DataStore
{
    public interface IHistoryStore
    {
        void MarkRead(int postId);
        bool IsRead(int postId);
    }
}
