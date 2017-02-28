using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevRant.WPF.DataStore;

namespace DevRant.WPF
{
    internal class AppManager
    {
        private static AppManager instance;
        
        public static AppManager Instance
        {
            get
            {
                if (instance == null)
                    throw new Exception("The instance has not been initialized");
                else
                    return instance;
            }
        }

        private AppManager()
        {

        }

        public IDevRantClient API { get; private set; }
        public IDataStore Settings { get; private set; }
        public IPersistentDataStore DB { get; private set; }

        public static void Initialize(IDevRantClient api, DataStore.IDataStore settings, DataStore.IPersistentDataStore db)
        {
            instance = new AppManager()
            {
                API = api,
                Settings = settings,
                DB = db
            };
        }
    }
}
