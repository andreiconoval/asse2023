//------------------------------------------------------------------------------
// <copyright file="BookSampleService.cs" company="Transilvania University of Brasov">
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
    /// Defines the <see cref="BookSampleService" />.
    /// </summary>
    public class BookSampleService : BaseService<BookSample, IBookSampleRepository>, IBookSampleService
    {
        /// <summary>
        /// Defines the _bookEditionRepository.
        /// </summary>
        private readonly IBookEditionRepository bookEditionRepository;

        /// <summary>
        /// Defines the _bookLoanDetailRepository.
        /// </summary>
        private readonly IBookLoanDetailRepository bookLoanDetailRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookSampleService"/> class.
        /// </summary>
        /// <param name="repository">The repository<see cref="IBookSampleRepository"/>.</param>
        /// <param name="bookEditionRepository">The bookEditionRepository<see cref="IBookEditionRepository"/>.</param>
        /// <param name="bookLoanDetailRepository">The bookLoanDetailRepository<see cref="IBookLoanDetailRepository"/>.</param>
        /// <param name="logger">The logger<see cref="ILogger"/>.</param>
        public BookSampleService(
            IBookSampleRepository repository,
            IBookEditionRepository bookEditionRepository,
            IBookLoanDetailRepository bookLoanDetailRepository,
            ILogger logger)
            : base(repository, new BookSampleValidator(), logger)
        {
            this.bookEditionRepository = bookEditionRepository;
            this.bookLoanDetailRepository = bookLoanDetailRepository;
        }

        /// <summary>
        /// Insert new book sample.
        /// </summary>
        /// <param name="bookSample">Book sample.</param>
        /// <returns>Validation result.</returns>
        public override ValidationResult Insert(BookSample bookSample)
        {
            try
            {
                var result = Validator.Validate(bookSample);

                if (bookSample == null || !result.IsValid)
                {
                    throw new ArgumentException("Cannot add book sample, invalid entity");
                }

                var bookEditionExists = this.bookEditionRepository.Get(i => i.Id == bookSample.BookEditionId).Any();

                if (!bookEditionExists)
                {
                    throw new ArgumentException("Cannot add book sample, book edition do not exists");
                }

                Repository.Insert(bookSample);

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
        /// Method to update existing book sample.
        /// </summary>
        /// <param name="bookSample">Book sample.</param>
        /// <returns>Validation result.</returns>
        public override ValidationResult Update(BookSample bookSample)
        {
            try
            {
                var result = Validator.Validate(bookSample);

                if (bookSample == null || !result.IsValid)
                {
                    throw new ArgumentException("Cannot update book sample, invalid entity");
                }

                var bookSampleExists = Repository.Get(i => i.Id == bookSample.Id).Any();

                if (!bookSampleExists)
                {
                    throw new ArgumentException("Cannot update book sample, book sample do not exists");
                }

                var bookEditionExists = this.bookEditionRepository.Get(i => i.Id == bookSample.BookEditionId).Any();

                if (!bookEditionExists)
                {
                    throw new ArgumentException("Cannot add book sample, book edition do not exists");
                }

                Repository.Update(bookSample);

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
        /// Deleting book sample, cannot be deleted if book sample has active book loan.
        /// </summary>
        /// <param name="bookSample">Book sample to delete.</param>
        public override void Delete(BookSample bookSample)
        {
            if (bookSample is null || bookSample.Id == 0)
            {
                Logger.LogInformation("Cannot delete book sample, invalid entity");
                throw new ArgumentException("Cannot delete book sample, invalid entity");
            }

            var hasActiveBookLoan = this.bookLoanDetailRepository.Get(bld => bld.BookSampleId == bookSample.Id && bld.EffectiveReturnDate != null).Any();
            if (hasActiveBookLoan)
            {
                Logger.LogInformation("Cannot delete book sample, book is in use");
                throw new ArgumentException("Cannot delete book sample, book is in use");
            }

            base.Delete(bookSample);
        }
    }
}
