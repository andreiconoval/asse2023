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
    using NUnit.Framework;

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
        private User User2 => new User() { };

        /// <summary>
        /// Gets the User.
        /// </summary>
        private User User1 => new User() { LibraryStaff = new LibraryStaff() };

        /// <summary>
        /// Gets the NewLoan.
        /// </summary>
        private ReaderLoan NewLoan => new ReaderLoan()
        {
            StaffId = 1,
            ReaderId = 1,
            LoanDate = new DateTime(2024, 1, 1),
            BorrowedBooks = 1,
            BookLoanDetails = new List<BookLoanDetail> { this.NewBookLoanDetail2, this.NewBookLoanDetail, this.NewBookLoanDetail }
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
        /// Gets the NewBookLoanDetail.
        /// </summary>
        private BookLoanDetail NewBookLoanDetail2 => new BookLoanDetail()
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
                            new BookDomain { DomainId = 2 }
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

        #region CheckIfUserCanBorrowBooks

        /// <summary>
        /// The CheckIfUserCanBorrowBooks_MaximumBooksBorrowed_UserId2_Test.
        /// </summary>
        [Test]
        public void CheckIfUserCanBorrowBooks_ExceededMaximumBooksBorrowed_UserId2_Test()
        {
            var librarySettings = this.GetLibrarySettings();
            librarySettings.MaxBookBorrowed = 1;
            this.SetLibrarySettings(librarySettings);

            var previousLoans = this.PreviousLoans;
            previousLoans.Add(previousLoans.First());

            var ex = Assert.Throws<ArgumentException>(() => this.service.CheckIfUserCanBorrowBooks(User2, this.NewLoan, previousLoans, this.StaffLendCount));
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

            var ex = Assert.Throws<ArgumentException>(() => this.service.CheckIfUserCanBorrowBooks(User1, this.NewLoan, this.PreviousLoans, this.StaffLendCount));
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
            var librarySettings = this.GetLibrarySettings();
            librarySettings.MaxBooksBorrowedPerTime = 1;
            this.SetLibrarySettings(librarySettings);

            var ex = Assert.Throws<ArgumentException>(() => this.service.CheckIfUserCanBorrowBooks(User2, this.NewLoan, this.PreviousLoans, this.StaffLendCount));
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
            var librarySettings = this.GetLibrarySettings();
            librarySettings.MaxBookBorrowed = 3;
            librarySettings.MaxBooksBorrowedPerTime = 1;
            this.SetLibrarySettings(librarySettings);

            var ex = Assert.Throws<ArgumentException>(() => this.service.CheckIfUserCanBorrowBooks(User1, this.NewLoan, this.PreviousLoans, this.StaffLendCount));
            Assert.That(ex, Is.Not.Null);
            Assert.That(ex.Message, Is.EqualTo("Exceeded maximum books borrowed per time."));
            Assert.Pass();
        }


        /// <summary>
        /// The CheckIfUserCanBorrowBooks_NotDistinctCategories_Test.
        /// The CheckIfUserCanBorrowBooks_NotDistinctCategories_Test doesn't depend on UserInd
        /// </summary>
        [Test]
        public void CheckIfUserCanBorrowBooks_NotDistinctCategories_Test()
        {
            this.SetLibrarySettings(this.GetLibrarySettings());

            var newLoan = this.NewLoan;
            newLoan.BookLoanDetails.First().BookSample.BookEdition.Book.BookDomains.First().DomainId = 1;

            var ex = Assert.Throws<ArgumentException>(() => this.service.CheckIfUserCanBorrowBooks(User2, newLoan, this.PreviousLoans, this.StaffLendCount));
            Assert.That(ex, Is.Not.Null);
            Assert.That(ex.Message, Is.EqualTo("At least 2 distinct categories are required for borrowing 3 or more books."));
            Assert.Pass();
        }

        /// <summary>
        /// The CheckIfUserCanBorrowBooks_ExceededBooksForTheSameCategory_UserInd2_Test.
        /// </summary>
        [Test]
        public void CheckIfUserCanBorrowBooks_ExceededBooksForTheSameCategory_UserInd2_Test()
        {
            var librarySettings = this.GetLibrarySettings();
            librarySettings.MaxAllowedBooksPerDomain = 1;
            this.SetLibrarySettings(librarySettings);

            var ex = Assert.Throws<ArgumentException>(() => this.service.CheckIfUserCanBorrowBooks(User2, NewLoan, this.PreviousLoans, this.StaffLendCount));
            Assert.That(ex, Is.Not.Null);
            Assert.That(ex.Message, Is.EqualTo("Exceeded maximum allowed books from the same domain in the specified period."));
            Assert.Pass();
        }

        /// <summary>
        /// The CheckIfUserCanBorrowBooks_ExceededBooksForTheSameCategory_UserInd1_Test.
        /// </summary>
        [Test]
        public void CheckIfUserCanBorrowBooks_ExceededBooksForTheSameCategory_UserInd1_Test()
        {
            var librarySettings = this.GetLibrarySettings();
            librarySettings.MaxAllowedBooksPerDomain = 2;
            librarySettings.MaxBookBorrowed = 4;
            this.SetLibrarySettings(librarySettings);

            var ex = Assert.Throws<ArgumentException>(() => this.service.CheckIfUserCanBorrowBooks(User1, NewLoan, this.PreviousLoans, this.StaffLendCount));
            Assert.That(ex, Is.Not.Null);
            Assert.That(ex.Message, Is.EqualTo("Exceeded maximum allowed books from the same domain in the specified period."));
            Assert.Pass();
        }

        /// <summary>
        /// The CheckIfUserCanBorrowBooks_MaxAllowedBooksPerDomain_UserInd2_Test.
        /// </summary>
        [Test]
        public void CheckIfUserCanBorrowBooks_MaxAllowedBooksPerDomain_UserInd2_Test()
        {
            var librarySettings = this.GetLibrarySettings();
            this.SetLibrarySettings(librarySettings);

            var previousLoans = this.PreviousLoans;
            previousLoans.First().BookLoanDetails.First().LoanDate = DateTime.Now;

            var ex = Assert.Throws<ArgumentException>(() => this.service.CheckIfUserCanBorrowBooks(User2, NewLoan, previousLoans, this.StaffLendCount));
            Assert.That(ex, Is.Not.Null);
            Assert.That(ex.Message, Is.EqualTo("Cannot borrow the same book within the specified interval."));
            Assert.Pass();
        }

        /// <summary>
        /// The CheckIfUserCanBorrowBooks_MaxAllowedBooksPerDomain_UserInd1_Test.
        /// </summary>
        [Test]
        public void CheckIfUserCanBorrowBooks_MaxAllowedBooksPerDomain_UserInd1_Test()
        {
            var librarySettings = this.GetLibrarySettings();
            librarySettings.MaxBookBorrowed = 4;
            this.SetLibrarySettings(librarySettings);

            var previousLoans = this.PreviousLoans;
            previousLoans.First().BookLoanDetails.First().LoanDate = DateTime.Now;

            var ex = Assert.Throws<ArgumentException>(() => this.service.CheckIfUserCanBorrowBooks(User1, NewLoan, previousLoans, this.StaffLendCount));
            Assert.That(ex, Is.Not.Null);
            Assert.That(ex.Message, Is.EqualTo("Cannot borrow the same book within the specified interval."));
            Assert.Pass();
        }


        /// <summary>
        /// The CheckIfUserCanBorrowBooks_LimitBookLend_Test.
        /// The CheckIfUserCanBorrowBooks_LimitBookLend_Test doesn't depend on UserInd.
        /// </summary>
        [Test]
        public void CheckIfUserCanBorrowBooks_LimitBookLend_Test()
        {
            this.SetLibrarySettings(this.GetLibrarySettings());

            var ex = Assert.Throws<ArgumentException>(() => this.service.CheckIfUserCanBorrowBooks(User2, NewLoan, this.PreviousLoans, 11));
            Assert.That(ex, Is.Not.Null);
            Assert.That(ex.Message, Is.EqualTo("Exceeded maximum books that library staff can lend in a day."));
            Assert.Pass();
        }

        /// <summary>
        /// The CheckIfUserCanBorrowBooks_MaxBorrowedBooksPerDay_UserInd2_Test.
        /// </summary>
        [Test]
        public void CheckIfUserCanBorrowBooks_MaxBorrowedBooksPerDay_UserInd2_Test()
        {
            var librarySettings = this.GetLibrarySettings();
            librarySettings.MaxBorrowedBooksPerDay = 2;
            this.SetLibrarySettings(librarySettings);

            var ex = Assert.Throws<ArgumentException>(() => this.service.CheckIfUserCanBorrowBooks(User2, NewLoan, this.PreviousLoans, this.StaffLendCount));
            Assert.That(ex, Is.Not.Null);
            Assert.That(ex.Message, Is.EqualTo("Reader exceed limit for today")); //3>2
            Assert.Pass();
        }

        /// <summary>
        /// The CheckIfUserCanBorrowBooks_MaxBorrowedBooksPerDay_Test.
        /// </summary>
        [Test]
        public void CheckIfUserCanBorrowBooks_MaxBorrowedBooksPerDay_PreviousLoan_UserInd2_Test()
        {
            var librarySettings = this.GetLibrarySettings();
            librarySettings.MaxBorrowedBooksPerDay = 4;
            this.SetLibrarySettings(librarySettings);

            var previousLoans = this.PreviousLoans;
            previousLoans.First().LoanDate = this.NewLoan.LoanDate;
            previousLoans.Last().LoanDate = this.NewLoan.LoanDate;

            var ex = Assert.Throws<ArgumentException>(() => this.service.CheckIfUserCanBorrowBooks(this.User2, this.NewLoan, previousLoans, this.StaffLendCount));
            Assert.That(ex, Is.Not.Null);
            Assert.That(ex.Message, Is.EqualTo("Reader exceed limit for today")); //3+2>4
            Assert.Pass();
        }

        /// <summary>
        /// The CheckIfUserCanBorrowBooks_Success_NoLimitsBorrowedBooksPerDay_Test.
        /// </summary>
        [Test]
        public void CheckIfUserCanBorrowBooks_Success_NoLimitsBorrowedBooksPerDay_Test()
        {
            var librarySettings = this.GetLibrarySettings();
            librarySettings.MaxBookBorrowed = 4;
            librarySettings.SameBookRepeatBorrowingLimit = 4;
            this.SetLibrarySettings(librarySettings);

            this.service.CheckIfUserCanBorrowBooks(User1, NewLoan, this.PreviousLoans, this.StaffLendCount);
            Assert.Pass();
        }

        /// <summary>
        /// The CheckIfUserCanBorrowBooks_Success_UserInd2_Test.
        /// </summary>
        [Test]
        public void CheckIfUserCanBorrowBooks_Success_UserInd2_Test()
        {
            var librarySettings = this.GetLibrarySettings();
            librarySettings.MaxBookBorrowed = 4;
            this.SetLibrarySettings(librarySettings);

            this.service.CheckIfUserCanBorrowBooks(User2, NewLoan, this.PreviousLoans, this.StaffLendCount);
            Assert.Pass();
        }

        #endregion


        #region CheckIfUserCanExtendForLoan

        /// <summary>
        /// The CheckIfUserCanExtendForLoan_UserInd2_False_Test.
        /// </summary>
        [Test]
        public void CheckIfUserCanExtendForLoan_UserInd2_False_Test()
        {
            var librarySettings = this.GetLibrarySettings();
            librarySettings.BorrowedBooksExtensionLimit = 4;
            this.SetLibrarySettings(librarySettings);

            var result = this.service.CheckIfUserCanExtendForLoan(User2, 9);
            Assert.That(result, Is.False);//9>4*2
            Assert.Pass();
        }

        /// <summary>
        /// The CheckIfUserCanExtendForLoan_UserInd1_False_Test.
        /// </summary>
        [Test]
        public void CheckIfUserCanExtendForLoan_UserInd1_False_Test()
        {
            var librarySettings = this.GetLibrarySettings();
            librarySettings.BorrowedBooksExtensionLimit = 4;
            this.SetLibrarySettings(librarySettings);

            var result = this.service.CheckIfUserCanExtendForLoan(User1, 5);
            Assert.That(result, Is.False);//5>4*1
            Assert.Pass();
        }

        /// <summary>
        /// The CheckIfUserCanExtendForLoan_UserInd2_True_Test.
        /// </summary>
        [Test]
        public void CheckIfUserCanExtendForLoan_UserInd2_True_Test()
        {
            var librarySettings = this.GetLibrarySettings();
            librarySettings.BorrowedBooksExtensionLimit = 4;
            this.SetLibrarySettings(librarySettings);

            var result = this.service.CheckIfUserCanExtendForLoan(User2, 5); 
            Assert.That(result, Is.True);//5<4*2
            Assert.Pass();
        }

        /// <summary>
        /// The CheckIfUserCanExtendForLoan_UserInd1_True_Test.
        /// </summary>
        [Test]
        public void CheckIfUserCanExtendForLoan_UserInd1_True_Test()
        {
            var librarySettings = this.GetLibrarySettings();
            librarySettings.BorrowedBooksExtensionLimit = 4;
            this.SetLibrarySettings(librarySettings);

            var result = this.service.CheckIfUserCanExtendForLoan(User1, 3);
            Assert.That(result, Is.True);//3<4*1
            Assert.Pass();
        }


        #endregion


        /// <summary>
        /// The this.GetLibrarySettings.
        /// </summary>
        /// <returns>The <see cref="LibrarySettings"/>.</returns>
        private LibrarySettings GetLibrarySettings()
        {
            return new LibrarySettings()
            {
                MaxBookBorrowed = 2,
                MaxBorrowedBooksPerDay = 4,
                BorrowedBooksPeriod = 10,
                MaxBooksBorrowedPerTime = 10,
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
