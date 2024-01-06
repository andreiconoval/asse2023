//------------------------------------------------------------------------------
// <copyright file="IService.cs" company="Transilvania University of Brasov">
// Copyright (c) Conoval. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
//------------------------------------------------------------------------------

namespace Library.BL.Interfaces
{
    using System.Collections.Generic;
    using FluentValidation.Results;

    /// <summary>
    /// Represents the generic service interface for managing entities.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    public interface IService<T>
        where T : class
    {
        /// <summary>
        /// Inserts a new entity.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        /// <returns>The validation result for the insert operation.</returns>
        ValidationResult Insert(T entity);

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>The validation result for the update operation.</returns>
        ValidationResult Update(T entity);

        /// <summary>
        /// Deletes an entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        void Delete(T entity);

        /// <summary>
        /// Gets an entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <returns>The entity with the specified ID.</returns>
        T GetByID(object id);

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <returns>An enumerable of all entities.</returns>
        IEnumerable<T> GetAll();
    }
}
