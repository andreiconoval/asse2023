//------------------------------------------------------------------------------
// <copyright file="ReaderLoanService.cs" company="Transilvania University of Brasov">
// Copyright (c) Conoval. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
//------------------------------------------------------------------------------

namespace Library.BL.Services
{
    using System;
    using System.Linq;
    using FluentValidation;
    using Library.BL.Interfaces;
    using Library.BL.Validators;
    using Library.DAL.DomainModel;
    using Library.DAL.Interfaces;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Defines the <see cref="ReaderLoanService" />.
    /// </summary>
    public class ReaderLoanService : IReaderLoanService
    {
        /// <summary>
        /// Defines the bookLoanDetailRepository.
        /// </summary>
        private readonly IBookLoanDetailRepository bookLoanDetailRepository;

        /// <summary>
        /// Defines the readerLoanRepository.
        /// </summary>
        private readonly IReaderLoanRepository readerLoanRepository;

        /// <summary>
        /// Defines the bookSampleRepository.
        /// </summary>
        private readonly IBookSampleRepository bookSampleRepository;

        /// <summary>
        /// Defines the userRepository.
        /// </summary>
        private readonly IUserRepository userRepository;

        /// <summary>
        /// Defines the librarySettingsService.
        /// </summary>
        private readonly ILibrarySettingsService librarySettingsService;

        /// <summary>
        /// Defines the readerLoanValidator.
        /// </summary>
        private readonly IValidator<ReaderLoan> readerLoanValidator;

        /// <summary>
        /// Defines the bookLoanDetailValidator.
        /// </summary>
        private readonly IValidator<BookLoanDetail> bookLoanDetailValidator;

        /// <summary>
        /// Defines the logger.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReaderLoanService"/> class.
        /// </summary>
        /// <param name="readerLoanRepository">The readerLoanRepository<see cref="IReaderLoanRepository"/>.</param>
        /// <param name="bookLoanDetailRepository">The bookLoanDetailRepository<see cref="IBookLoanDetailRepository"/>.</param>
        /// <param name="bookSampleRepository">The bookSampleRepository<see cref="IBookSampleRepository"/>.</param>
        /// <param name="librarySettingsService">The librarySettingsService<see cref="ILibrarySettingsService"/>.</param>
        /// <param name="userRepository">The userRepository<see cref="IUserRepository"/>.</param>
        /// <param name="logger">The logger<see cref="ILogger"/>.</param>
        public ReaderLoanService(
            IReaderLoanRepository readerLoanRepository,
            IBookLoanDetailRepository bookLoanDetailRepository,
            IBookSampleRepository bookSampleRepository,
            ILibrarySettingsService librarySettingsService,
            IUserRepository userRepository,
            ILogger logger)
        {
            this.bookLoanDetailRepository = bookLoanDetailRepository;
            this.readerLoanRepository = readerLoanRepository;
            this.bookSampleRepository = bookSampleRepository;
            this.userRepository = userRepository;
            this.librarySettingsService = librarySettingsService;
            this.readerLoanValidator = new ReaderLoanValidator();
            this.bookLoanDetailValidator = new BookLoanDetailValidator();
            this.logger = logger;
        }

        /// <summary>
        /// Insert new reader loan.
        /// </summary>
        /// <param name="loan">New reader loan.</param>
        /// <returns>Reader loan with filled id.</returns>
        public ReaderLoan Insert(ReaderLoan loan)
        {
            try
            {
                loan.LoanDate = DateTime.Now;
                var result = this.readerLoanValidator.Validate(loan);

                if (!result.IsValid)
                {
                    this.logger.LogInformation($"Cannot add new loan, entity is invalid");
                    throw new ArgumentException("Cannot add new loan, entity is invalid");
                }

                var user = this.userRepository.Get(u => u.Id == loan.ReaderId, includeProperties: "LibraryStaff,Reader").FirstOrDefault();
                if (user == null || user.Reader == null)
                {
                    this.logger.LogInformation($"Cannot add new loan, reader is missing");
                    throw new ArgumentException("Cannot add new loan, reader is missing");
                }

                var staff = this.userRepository.Get(u => u.Id == loan.StaffId, includeProperties: "LibraryStaff").FirstOrDefault();
                if (staff == null || staff.LibraryStaff == null)
                {
                    this.logger.LogInformation($"Cannot add new loan, staff is missing");
                    throw new ArgumentException("Cannot add new loan, staff is missing");
                }

                if (loan.BookLoanDetails != null)
                {
                    foreach (var bookLoanDetails in loan.BookLoanDetails)
                    {
                        bookLoanDetails.LoanDate = DateTime.Now;
                        var resultBookLoanDetails = this.bookLoanDetailValidator.Validate(bookLoanDetails);
                        if (!resultBookLoanDetails.IsValid)
                        {
                            this.logger.LogInformation($"Cannot add new book loan detail, entity is invalid");
                            throw new ArgumentException("Cannot add new book loan detail, entity is invalid");
                        }

                        var bookSample = this.bookSampleRepository.Get(bs => bookLoanDetails.BookSampleId == bs.Id, includeProperties: "BookEdition,Book").FirstOrDefault();
                        if (bookSample == null)
                        {
                            this.logger.LogInformation($"Cannot add new book loan detail, bookSample is invalid");
                            throw new ArgumentException("Cannot add new book loan detail, bookSample is invalid");
                        }

                        bookLoanDetails.BookSample = bookSample;
                        bookLoanDetails.BookEditionId = bookSample.BookEditionId;
                        bookLoanDetails.BookId = bookSample?.BookEdition?.BookId;
                    }
                }

                var previousLoans = this.readerLoanRepository.Get(u => u.ReaderId == loan.ReaderId, includeProperties: "BookLoanDetails").ToList();

                var dateNow = DateTime.Now.Date;
                var staffLendCount = this.readerLoanRepository.Get(s => s.StaffId == loan.StaffId && s.LoanDate.Date == dateNow, includeProperties: "BookLoanDetails").Count();
                this.librarySettingsService.CheckIfUserCanBorrowBooks(user, loan, previousLoans, staffLendCount);

                this.readerLoanRepository.Insert(loan);

                foreach (var item in loan.BookLoanDetails)
                {
                    item.ReaderLoanId = loan.Id;
                    this.bookLoanDetailRepository.Insert(item);
                }

                return loan;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Mark reader loan as returned for all book.
        /// </summary>
        /// <param name="readerLoanId"> Reader loan id.</param>
        public void Delete(int readerLoanId)
        {
            var readerLoan = this.readerLoanRepository.Get(u => u.Id == readerLoanId, includeProperties: "BookLoanDetails").FirstOrDefault();
            if (readerLoan == null)
            {
                this.logger.LogInformation($"Cannot delete book loan, book loan is invalid");
                throw new ArgumentException("Cannot delete book loan, book loan is invalid");
            }

            foreach (var item in readerLoan.BookLoanDetails ?? Enumerable.Empty<BookLoanDetail>())
            {
                if (item.EffectiveReturnDate == null)
                {
                    item.EffectiveReturnDate = DateTime.Now;
                }

                this.bookLoanDetailRepository.Update(item);
            }
        }

        /// <summary>
        /// Extend Book loan details period from Now.
        /// </summary>
        /// <param name="bookLoanDetailId"> Book loan detail id.</param>
        public void SetExtensionsForLoan(int bookLoanDetailId)
        {
            var bookLoanDetail = this.bookLoanDetailRepository.Get(bld => bld.Id == bookLoanDetailId, includeProperties: "ReaderLoan").FirstOrDefault();
            if (bookLoanDetail == null)
            {
                this.logger.LogInformation($"Cannot update book loan detail, book loan is invalid");
                throw new ArgumentException("Cannot update book loan detail, book loan is invalid");
            }

            var previousLoans = this.readerLoanRepository.Get(u => bookLoanDetail.ReaderLoan != null && u.ReaderId == bookLoanDetail.ReaderLoan.ReaderId, includeProperties: "BookLoanDetails").ToList();

            // Verificare limita LIM pentru prelungiri
            var totalExtensionsInLastThreeMonths = previousLoans.Sum(pl => pl.ExtensionsGranted);
            if (totalExtensionsInLastThreeMonths + 1 > this.librarySettingsService.LIM)
            {
                throw new ArgumentException("Exceeded maximum allowed extensions for borrowed books in the last three months.");
            }

            bookLoanDetail.ExpectedReturnDate = DateTime.Now;

            if (bookLoanDetail.ReaderLoan != null)
            {
                bookLoanDetail.ReaderLoan.ExtensionsGranted += 1;
                this.readerLoanRepository.Update(bookLoanDetail.ReaderLoan);
            }

            this.bookLoanDetailRepository.Update(bookLoanDetail);
        }
    }
}
