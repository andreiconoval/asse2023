//------------------------------------------------------------------------------
// <copyright file="UserService.cs" company="Transilvania University of Brasov">
// Copyright (c) Conoval. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
//------------------------------------------------------------------------------

namespace Library.BL.Services
{
    using System;
    using System.Linq;
    using Library.BL.Interfaces;
    using Library.BL.Validators;
    using Library.DAL.DomainModel;
    using Library.DAL.Interfaces;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Defines the <see cref="UserService" />.
    /// </summary>
    public class UserService : BaseService<User, IUserRepository>, IUserService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="repository">The repository<see cref="IUserRepository"/>.</param>
        /// <param name="logger">The logger<see cref="ILogger{IUserService}"/>.</param>
        public UserService(IUserRepository repository, ILogger<IUserService> logger) : base(repository, new UserValidator(), logger)
        {
        }

        /// <summary>
        /// Add new user.
        /// </summary>
        /// <param name="user">user model.</param>
        /// <returns>New user Id.</returns>
        public int AddUser(User user)
        {
            try
            {
                var result = Validator.Validate(user);

                if (user == null || !result.IsValid)
                {
                    Logger.LogInformation("Cannot add user, invalid entity");
                    throw new ArgumentException("Cannot add user, invalid entity");
                }

                var authorExists = Repository.Get(i => i.Email.Trim() == user.Email.Trim()).Any();

                if (authorExists)
                {
                    Logger.LogInformation("Cannot add user, user already exists");
                    throw new ArgumentException("Cannot add user, user already exists");
                }

                Repository.Insert(user);

                return user.Id;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Delete user.
        /// </summary>
        /// <param name="id">Entity id.</param>
        public void DeleteUser(int id)
        {
            try
            {
                var user = GetByID(id);
                if (user == null)
                {
                    Logger.LogInformation($"Cannot delete user with id: {id}, id is invalid");
                    throw new ArgumentException($"Cannot delete user with id: {id}, id is invalid");
                }

                this.Delete(user);
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Update user based on email or id
        /// Email and Id are not updated.
        /// </summary>
        /// <param name="user">User entity.</param>
        public void UpdateUser(User user)
        {
            var result = Validator.Validate(user);

            if (user == null || user.Id == 0 || !result.IsValid)
            {
                Logger.LogInformation("Cannot update user, invalid entity");
                throw new ArgumentException("Cannot update user, invalid entity");
            }

            var databaseAuthor = Repository
                .Get(i => i.Id == user.Id || (i.Email != null && user.Email != null && i.Email.Trim().ToLower() == user.Email.Trim().ToLower()))
                .FirstOrDefault();

            if (databaseAuthor == null)
            {
                Logger.LogInformation("Cannot update user, entity is missing");
                throw new ArgumentException("Cannot update user, entity is missing");
            }

            databaseAuthor.FirstName = user.FirstName;
            databaseAuthor.LastName = user.LastName;
            databaseAuthor.Phone = user.Phone;
            databaseAuthor.Address = user.Address;

            Repository.Update(databaseAuthor);
        }
    }
}
