//------------------------------------------------------------------------------
// <copyright file="IBookService.cs" company="Transilvania University of Brasov">
// Copyright (c) Conoval. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
//------------------------------------------------------------------------------

namespace Library.BL.Interfaces
{
    using Library.DAL.DomainModel;

    /// <summary>
    /// Represents the service interface for managing books.
    /// </summary>
    public interface IBookService : IService<Book>
    {
        /// <summary>
        ///  Deletes an book based on the specified ID.
        /// </summary>
        /// <param name="book">Book entity</param>
        /// <param name="hardDelete">Flag to delete all relations</param>
        void Delete(Book book, bool hardDelete);
    }
}
