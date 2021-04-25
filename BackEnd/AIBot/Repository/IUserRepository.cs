using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Buaa.AIBot.Repository.Models;

namespace Buaa.AIBot.Repository
{
    interface IUserRepository
    {
        /// <summary>
        /// Select a User by uid.
        /// </summary>
        /// <param name="userId">uid</param>
        /// <returns>a UserInfo object if exist, or null</returns>
        Task<UserInfo> SelectUserByIdAsync(int userId);

        /// <summary>
        /// Select a User by email.
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>a UserInfo object if exist, or null</returns>
        Task<UserInfo> SelectUserByEmailAsync(string email);

        /// <summary>
        /// Select the bcrypt result of a User by email.
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>a bvrypt object if exist, or null</returns>
        Task<string> SelectBcryptByEmailAsync(string email);

        /// <summary>
        /// Select all qid by uid;
        /// </summary>
        /// <param name="userId">uid</param>
        /// <returns>list of qid, may empty</returns>
        Task<IEnumerable<int>> SelectQuestionsIdByIdAsync(int userId);

        /// <summary>
        /// Select all qid by uid;
        /// </summary>
        /// <param name="userId">uid</param>
        /// <returns>list of aid, may empty</returns>
        Task<IEnumerable<int>> SelectAnswersIdByIdAsync(int userId);

        /// <summary>
        /// Insert a new user. UserId will be generated automatically.
        /// </summary>
        /// <remarks>
        /// No operation if any exception occurs.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Email, Bcrypt, or Name is null.</exception>
        /// <exception cref="EmailHasExistException">There is already a user using the same email with <paramref name="user"/>.Email</exception>
        /// <param name="user">the new user to store</param>
        /// <returns>uid</returns>
        Task<int> InsertUserAsync(UserInfo user);

        /// <summary>
        /// Update the user with <paramref name="user"/>.UserId. 
        /// </summary>
        /// <remarks>
        /// Use <paramref name="user"/>.UserId to appoint the user to be update
        /// Every <paramref name="user"/>'s not-null (for Auth, AuthLevel.None) Property will replace the old value.
        /// Email will never be change although <paramref name="user"/>.Email is not null.
        /// No operation if any exception occurs.
        /// </remarks>
        /// <exception cref="UserNotExistException">There is no user with uesr.UserId == <paramref name="user"/>.UserId</exception>
        /// <param name="user">the new info for the user</param>
        /// <returns></returns>
        Task UpdateUserAsync(UserInfo user);

        /// <summary>
        /// Make sure no user whose Id is <paramref name="userId"/>. (no operation if it has already not exist).
        /// </summary>
        /// <param name="userId">uid</param>
        /// <returns></returns>
        Task DeleteUserByIdAsync(int userId);
    }
}
