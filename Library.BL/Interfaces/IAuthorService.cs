//------------------------------------------------------------------------------
// <copyright file="IAuthorService.cs" company="Transilvania University of Brasov">
// Copyright (c) Conoval. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
//------------------------------------------------------------------------------

namespace Library.BL.Interfaces
{
    using System.Collections.Generic;
    using Library.DAL.DomainModel;

    /// <summary>
    /// Represents the service interface for managing authors.
    /// </summary>
    public interface IAuthorService : IService<Author>
    {
        /// <summary>
        /// Deletes an author based on the specified ID.
        /// </summary>
        /// <param name="id">The ID of the author to delete.</param>
        /// <param name="hardDelete">True if a hard delete should be performed; otherwise, false.</param>
        void DeleteAuthor(int id, bool hardDelete);

        /// <summary>
        /// Retrieves the books associated with the specified author ID.
        /// </summary>
        /// <param name="id">The ID of the author.</param>
        /// <returns>An enumerable of BookAuthor objects representing the books associated with the author.</returns>
        IEnumerable<BookAuthor> GetAuthorBooks(int id);

        /// <summary>
        /// Adds a new author.
        /// </summary>
        /// <param name="author">The author to add.</param>
        /// <returns>The ID of the added author.</returns>
        int AddAuthor(Author author);

        /// <summary>
        /// Updates an existing author.
        /// </summary>
        /// <param name="author">The updated author information.</param>
        void UpdateAuthor(Author author);
    }
}
