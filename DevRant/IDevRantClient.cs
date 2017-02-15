using System.Collections.Generic;
using System.Threading.Tasks;
using DevRant.Dtos;

namespace DevRant
{
    /// <summary>
    /// Represents an interface which describes the public api for devrant.
    /// </summary>
    public interface IDevRantClient
    {
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

        /// <summary>
        /// Requests notification details
        /// </summary>
        /// <param name="username">Username of the profile to request.</param>
        //Task<Profile> GetNotificationsAsync(string username);


        /// <summary>
        /// Requests a collection of rants sorted and selected by the arguments from the rest-api.
        /// </summary>
        /// <param name="sort">Sorting of the rant collection.</param>
        /// <param name="limit">Maximal rants to return.</param>
        /// <param name="skip">Number of rants to skip.</param>
        Task<IReadOnlyCollection<RantInfo>> GetRantsAsync(RantSort sort = RantSort.Algo, int limit = 50, int skip = 0);
        /// <summary>
        /// Requests a collection of stories 
        /// </summary>
        /// <param name="sort">Sorting of the rant collection.</param>
        /// <param name="range">Range of stories</param>
        /// <param name="limit">Maximal rants to return.</param>
        /// <param name="skip">Number of rants to skip.</param>
        /// <returns></returns>
        Task<IReadOnlyCollection<RantInfo>> GetStoriesAsync(RantSort sort = RantSort.Top, StoryRange range = StoryRange.Day, int limit = 50, int skip = 0);

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        Task Login(string username, string password);
    }
    
}