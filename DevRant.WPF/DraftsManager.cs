using DevRant.WPF.DataStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevRant.WPF
{
    class DraftsManager
    {
        private IPersistentDataStore db;

        public Action OnDatabaseUpdated;

        public DraftsManager(IPersistentDataStore db)
        {
            this.db = db;
        }


    }
}
