//------------------------------------------------------------------------------
// <copyright file="AuthorService.cs" company="Transilvania University of Brasov">
// Copyright (c) Conoval. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
//------------------------------------------------------------------------------

namespace Library.BL.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Library.BL.Interfaces;
    using Library.BL.Validators;
    using Library.DAL.DomainModel;
    using Library.DAL.Interfaces;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Represents the service for managing authors.
    /// </summary>
    public class AuthorService : BaseService<Author, IAuthorRepository>, IAuthorService
    {
        /// <summary>
        /// Book author service instance.
        /// </summary>
        private readonly IBookAuthorService bookAuthorservice;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorService"/> class.
        /// </summary>
        /// <param name="authorRepository">The author repository.</param>
        /// <param name="bookAuthorService">The book author service.</param>
        /// <param name="logger">The logger.</param>
        public AuthorService(IAuthorRepository authorRepository, IBookAuthorService bookAuthorService, ILogger<IAuthorService> logger)
            : base(authorRepository, new AuthorValidator(), logger)
        {
            this.bookAuthorservice = bookAuthorService;
        }

        #region Public

        /// <summary>
        /// Deletes an author. Hard delete will delete also relations with books.
        /// </summary>
        /// <param name="id">Author Id.</param>
        /// <param name="hardDelete">Indicates whether to delete author book relations.</param>
        /// <exception cref="ArgumentException">Thrown when the ID is invalid or there are books linked to the author.</exception>
        public void DeleteAuthor(int id, bool hardDelete)
        {
            try
            {
                var author = GetByID(id);
                if (author == null)
                {
                    Logger.LogInformation($"Cannot delete author with id: {id}, id is invalid");
                    throw new ArgumentException($"Cannot delete author with id: {id}, id is invalid");
                }

                var bookAuthors = this.GetAuthorBooks(id);

                if ((!bookAuthors?.Any() ?? true) || hardDelete)
                {
                    if (hardDelete && bookAuthors != null)
                    {
                        foreach (var bookAuthor in bookAuthors)
                        {
                            this.bookAuthorservice.Delete(bookAuthor);
                        }

                        Logger.LogInformation($"All links to books were deleted");
                    }

                    this.Delete(author);
                }
                else
                {
                    Logger.LogInformation($"Cannot delete author with id: {id}, there are books liked to it");
                    throw new ArgumentException($"Cannot delete author with id: {id}, there are books liked to it");
                }
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets all books linked to an author.
        /// </summary>
        /// <param name="id">Author id.</param>
        /// <returns>List of author book relations.</returns>
        /// <exception cref="ArgumentException">Thrown when the author doesn't exist.</exception>
        public IEnumerable<BookAuthor> GetAuthorBooks(int id)
        {
            var author = Repository.Get(i => i.Id == id, null, "BookAuthors").FirstOrDefault();
            if (author == null)
            {
                Logger.LogInformation("Author doesn't exist");
                throw new ArgumentException("Author doesn't exist");
            }

            var books = author.BookAuthors ?? Enumerable.Empty<BookAuthor>();
            return books;
        }

        /// <summary>
        /// Adds a new author.
        /// </summary>
        /// <param name="author">New author.</param>
        /// <returns>Author id.</returns>
        /// <exception cref="ArgumentException">Thrown when the entity is invalid or the author already exists.</exception>
        public int AddAuthor(Author author)
        {
            try
            {
                var result = Validator.Validate(author);

                if (author == null || !result.IsValid)
                {
                    Logger.LogInformation("Cannot add author, invalid entity");
                    throw new ArgumentException("Cannot add author, invalid entity");
                }

                var authorExists = Repository.Get(i => i.FirstName != null &&
                                                        author.FirstName != null &&
                                                        i.FirstName.Trim() == author.FirstName.Trim() &&
                                                        i.LastName != null &&
                                                        author.LastName != null &&
                                                        i.LastName.Trim() == author.LastName.Trim()).Any();

                if (authorExists)
                {
                    Logger.LogInformation("Cannot add author, author already exists");
                    throw new ArgumentException("Cannot add author, author already exists");
                }

                Repository.Insert(author);

                return author.Id;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Updates an existing author.
        /// </summary>
        /// <param name="author">Author instance.</param>
        /// <exception cref="ArgumentException">Thrown when the entity is invalid or the author doesn't exist.</exception>
        public void UpdateAuthor(Author author)
        {
            var result = Validator.Validate(author);

            if (author == null || author.Id == 0 || !result.IsValid)
            {
                Logger.LogInformation("Cannot update author, invalid entity");
                throw new ArgumentException("Cannot update author, invalid entity");
            }

            var databaseAuthor = Repository.Get(i => i.Id == author.Id).FirstOrDefault();

            if (databaseAuthor == null)
            {
                Logger.LogInformation("Cannot update author, entity is missing");
                throw new ArgumentException("Cannot update author, entity is missing");
            }

            databaseAuthor.FirstName = author.FirstName;
            databaseAuthor.LastName = author.LastName;

            Repository.Update(databaseAuthor);
        }

        #endregion
    }
}