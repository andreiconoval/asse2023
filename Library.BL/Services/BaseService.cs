//------------------------------------------------------------------------------
// <copyright file="BaseService.cs" company="Transilvania University of Brasov">
// Copyright (c) Conoval. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
//------------------------------------------------------------------------------

namespace Library.BL.Services
{
    using System;
    using System.Collections.Generic;
    using FluentValidation;
    using FluentValidation.Results;
    using Library.BL.Interfaces;
    using Library.DAL.Interfaces;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Abstract base service providing common CRUD operations.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    /// <typeparam name="U">The type of repository for the entity.</typeparam>
    public abstract class BaseService<T, U> : IService<T>
         where T : class
         where U : IRepository<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseService{T,U}" /> class.
        /// Constructor for the BaseService class.
        /// </summary>
        /// <param name="repository">The actual repository that will fit into this class.</param>
        /// <param name="validator">The Fluent Validation validator.</param>
        /// <param name="logger">The Logger.</param>
        public BaseService(U repository, IValidator<T> validator, ILogger logger)
        {
            this.Repository = repository;
            this.Validator = validator;
            this.Logger = logger;
        }

        /// <summary>
        /// Gets or sets the actual repository that will fit into this class.
        /// </summary>
        internal U Repository { get; set; }

        /// <summary>
        /// Gets or sets the validator instance.
        /// </summary>
        internal IValidator<T> Validator { get; set; }

        /// <summary>
        /// Gets or sets the logger instance.
        /// </summary>
        internal ILogger Logger { get; set; }

        /// <summary>Inserts the specified entity.</summary>
        /// <param name="entity">The entity.</param>
        /// <returns>
        ///   The validation result.
        /// </returns>
        public virtual ValidationResult Insert(T entity)
        {
            try
            {
                var result = this.Validator.Validate(entity);
                if (result.IsValid)
                {
                    this.Repository.Insert(entity);
                    this.Logger.LogInformation($"Add new {nameof(T)}");
                }
                else
                {
                    this.Logger.LogInformation($"Cannot add new {nameof(T)}, entity is invalid");
                }

                return result;
            }
            catch (Exception ex)
            {
                this.Logger.LogCritical(ex, "Log on error add");
                throw;
            }
        }

        /// <inheritdoc/>
        public virtual ValidationResult Update(T entity)
        {
            var result = this.Validator.Validate(entity);
            if (result.IsValid)
            {
                this.Repository.Update(entity);
            }

            return result;
        }

        /// <summary>Deletes the specified entity.</summary>
        /// <param name="entity">The entity to be deleted.</param>
        public virtual void Delete(T entity)
        {
            this.Repository.Delete(entity);
        }

        /// <summary>
        /// Get entity based on Id
        /// </summary>
        /// <param name="id"> Id of entity</param>
        /// <returns>T entity</returns>
        public virtual T GetByID(object id)
        {
            return this.Repository.GetByID(id);
        }

        /// <summary>
        /// Get all entities
        /// </summary>
        /// <returns>Return list of entities</returns>
        public virtual IEnumerable<T> GetAll()
        {
            return this.Repository.Get();
        }
    }
}
