//------------------------------------------------------------------------------
// <copyright file="LibrarySettingsService.cs" company="Transilvania University of Brasov">
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
    using Library.DAL.DomainModel;
    using Library.DAL.Interfaces;

    /// <summary>
    /// Defines the <see cref="LibrarySettingsService" />.
    /// </summary>
    public class LibrarySettingsService : ILibrarySettingsService
    {
        /// <summary>
        /// Defines the _librarySettingsRepository.
        /// </summary>
        private readonly ILibrarySettingsRepository librarySettingsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="LibrarySettingsService"/> class.
        /// </summary>
        /// <param name="librarySettingsRepository">The librarySettingsRepository<see cref="ILibrarySettingsRepository"/>.</param>
        public LibrarySettingsService(ILibrarySettingsRepository librarySettingsRepository)
        {
            this.librarySettingsRepository = librarySettingsRepository ?? throw new ArgumentNullException();
            LibrarySettings = this.librarySettingsRepository.Get();
        }

        /// <summary>
        /// Gets the DOMENII.
        /// </summary>
        public int DOMENII { get => LibrarySettings.MaxDomains; }

        /// <summary>
        /// Gets the NMC.
        /// </summary>
        public int NMC { get => LibrarySettings.MaxBookBorrowed * this.USER_IND; }

        /// <summary>
        /// Gets the PER.
        /// </summary>
        public int PER { get => LibrarySettings.BorrowedBooksPeriod / this.USER_IND; }

        /// <summary>
        /// Gets the C.
        /// </summary>
        public int C { get => LibrarySettings.MaxBooksBorrowedPerTime * this.USER_IND; }

        /// <summary>
        /// Gets the D.
        /// </summary>
        public int D { get => LibrarySettings.MaxAllowedBooksPerDomain * this.USER_IND; }

        /// <summary>
        /// Gets the L.
        /// </summary>
        public int L { get => LibrarySettings.AllowedMonthsForSameDomain; }

        /// <summary>
        /// Gets the LIM.
        /// </summary>
        public int LIM { get => LibrarySettings.BorrowedBooksExtensionLimit * this.USER_IND; }

        /// <summary>
        /// Gets the DELTA.
        /// </summary>
        public int DELTA { get => LibrarySettings.SameBookRepeatBorrowingLimit / this.USER_IND; }

        /// <summary>
        /// Gets the NCZ.
        /// </summary>
        public int NCZ { get => LibrarySettings.MaxBorrowedBooksPerDay; }

        /// <summary>
        /// Gets the PERSIMP
        /// LimitBookLend.
        /// </summary>
        public int PERSIMP { get => LibrarySettings.LimitBookLend; }

        /// <summary>
        /// Gets or sets the USER_IND
        /// User index if is staff should be 2 if reader 1.
        /// </summary>
        public int USER_IND { get; set; } = 1;

        /// <summary>
        /// Gets or sets the LibrarySettings.
        /// </summary>
        public LibrarySettings LibrarySettings { get; set; }

        /// <summary>
        /// Check base on library settings is user can borrow books or not.
        /// </summary>
        /// <param name="user">User that with borrow.</param>
        /// <param name="newLoan">New reader loan.</param>
        /// <param name="previousLoans">Previous user loans.</param>
        /// <param name="staffLendCount">Staff lend count for today.</param>
        public void CheckIfUserCanBorrowBooks(User user, ReaderLoan newLoan, List<ReaderLoan> previousLoans, int staffLendCount)
        {
            var distinctCategoriesCount = newLoan.BookLoanDetails.SelectMany(bld => bld.BookSample.BookEdition.Book.BookDomains.Select(bd => bd.DomainId)).Distinct().Count();
            if (distinctCategoriesCount < 2)
            {
                this.USER_IND = 2;
            }
            else
            {
                this.USER_IND = 1;
            }
            
            // Implementare pentru cerinte si limitari

            // Verificare limita NMC in perioada PER
            var loansInPeriod = previousLoans.Count(pl => (DateTime.Now - pl.LoanDate).TotalDays <= this.PER);
            if (loansInPeriod + 1 > this.NMC)
            {
                throw new ArgumentException("Exceeded maximum books borrowed in the specified period.");
            }

            // Verificare limita C
            if (newLoan?.BookLoanDetails?.Count() > this.C)
            {
                throw new ArgumentException("Exceeded maximum books borrowed per time.");
            }

            // Verificare categorii distincte daca sunt cel putin 3 carti imprumutate
            if (newLoan?.BookLoanDetails?.Count() >= 3)
            {
                var distinctCategoriesCount = newLoan.BookLoanDetails.Select(bld => bld.BookSample?.BookEdition?.Book?.BookDomains?.Select(bd => bd.DomainId)).Distinct().Count();
                if (distinctCategoriesCount < 2)
                {
                    throw new ArgumentException("At least 2 distinct categories are required for borrowing 3 or more books.");
                }
            }

            // Verificare limita D pentru carti din acelasi domeniu in ultimele L luni
            var currentDate = DateTime.Now;
            var lastMonthsDate = currentDate.AddMonths(-this.L);
            var recentBorrowedBooks = previousLoans.SelectMany(pl => pl.BookLoanDetails ?? Enumerable.Empty<BookLoanDetail>())
                                                  .Where(bld => bld.LoanDate >= lastMonthsDate && bld.LoanDate <= currentDate)
                                                  .ToList();

            var borrowedBooksFromSameDomain = recentBorrowedBooks.Count(bld =>
                            bld?.BookSample?.BookEdition?.Book?.BookDomains?.Any(bd =>
                            bd.DomainId == newLoan?.BookLoanDetails?.First().BookSample?.BookEdition?.Book?.BookDomains?.First().DomainId) ?? false);
            if (borrowedBooksFromSameDomain + newLoan?.BookLoanDetails?.Count() > this.D)
            {
                throw new ArgumentException("Exceeded maximum allowed books from the same domain in the specified period.");
            }

            // Verificare limita DELTA pentru aceeasi carte
            var lastBorrowedBook = previousLoans.SelectMany(pl => pl?.BookLoanDetails ?? Enumerable.Empty<BookLoanDetail>()).OrderByDescending(bld => bld.LoanDate).FirstOrDefault();
            if (lastBorrowedBook != null && (DateTime.Now - lastBorrowedBook.LoanDate).Days < this.DELTA)
            {
                throw new ArgumentException("Cannot borrow the same book within the specified interval.");
            }

            // Verificare limita pentru personalul bibliotecii
            if (staffLendCount > this.PERSIMP)
            {
                throw new ArgumentException("Exceeded maximum books that library staff can lend in a day.");
            }

            var loansForNewLoanDay = previousLoans.Where(pl => pl.LoanDate.Date == newLoan?.LoanDate.Date).SelectMany(pl => pl?.BookLoanDetails ?? Enumerable.Empty<BookLoanDetail>()).Count();

            if (user.LibraryStaff == null &&
                (newLoan?.BookLoanDetails?.Count() > this.NCZ || loansForNewLoanDay + newLoan?.BookLoanDetails?.Count() > this.NCZ))
            {
                throw new ArgumentException("Reader exceed limit for today");
            }
        }
    }
}
