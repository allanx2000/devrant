using DevRant.Dtos;
using DevRant.WPF.DataStore;
using DevRant.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Permissions;
using System.Threading;

namespace DevRant.WPF
{
    internal class NotificationsChecker
    {
        private Thread checkerThread;

        private IDevRantClient api;
        private IDataStore ds;
        private IHistoryStore history;
        
        public ObservableCollection<Notification> Notifications { get; private set; }

        public NotificationsChecker(IDataStore ds, IDevRantClient api)
        {
            this.ds = ds;
            this.api = api;

            Notifications = new ObservableCollection<Notification>();
        }
        
        public delegate void OnUpdatedHandler(UpdateArgs args);
        public event OnUpdatedHandler OnUpdate;
        
        public class UpdateArgs
        {
            public UpdateArgs(int unread, int total)
            {
                Total = total;
                TotalUnread = unread;
            }

            public int Total { get; private set; }
            public int TotalUnread { get; private set; }
            public string Error { get; internal set; }
        }
        
        private int latestVersion = 0;
        private const int SleepTime = 5 * 60 * 1000;

        public void Start()
        {
            latestVersion++;

            int v = latestVersion;
            checkerThread = new Thread(() => RunChecker(v));
            checkerThread.Start();
        }

        public void Stop()
        {
            checkerThread.Abort();
            latestVersion++;
        }

        private bool IsLatest(int version)
        {
            return version == latestVersion;
        }
        private async void RunChecker(int version)
        {
            try
            {
                while (true)
                {
                    if (!IsLatest(version))
                        break;

                    var notifs = await api.GetNotificationsAsync();
                    var sorted = (from n in notifs orderby n.CreateTime descending, n.Read ascending select n);

                    Notifications.Clear();
                    
                    foreach (var notif in sorted)
                    {
                        var vm = new Notification(notif);
                        Notifications.Add(vm);        
                    }

                    int unread = Notifications.Count(x => x.Read == false);

                    SendUpdate(unread, Notifications.Count);

                    int millis = SleepTime;
                    Thread.Sleep(millis);
                }
            }
            catch (Exception ex)
            {
                SendUpdate(error: ex.Message);
            }
        }

        internal void SendUpdate(int unread = 0, int total = 0, string error = null)
        {
            if (OnUpdate != null)
            {
                var update = new UpdateArgs(unread, total);
                update.Error = error;

                OnUpdate.Invoke(update);
            }
        }

        public void Restart()
        {
            Stop();
            Start();
        }
    }
}