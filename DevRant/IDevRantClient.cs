using System.Collections.Generic;
using System.Threading.Tasks;
using DevRant.Dtos;
using DevRant.Enums;

namespace DevRant
{
    /// <summary>
    /// API commands to get feeds
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
    /// API commands to perform actions related to the user
    /// </summary>
    public interface IUserCommands
    {
        /// <summary>
        /// 
        /// </summary>
        AccessInfo Token { get; }

        /// <summary>
        /// Returns the user's notifications
        /// </summary>
        /// <returns></returns>
        Task<List<Dtos.Notification>> GetNotificationsAsync();

        /// <summary>
        /// Whether a user is logged in. Certain methods may throw an NotLoggedInException if authentication is required
        /// </summary>
        bool LoggedIn { get; }

        /// <summary>
        /// The username of the current user
        /// </summary>
        string LoggedInUser { get; }

        /// <summary>
        /// Tries to login. May throw an exception if failed.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        Task Login(string username, string password);

        /// <summary>
        /// Logs out the user if he is logged in.
        /// </summary>
        /// <returns></returns>
        Task Logout();

        /// <summary>
        /// Vote on a rant
        /// </summary>
        /// <param name="rantId"></param>
        /// <param name="vote"></param>
        /// <returns></returns>
        Task<Rant> VoteRant(long rantId, Vote vote);

        /// <summary>
        /// Uploads a rant
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task UploadRant(PostContent data);

        /// <summary>
        /// Vote on a comment
        /// </summary>
        /// <param name="commentId"></param>
        /// <param name="vote"></param>
        /// <returns></returns>
        Task<Comment> VoteComment(long commentId, Vote vote);
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