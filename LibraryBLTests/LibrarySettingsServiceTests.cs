//------------------------------------------------------------------------------
// <copyright file="LibrarySettingsServiceTests.cs" company="Transilvania University of Brasov">
// Copyright (c) Conoval. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
//------------------------------------------------------------------------------

namespace LibraryBLTests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Library.BL.Interfaces;
    using Library.BL.Services;
    using Library.DAL.DomainModel;
    using Library.DAL.Interfaces;
    using Moq;

    /// <summary>
    /// Defines the <see cref="LibrarySettingsServiceTests" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class LibrarySettingsServiceTests
    {
        /// <summary>
        /// Defines the service.
        /// </summary>
        private ILibrarySettingsService service;

        /// <summary>
        /// Defines the repositoryMock.
        /// </summary>
        private Mock<ILibrarySettingsRepository> repositoryMock;

        /// <summary>
        /// Initializes a new instance of the <see cref="LibrarySettingsServiceTests"/> class.
        /// </summary>
        public LibrarySettingsServiceTests()
        {
        }

        /// <summary>
        /// Gets the User.
        /// </summary>
        private User User => new User() { };

        /// <summary>
        /// Gets the NewLoan.
        /// </summary>
        private ReaderLoan NewLoan => new ReaderLoan()
        {
            StaffId = 1,
            ReaderId = 1,
            LoanDate = new DateTime(),
            BorrowedBooks = 1,
            BookLoanDetails = new List<BookLoanDetail> { this.NewBookLoanDetail, this.NewBookLoanDetail, this.NewBookLoanDetail }
        };

        /// <summary>
        /// Gets the this.PreviousLoans.
        /// </summary>
        private List<ReaderLoan> PreviousLoans => new List<ReaderLoan>()
            {
                new ReaderLoan()
                {
                    LoanDate = DateTime.Now.AddDays(-4),
                    BookLoanDetails = new List<BookLoanDetail> { this.NewBookLoanDetail }
                }, 
                new ReaderLoan()
                {
                    LoanDate = DateTime.Now.AddDays(-4),
                    BookLoanDetails = new List<BookLoanDetail> { this.NewBookLoanDetail }
                }
            };

        /// <summary>
        /// Gets the StaffLendCount.
        /// </summary>
        private int StaffLendCount => 2;

        /// <summary>
        /// Gets the NewBookLoanDetail.
        /// </summary>
        private BookLoanDetail NewBookLoanDetail => new BookLoanDetail()
        {
            Id = 1,
            BookSampleId = 101,
            BookEditionId = 201,
            BookId = 301,
            ReaderLoanId = 401,
            LoanDate = DateTime.Now.AddDays(-6),
            ExpectedReturnDate = DateTime.Now.AddDays(14),
            EffectiveReturnDate = null,
            BookSample = new BookSample()
            {
                BookEdition = new BookEdition()
                {
                    Book = new Book()
                    {
                        BookDomains = new List<BookDomain>()
                        {
                            new BookDomain { DomainId = 1 }
                        }
                    }
                }
            }
        };

        /// <summary>
        /// The Setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.repositoryMock = new Mock<ILibrarySettingsRepository>();
            this.service = new LibrarySettingsService(this.repositoryMock.Object);
        }

        /// <summary>
        /// The Constructor_Test.
        /// </summary>
        [Test]
        public void Constructor_Test()
        {
            Assert.That(this.service, Is.Not.Null);
            Assert.Pass();
        }

        /// <summary>
        /// The Constructor_ArgumentNullException_Test.
        /// </summary>
        [Test]
        public void Constructor_ArgumentNullException_Test()
        {
            Assert.Throws<ArgumentNullException>(() => new LibrarySettingsService(null));
            Assert.Pass();
        }

        /// <summary>
        /// The CheckIfUserCanBorrowBooks_MaximumBooksBorrowed_UserId2_Test.
        /// </summary>
        [Test]
        public void CheckIfUserCanBorrowBooks_MaximumBooksBorrowed_UserId2_Test()
        {
            var librarySettings = this.GetLibrarySettings();
            librarySettings.MaxBookBorrowed = 1;
            this.SetLibrarySettings(librarySettings);

            var previousLoans = this.PreviousLoans;
            previousLoans.Add(previousLoans.First());

            var ex = Assert.Throws<ArgumentException>(() => this.service.CheckIfUserCanBorrowBooks(User, this.NewLoan, previousLoans, this.StaffLendCount));
            Assert.That(ex, Is.Not.Null);
            Assert.That(ex.Message, Is.EqualTo("Exceeded maximum books borrowed in the specified period."));
            Assert.Pass();
        }

        /// <summary>
        /// The CheckIfUserCanBorrowBooks_MaximumBooksBorrowed_UserId1_Test.
        /// </summary>
        [Test]
        public void CheckIfUserCanBorrowBooks_MaximumBooksBorrowed_UserId1_Test()
        {
            this.SetLibrarySettings(this.GetLibrarySettings());

            var user = User;
            user.LibraryStaff = new LibraryStaff();

            var ex = Assert.Throws<ArgumentException>(() => this.service.CheckIfUserCanBorrowBooks(user, this.NewLoan, this.PreviousLoans, this.StaffLendCount));
            Assert.That(ex, Is.Not.Null);
            Assert.That(ex.Message, Is.EqualTo("Exceeded maximum books borrowed in the specified period."));
            Assert.Pass();
        }

        /// <summary>
        /// The CheckIfUserCanBorrowBooks_MaximumBooksBorrowedPerTime_UserId2_Test.
        /// </summary>
        [Test]
        public void CheckIfUserCanBorrowBooks_MaximumBooksBorrowedPerTime_UserId2_Test()
        {
            this.SetLibrarySettings(this.GetLibrarySettings());

            var ex = Assert.Throws<ArgumentException>(() => this.service.CheckIfUserCanBorrowBooks(User, this.NewLoan, this.PreviousLoans, this.StaffLendCount));
            Assert.That(ex, Is.Not.Null);
            Assert.That(ex.Message, Is.EqualTo("Exceeded maximum books borrowed per time."));
            Assert.Pass();
        }

        /// <summary>
        /// The CheckIfUserCanBorrowBooks_MaximumBooksBorrowedPerTime_UserId1_Test.
        /// </summary>
        [Test]
        public void CheckIfUserCanBorrowBooks_MaximumBooksBorrowedPerTime_UserId1_Test()
        {
            this.SetLibrarySettings(this.GetLibrarySettings());

            var user = User;
            user.LibraryStaff = new LibraryStaff();

            var ex = Assert.Throws<ArgumentException>(() => this.service.CheckIfUserCanBorrowBooks(User, this.NewLoan, this.PreviousLoans, this.StaffLendCount));
            Assert.That(ex, Is.Not.Null);
            Assert.That(ex.Message, Is.EqualTo("Exceeded maximum books borrowed per time."));
            Assert.Pass();
        }

        /// <summary>
        /// The CheckIfUserCanBorrowBooks_NotDistinctCategories_Test.
        /// </summary>
        [Test]
        public void CheckIfUserCanBorrowBooks_NotDistinctCategories_Test()
        {
            var librarySettings = this.GetLibrarySettings();
            librarySettings.MaxBooksBorrowedPerTime = 10;
            this.SetLibrarySettings(librarySettings);

            var ex = Assert.Throws<ArgumentException>(() => this.service.CheckIfUserCanBorrowBooks(User, this.NewLoan, this.PreviousLoans, this.StaffLendCount));
            Assert.That(ex, Is.Not.Null);
            Assert.That(ex.Message, Is.EqualTo("At least 2 distinct categories are required for borrowing 3 or more books."));
            Assert.Pass();
        }

        /// <summary>
        /// The CheckIfUserCanBorrowBooks_DistinctCategories_Test.
        /// </summary>
        [Test]
        public void CheckIfUserCanBorrowBooks_DistinctCategories_Test()
        {
            var librarySettings = this.GetLibrarySettings();
            librarySettings.MaxBooksBorrowedPerTime = 10;
            librarySettings.MaxAllowedBooksPerDomain = 1;
            this.SetLibrarySettings(librarySettings);

            var newLoan = this.NewLoan;
            newLoan.BookLoanDetails.First().BookSample.BookEdition.Book.BookDomains.First().DomainId = 2;

            var ex = Assert.Throws<ArgumentException>(() => this.service.CheckIfUserCanBorrowBooks(User, newLoan, this.PreviousLoans, this.StaffLendCount));
            Assert.That(ex, Is.Not.Null);
            Assert.That(ex.Message, Is.EqualTo("Exceeded maximum allowed books from the same domain in the specified period."));
            Assert.Pass();
        }

        /// <summary>
        /// The CheckIfUserCanBorrowBooks_MaxAllowedBooksPerDomain_Test.
        /// </summary>
        [Test]
        public void CheckIfUserCanBorrowBooks_MaxAllowedBooksPerDomain_Test()
        {
            var librarySettings = this.GetLibrarySettings();
            librarySettings.MaxBooksBorrowedPerTime = 10;
            this.SetLibrarySettings(librarySettings);

            var newLoan = this.NewLoan;
            newLoan.BookLoanDetails.First().BookSample.BookEdition.Book.BookDomains.First().DomainId = 2;

            var previousLoans = this.PreviousLoans;
            previousLoans.First().BookLoanDetails.First().LoanDate = DateTime.Now;

            var ex = Assert.Throws<ArgumentException>(() => this.service.CheckIfUserCanBorrowBooks(User, newLoan, previousLoans, this.StaffLendCount));
            Assert.That(ex, Is.Not.Null);
            Assert.That(ex.Message, Is.EqualTo("Cannot borrow the same book within the specified interval."));
            Assert.Pass();
        }

        /// <summary>
        /// The CheckIfUserCanBorrowBooks_LimitBookLend_Test.
        /// </summary>
        [Test]
        public void CheckIfUserCanBorrowBooks_LimitBookLend_Test()
        {
            var librarySettings = this.GetLibrarySettings();
            librarySettings.MaxBooksBorrowedPerTime = 10;
            this.SetLibrarySettings(librarySettings);

            var newLoan = this.NewLoan;
            newLoan.BookLoanDetails.First().BookSample.BookEdition.Book.BookDomains.First().DomainId = 2;

            var ex = Assert.Throws<ArgumentException>(() => this.service.CheckIfUserCanBorrowBooks(User, newLoan, this.PreviousLoans, 11));
            Assert.That(ex, Is.Not.Null);
            Assert.That(ex.Message, Is.EqualTo("Exceeded maximum books that library staff can lend in a day."));
            Assert.Pass();
        }

        /// <summary>
        /// The CheckIfUserCanBorrowBooks_MaxBorrowedBooksPerDay_Test.
        /// </summary>
        [Test]
        public void CheckIfUserCanBorrowBooks_MaxBorrowedBooksPerDay_Test()
        {
            var librarySettings = this.GetLibrarySettings();
            librarySettings.MaxBooksBorrowedPerTime = 10;
            this.SetLibrarySettings(librarySettings);

            var newLoan = this.NewLoan;
            newLoan.BookLoanDetails.First().BookSample.BookEdition.Book.BookDomains.First().DomainId = 2;

            var ex = Assert.Throws<ArgumentException>(() => this.service.CheckIfUserCanBorrowBooks(User, newLoan, this.PreviousLoans, this.StaffLendCount));
            Assert.That(ex, Is.Not.Null);
            Assert.That(ex.Message, Is.EqualTo("Reader exceed limit for today"));
            Assert.Pass();
        }

        /// <summary>
        /// The CheckIfUserCanBorrowBooks_Success_Test.
        /// </summary>
        [Test]
        public void CheckIfUserCanBorrowBooks_Success_Test()
        {
            var librarySettings = this.GetLibrarySettings();
            librarySettings.MaxBooksBorrowedPerTime = 10;
            librarySettings.SameBookRepeatBorrowingLimit = 5;
            librarySettings.MaxBookBorrowed = 4;
            this.SetLibrarySettings(librarySettings);

            var user = User;
            user.LibraryStaff = new LibraryStaff();

            var newLoan = this.NewLoan;
            newLoan.BookLoanDetails.First().BookSample.BookEdition.Book.BookDomains.First().DomainId = 2;

            this.service.CheckIfUserCanBorrowBooks(user, newLoan, this.PreviousLoans, this.StaffLendCount);
            Assert.Pass();
        }

        /// <summary>
        /// The this.GetLibrarySettings.
        /// </summary>
        /// <returns>The <see cref="LibrarySettings"/>.</returns>
        private LibrarySettings GetLibrarySettings()
        {
            return new LibrarySettings()
            {
                MaxBookBorrowed = 2,
                BorrowedBooksPeriod = 10,
                MaxBooksBorrowedPerTime = 1,
                MaxAllowedBooksPerDomain = 3,
                SameBookRepeatBorrowingLimit = 10,
                LimitBookLend = 10,
            };
        }

        /// <summary>
        /// The this.SetLibrarySettings.
        /// </summary>
        /// <param name="librarySettings">The librarySettings<see cref="LibrarySettings"/>.</param>
        private void SetLibrarySettings(LibrarySettings librarySettings)
        {
            this.service.LibrarySettings = librarySettings;
        }
    }
}
