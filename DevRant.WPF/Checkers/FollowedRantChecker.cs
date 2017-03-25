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
    internal class FollowedRantChecker
    {
        private Thread checkerThread;

        public UpdateArgs CheckFollowedRantsForUpdates()
        {
            return new UpdateArgs(UpdateType.UpdatedRants, 0, null, Posts);
        }

        public ObservableCollection<ViewModels.Rant> Posts { get; private set; }

        public FollowedRantChecker()
        {
            Posts = new ObservableCollection<ViewModels.Rant>();
        }

        public delegate void OnUpdatedHandler(UpdateArgs args);
        public event OnUpdatedHandler OnUpdate;

        private void SafeStart(object paramz)
        {
        }

        private int latestVersion = 0;

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
            AppManager manager = AppManager.Instance;

            try
            {
                while (true)
                {
                    if (!IsLatest(version))
                        break;

                    RemoveRead();

                    long lastTime = manager.Settings.FollowedRantsLastChecked;

                    if (lastTime == 0)
                        lastTime = Utilities.ToUnixTime(DateTime.Today);

                    List<ViewModels.Rant> added = new List<ViewModels.Rant>();

                    var rantIds = await GetFavoritedRantIds(manager);

                    foreach (long rantId in rantIds)
                    {
                        Dtos.Rant rant = await manager.API.GetRant(rantId);
                        long userId = rant.UserId;

                        foreach (var comment in rant.Comments)
                        {
                            if (comment.CreatedTime > lastTime)
                            {
                                added.Add(new ViewModels.Rant(rant));
                            }
                        }
                    }

                    if (!IsLatest(version))
                        break;

                    if (added.Count > 0)
                    {
                        long latest = added.Max(x => x.RawCreateTime);

                        foreach (var r in added)
                        {
                            Posts.Add(r);
                        }

                        manager.Settings.FollowedRantsLastChecked = latest;
                    }

                    SendUpdate(UpdateType.UpdatedRants, added.Count);

                    Thread.Sleep(manager.Settings.FollowedRantsInterval);
                }
            }
            catch (Exception ex)
            {
                SendUpdate(UpdateType.Error, error: ex);
            }
        }

        private async Task<List<long>> GetFavoritedRantIds(AppManager manager)
        {
            List<long> ids = new List<long>();

            //TODO: Get all, paging how?
            Profile profile;

            int skipCounter = 0;

            while (true)
            {
                profile = await manager.API.GetProfileAsync(manager.API.User.LoggedInUser, Enums.ProfileContentType.Favorites, skipCounter);

                if (profile.Rants.Count == 0)
                    break;

                foreach (Dtos.Rant rant in profile.Rants)
                {
                    ids.Add(rant.Id);
                }
            }

            return ids;
        }

        private void RemoveRead()
        {
            var read = Posts.Where(x => x.Read).ToList();

            foreach (var r in read)
                Posts.Remove(r);
        }

        internal void SendUpdate(UpdateType type, int added = 0, string users = null, Exception error = null)
        {
            if (OnUpdate != null)
            {
                var update = new UpdateArgs(type, added, users, Posts);
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