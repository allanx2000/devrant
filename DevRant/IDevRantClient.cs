using System.Collections.Generic;
using System.Threading.Tasks;
using DevRant.Dtos;
using DevRant.Enums;

namespace DevRant
{
    /// <summary>
    /// 
    /// </summary>
    public interface IFeeds
    {
        /// <summary>
        /// Requests a collection of collabs
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="skip"></param>
        /// <returns></returns>
        Task<IReadOnlyCollection<Collab>> GetCollabsAsync(int limit = 50, int skip = 0);


        /// <summary>
        /// Requests a collection of rants sorted and selected by the arguments from the rest-api.
        /// </summary>
        /// <param name="sort">Sorting of the rant collection.</param>
        /// <param name="limit">Maximal rants to return.</param>
        /// <param name="skip">Number of rants to skip.</param>
        /// <param name="settings">If passed, will hold the values of settings that may be returned in the response</param>
        Task<IReadOnlyCollection<Rant>> GetRantsAsync(RantSort sort = RantSort.Algo, int limit = 50, int skip = 0, SettingsCollection settings = null);
        /// <summary>
        /// Requests a collection of stories 
        /// </summary>
        /// <param name="sort">Sorting of the rant collection.</param>
        /// <param name="range">Range of stories</param>
        /// <param name="limit">Maximal rants to return.</param>
        /// <param name="skip">Number of rants to skip.</param>
        /// <returns></returns>
        Task<IReadOnlyCollection<Rant>> GetStoriesAsync(RantSort sort = RantSort.Top, StoryRange range = StoryRange.Day, int limit = 50, int skip = 0);
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IUserCommands
    {
        /// <summary>
        /// 
        /// </summary>
        AccessInfo Token { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<List<Dtos.Notification>> GetNotificationsAsync();

        /// <summary>
        /// 
        /// </summary>
        bool LoggedIn { get; }

        /// <summary>
        /// 
        /// </summary>
        string LoggedInUser { get; }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        Task Login(string username, string password);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task Logout();
    }

    /// <summary>
    /// Represents an interface which describes the public api for devrant.
    /// </summary>
    public interface IDevRantClient
    {
        /// <summary>
        /// 
        /// </summary>
        IFeeds Feeds { get; }

        /// <summary>
        /// 
        /// </summary>
        IUserCommands User { get; }

        /// <summary>
        /// Requests profile details to the rest-api.
        /// </summary>
        /// <param name="username">Username of the profile to request.</param>
        Task<Profile> GetProfileAsync(string username);

        /// <summary>
        /// Checks if user exists
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<bool> IsValidUser(string username);
    }

}