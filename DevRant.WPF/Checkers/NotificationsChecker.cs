using DevRant.Dtos;
using DevRant.WPF.DataStore;
using DevRant.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;

namespace DevRant.WPF.Checkers
{
    internal class NotificationsChecker
    {
        private Thread checkerThread;

        private IDevRantClient api;
        private IDataStore ds;

        private int latestVersion = 0;
        private const int SleepTime = CheckInterval * 60 * 1000;
        private const int CheckInterval = 10; //Minutes
        
        public ObservableCollection<ViewModels.Notification> Notifications { get; private set; }

        public NotificationsChecker(IDataStore ds, IDevRantClient api)
        {
            this.ds = ds;
            this.api = api;

            Notifications = new ObservableCollection<ViewModels.Notification>();
        }

        public NotificationsChecker() : this(AppManager.Instance.Settings, AppManager.Instance.API)
        {
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
            public Exception Error { get; internal set; }
        }
        
        
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
                    await Check();

                    int millis = SleepTime;
                    Thread.Sleep(millis);
                }
            }
            catch (Exception ex)
            {
                SendUpdate(error: ex);
            }
        }

        public async Task Check()
        {
            var notifs = await api.User.GetNotificationsAsync();
            //var sorted = (from n in notifs orderby n.CreateTime descending, n.Read ascending select n);

            Notifications.Clear();

            foreach (var notif in notifs)
            {
                var vm = new ViewModels.Notification(notif);
                Notifications.Add(vm);
            }

            int unread = Notifications.Count(x => x.Read == false);

            SendUpdate(unread, Notifications.Count);
        }

        internal void SendUpdate(int unread = 0, int total = 0, Exception error = null)
        {
            if (OnUpdate != null)
            {
                var update = new UpdateArgs(unread, total);
                update.Error = error;

                App.Current.Dispatcher.Invoke(() => OnUpdate.Invoke(update));
            }
        }

        public void Restart()
        {
            Stop();
            Start();
        }
    }
}