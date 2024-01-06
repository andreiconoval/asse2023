//------------------------------------------------------------------------------
// <copyright file="IUserService.cs" company="Transilvania University of Brasov">
// Copyright (c) Conoval. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
//------------------------------------------------------------------------------

namespace Library.BL.Interfaces
{
    using Library.DAL.DomainModel;

    /// <summary>
    /// User service interface.
    /// </summary>
    public interface IUserService : IService<User>
    {
        /// <summary>
        /// Deletes a user.
        /// </summary>
        /// <param name="id">The user ID.</param>
        void DeleteUser(int id);

        /// <summary>
        /// Adds a new user.
        /// </summary>
        /// <param name="user">The user entity to add.</param>
        /// <returns>The ID of the newly added user.</returns>
        int AddUser(User user);

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="user">The user entity to update.</param>
        void UpdateUser(User user);
    }
}
