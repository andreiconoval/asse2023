//------------------------------------------------------------------------------
// <copyright file="BookAuthorService.cs" company="Transilvania University of Brasov">
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
    /// Defines the <see cref="BookAuthorService" />.
    /// </summary>
    public class BookAuthorService : BaseService<BookAuthor, IBookAuthorRepository>, IBookAuthorService
    {
        /// <summary>
        /// Defines the authorRepository.
        /// </summary>
        private readonly IAuthorRepository authorRepository;

        /// <summary>
        /// Defines the bookRepository.
        /// </summary>
        private readonly IBookRepository bookRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookAuthorService"/> class.
        /// </summary>
        /// <param name="repository">The repository<see cref="IBookAuthorRepository"/>.</param>
        /// <param name="authorRepository">The authorRepository<see cref="IAuthorRepository"/>.</param>
        /// <param name="bookRepository">The bookRepository<see cref="IBookRepository"/>.</param>
        /// <param name="logger">The logger<see cref="ILogger"/>.</param>
        public BookAuthorService(IBookAuthorRepository repository, IAuthorRepository authorRepository, IBookRepository bookRepository, ILogger logger) : base(repository, new BookAuthorValidator(), logger)
        {
            this.authorRepository = authorRepository;
            this.bookRepository = bookRepository;
        }

        /// <summary>
        /// Add new book author method.
        /// </summary>
        /// <param name="bookAuthor">BookAuthor entity.</param>
        /// <returns>The Validation<see cref="ValidationResult"/>.</returns>
        public override ValidationResult Insert(BookAuthor bookAuthor)
        {
            try
            {
                var result = Validator.Validate(bookAuthor);

                if (!result.IsValid)
                {
                    throw new ArgumentException("Cannot add book author, invalid entity");
                }

                var bookAuthorExists = this.Repository.Get(i => i.BookId == bookAuthor.BookId && i.AuthorId == bookAuthor.AuthorId).Any();

                if (bookAuthorExists)
                {
                    throw new ArgumentException("Cannot add book author, book author already exists");
                }

                var bookExists = this.bookRepository.Get(i => i.Id == bookAuthor.BookId).Any();

                if (!bookExists)
                {
                    throw new ArgumentException("Cannot add book author, book does not exist");
                }

                var authorExists = this.authorRepository.Get(i => i.Id == bookAuthor.AuthorId).Any();

                if (!authorExists)
                {
                    throw new ArgumentException("Cannot add book author, author does not exist");
                }

                Repository.Insert(bookAuthor);

                return result;
            }
            catch (ArgumentException ex)
            {
                Logger.LogInformation(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// The Update.
        /// </summary>
        /// <param name="entity">The entity<see cref="BookAuthor"/>.</param>
        /// <returns>The <see cref="ValidationResult"/>.</returns>
        public override ValidationResult Update(BookAuthor entity)
        {
            throw new Exception("To delete and add new Book author is the best approach");
        }
    }
}
