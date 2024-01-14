//------------------------------------------------------------------------------
// <copyright file="BookService.cs" company="Transilvania University of Brasov">
// Copyright (c) Conoval. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
//------------------------------------------------------------------------------

namespace Library.BL.Services
{
    using System;
    using System.Linq;
    using FluentValidation.Results;
    using Library.BL.Interfaces;
    using Library.BL.Validators;
    using Library.DAL.DomainModel;
    using Library.DAL.Interfaces;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Book service.
    /// </summary>
    public class BookService : BaseService<Book, IBookRepository>, IBookService
    {
        /// <summary>
        /// Defines the bookDomainService.
        /// </summary>
        private readonly IBookDomainService bookDomainService;

        /// <summary>
        /// Defines the bookAuthorService.
        /// </summary>
        private readonly IBookAuthorService bookAuthorService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookService"/> class.
        /// </summary>
        /// <param name="repository">Book repository.</param>
        /// <param name="logger">Logger object.</param>
        /// <param name="bookDomainService">Book domain Service.</param>
        /// <param name="bookAuthorService">Book author service.</param>
        public BookService(
            IBookRepository repository,
            ILogger logger,
            IBookDomainService bookDomainService,
            IBookAuthorService bookAuthorService)
            : base(repository, new BookValidator(), logger)
        {
            this.bookDomainService = bookDomainService;
            this.bookAuthorService = bookAuthorService;
        }

        /// <summary>
        /// Add new book.
        /// </summary>
        /// <param name="entity">Book entity.</param>
        /// <returns>Validation result, Filled entity id with new one.</returns>
        public override ValidationResult Insert(Book entity)
        {
            try
            {
                var result = Validator.Validate(entity);

                if (!result.IsValid)
                {
                    Logger.LogInformation($"Cannot add new book, entity is invalid");
                    throw new ArgumentException("Cannot add new book, entity is invalid");
                }

                var bookExists = Repository.Get(i => entity.Title != null && i.Title != null && i.Title.Trim().ToLower() == entity.Title.Trim().ToLower()).Any();

                if (bookExists)
                {
                    Logger.LogInformation($"Cannot add new book, entity already exists");
                    throw new ArgumentException("Cannot add new book, entity already exists");
                }

                Repository.Insert(entity);
                Logger.LogInformation($"Add new book");
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "Log on error add");
                throw;
            }
        }

        /// <summary>
        /// Update book based on id.
        /// </summary>
        /// <param name="book">Book entity.</param>
        /// <returns>Validation result for entity.</returns>
        public override ValidationResult Update(Book book)
        {
            try
            {
                var result = Validator.Validate(book);

                if (!result.IsValid)
                {
                    Logger.LogInformation("Cannot update book, invalid entity");
                    throw new ArgumentException("Cannot update book, invalid entity");
                }

                var databaseBook = Repository.Get(i => i.Id == book.Id).FirstOrDefault();
                if (databaseBook == null)
                {
                    Logger.LogInformation("Cannot update book, entity is missing");
                    throw new ArgumentException("Cannot update book, entity is missing");
                }

                databaseBook.Title = book.Title;
                databaseBook.Id = book.Id;
                databaseBook.YearPublication = book.YearPublication;
                Repository.Update(databaseBook);

                return result;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "Log on error update");
                throw;
            }
        }

        /// <summary>
        /// Delete book from database based on Id.
        /// Hard delete will remove also all Book Domains and BookAuthor relations with hard delete flag,
        /// But will throw error if book have editions, instead of this mark book archived.
        /// </summary>
        /// <param name="book">Book to delete by Id.</param>
        /// <param name="hardDelete"> Flag that indicate to remove  book full.</param>
        public void Delete(Book book, bool hardDelete)
        {
            try
            {
                var fullDatabaseBook = Repository.Get(i => i.Id == book.Id, null, "BookDomains,BookAuthors,BookEditions").FirstOrDefault();
                if (fullDatabaseBook == null)
                {
                    Logger.LogInformation("Cannot delete book, entity is missing");
                    throw new ArgumentException("Cannot delete book, entity is missing");
                }

                if (fullDatabaseBook.BookEditions != null && fullDatabaseBook.BookEditions.Any())
                {
                    Logger.LogInformation("Cannot delete book with editions, mark archived instead");
                    throw new ArgumentException("Cannot delete book with editions, mark archived instead");
                }

                if (!hardDelete && ((fullDatabaseBook.BookDomains != null && fullDatabaseBook.BookDomains.Any()) || (fullDatabaseBook.BookAuthors != null && fullDatabaseBook.BookAuthors.Any())))
                {
                    Logger.LogInformation("Cannot delete book, entity has relations");
                    throw new ArgumentException("Cannot delete book, entity has relations");
                }

                if (fullDatabaseBook.BookDomains != null && fullDatabaseBook.BookDomains.Any())
                {
                    foreach (var bookDomain in fullDatabaseBook.BookDomains)
                    {
                        this.bookDomainService.Delete(bookDomain);
                    }
                }

                if (fullDatabaseBook.BookAuthors != null && fullDatabaseBook.BookAuthors.Any())
                {
                    foreach (var bookAuthor in fullDatabaseBook.BookAuthors)
                    {
                        this.bookAuthorService.Delete(bookAuthor);
                    }
                }

                base.Delete(book);
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "Log on error update");
                throw;
            }
        }

        /// <summary>
        /// Delete book.
        /// </summary>
        /// <param name="book">Book entity.</param>
        public override void Delete(Book book)
        {
            if (book == null)
            {
                throw new ArgumentException("Cannot delete book, book is missing");
            }

            this.Delete(book, false);
        }
    }
}
