using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevRant.WPF.DataStore;
using System.Collections.ObjectModel;
using DevRant.WPF.ViewModels;

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
            UpdatesFeed = new ObservableCollection<ViewModels.Rant>();
        }

        #region Updates Feed
        public ObservableCollection<ViewModels.Rant> UpdatesFeed { get; private set; }

        public void RemoveReadUpdates()
        {

        }

        #endregion

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

        internal void AddUpdate(Rant r)
        {
            //TODO: Need someway to check for dups before adding
            foreach (var existing in UpdatesFeed)
                if (existing.ID == r.ID)
                    return;

            UpdatesFeed.Add(r);
        }
    }
}
