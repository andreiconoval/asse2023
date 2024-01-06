//------------------------------------------------------------------------------
// <copyright file="BookEditionService.cs" company="Transilvania University of Brasov">
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
    /// Defines the <see cref="BookEditionService" />.
    /// </summary>
    public class BookEditionService : BaseService<BookEdition, IBookEditionRepository>, IBookEditionService
    {
        /// <summary>
        /// Defines the _bookRepository.
        /// </summary>
        private readonly IBookRepository bookRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookEditionService"/> class.
        /// </summary>
        /// <param name="repository">The repository<see cref="IBookEditionRepository"/>.</param>
        /// <param name="logger">The logger<see cref="ILogger"/>.</param>
        /// <param name="bookRepository">The bookRepository<see cref="IBookRepository"/>.</param>
        public BookEditionService(IBookEditionRepository repository, ILogger logger, IBookRepository bookRepository) : base(repository, new BookEditionValidator(), logger)
        {
            this.bookRepository = bookRepository;
        }

        /// <summary>
        /// Add new book edition.
        /// </summary>
        /// <param name="entity">Book edition.</param>
        /// <returns>Validation result and fill entity model with  id.</returns>
        public override ValidationResult Insert(BookEdition entity)
        {
            try
            {
                var result = Validator.Validate(entity);

                if (!result.IsValid)
                {
                    Logger.LogInformation("Cannot add new book edition, entity is invalid");
                    throw new ArgumentException("Cannot add new book edition, entity is invalid");
                }

                var editionExists = Repository.Get(i => entity.Edition != null && i.Edition != null && i.BookId == entity.BookId && i.Edition.Trim().ToLower() == entity.Edition.Trim().ToLower()).Any();

                if (editionExists)
                {
                    Logger.LogInformation("Cannot add new book edition, entity already exists");
                    throw new ArgumentException("Cannot add new book edition, entity already exists");
                }

                var bookExists = this.bookRepository.Get(i => i.Id == entity.BookId).Any();

                if (!bookExists)
                {
                    Logger.LogInformation("Cannot add new book edition, book is missing");
                    throw new ArgumentException("Cannot add new book edition, book is missing");
                }

                Repository.Insert(entity);
                Logger.LogInformation($"Add new book edition");
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "Log on error add");
                throw;
            }
        }

        /// <summary>
        /// Update book edition based on id.
        /// </summary>
        /// <param name="bookEdition">Book edition entity.</param>
        /// <returns>Validation result for entity.</returns>
        public override ValidationResult Update(BookEdition bookEdition)
        {
            try
            {
                var result = Validator.Validate(bookEdition);

                if (!result.IsValid)
                {
                    Logger.LogInformation("Cannot update book edition, invalid entity");
                    throw new ArgumentException("Cannot update book edition, invalid entity");
                }

                var databaseBookEdition = Repository.Get(i => i.Id == bookEdition.Id).FirstOrDefault();
                if (databaseBookEdition == null)
                {
                    Logger.LogInformation("Cannot update book edition, entity is missing");
                    throw new ArgumentException("Cannot update book edition, entity is missing");
                }

                if (databaseBookEdition.BookId != bookEdition.BookId)
                {
                    Logger.LogInformation($"Cannot add update book edition, book id was changed");
                    throw new ArgumentException("Cannot add update book edition, book id was changed");
                }

                databaseBookEdition.Edition = bookEdition.Edition;
                databaseBookEdition.PageNumber = bookEdition.PageNumber;
                databaseBookEdition.BookType = bookEdition.BookType;
                databaseBookEdition.ReleaseYear = bookEdition.ReleaseYear;
                Repository.Update(databaseBookEdition);
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "Log on error update");
                throw;
            }
        }

        /// <summary>
        /// Delete book edition from database based on Id.
        /// But will throw error if book edition have BookSamples.
        /// </summary>
        /// <param name="bookEdition">Book edition to delete by Id.</param>
        public override void Delete(BookEdition bookEdition)
        {
            try
            {
                var fullDatabaseBook = Repository.Get(i => i.Id == bookEdition.Id, null, "BookSamples").FirstOrDefault();
                if (fullDatabaseBook == null)
                {
                    Logger.LogInformation("Cannot delete book edition, entity is missing");
                    throw new ArgumentException("Cannot delete book edition, entity is missing");
                }

                if (fullDatabaseBook.BookSamples != null && fullDatabaseBook.BookSamples.Any())
                {
                    Logger.LogInformation("Cannot delete book edition, entity has relations");
                    throw new ArgumentException("Cannot delete book edition, entity has relations");
                }

                base.Delete(bookEdition);
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "Log on error update");
                throw;
            }
        }
    }
}
