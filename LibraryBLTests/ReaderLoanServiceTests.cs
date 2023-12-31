﻿//------------------------------------------------------------------------------
// <copyright file="ReaderLoanServiceTests.cs" company="Transilvania University of Brasov">
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
    using System.Linq.Expressions;
    using FluentValidation;
    using Library.BL.Infrastructure;
    using Library.BL.Interfaces;
    using Library.BL.Services;
    using Library.DAL.DomainModel;
    using Library.DAL.Interfaces;
    using Moq;
    using NUnit.Framework;

    /// <summary>
    /// Defines the <see cref="ReaderLoanServiceTests" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ReaderLoanServiceTests
    {
        /// <summary>
        /// Defines the bookLoanDetailRepositoryMock.
        /// </summary>
        private Mock<IBookLoanDetailRepository> bookLoanDetailRepositoryMock;

        /// <summary>
        /// Defines the readerLoanRepositoryMock.
        /// </summary>
        private Mock<IReaderLoanRepository> readerLoanRepositoryMock;

        /// <summary>
        /// Defines the bookSampleRepositoryMock.
        /// </summary>
        private Mock<IBookSampleRepository> bookSampleRepositoryMock;

        /// <summary>
        /// Defines the userRepositoryMock.
        /// </summary>
        private Mock<IUserRepository> userRepositoryMock;

        /// <summary>
        /// Defines the librarySettingsRepositoryMock.
        /// </summary>
        private Mock<ILibrarySettingsRepository> librarySettingsRepositoryMock;

        /// <summary>
        /// Defines the librarySettingsService.
        /// </summary>
        private ILibrarySettingsService librarySettingsService;

        /// <summary>
        /// Defines the readerLoanService.
        /// </summary>
        private IReaderLoanService readerLoanService;

        /// <summary>
        /// Defines the logger.
        /// </summary>
        private Microsoft.Extensions.Logging.ILogger<IReaderLoanService> logger;

        /// <summary>
        /// Defines the UserStaff.
        /// </summary>
        private User userStaff = new User()
        {
            Id = 1,
            LibraryStaff = new LibraryStaff() { UserId = 1 }
        };

        /// <summary>
        /// Defines the User1Reader.
        /// </summary>
        private User user1Reader = new User()
        {
            Id = 2,
            Reader = new Reader() { UserId = 2 }
        };

        /// <summary>
        /// Defines the User2ReaderAndStaff.
        /// </summary>
        private User user2ReaderAndStaff = new User()
        {
            Id = 3,
            Reader = new Reader() { UserId = 3 },
            LibraryStaff = new LibraryStaff() { UserId = 3 }
        };

        /// <summary>
        /// Defines the User3Reader.
        /// </summary>
        private User user3Reader = new User()
        {
            Id = 4,
            Reader = new Reader() { UserId = 4 }
        };

        /// <summary>
        /// Defines the LibrarySettings.
        /// </summary>
        private LibrarySettings librarySettings = new LibrarySettings()
        {
            AllowedMonthsForSameDomain = 2,
            BorrowedBooksExtensionLimit = 2,
            Id = 1,
            BorrowedBooksPeriod = 10,
            LimitBookLend = 1,
            MaxAllowedBooksPerDomain = 2,
            MaxBookBorrowed = 3,
            MaxBooksBorrowedPerTime = 3,
            MaxBorrowedBooksPerDay = 7,
            MaxDomains = 5,
            SameBookRepeatBorrowingLimit = 2,
        };

        /// <summary>
        /// Defines the ReaderLoan.
        /// </summary>
        private List<ReaderLoan> readerLoan = new List<ReaderLoan>()
        {
            new ReaderLoan()
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="ReaderLoanServiceTests"/> class.
        /// </summary>
        public ReaderLoanServiceTests()
        {
            this.logger = LoggerExtensions.TestLoggingInstance<IReaderLoanService>();
        }

        /// <summary>
        /// The Setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.bookLoanDetailRepositoryMock = new Mock<IBookLoanDetailRepository>();
            this.readerLoanRepositoryMock = new Mock<IReaderLoanRepository>();
            this.bookSampleRepositoryMock = new Mock<IBookSampleRepository>();
            this.userRepositoryMock = new Mock<IUserRepository>();
            this.librarySettingsRepositoryMock = new Mock<ILibrarySettingsRepository>();
            this.librarySettingsRepositoryMock.Setup(x => x.Get()).Returns(this.librarySettings);
            this.librarySettingsService = new LibrarySettingsService(this.librarySettingsRepositoryMock.Object);
            this.readerLoanService = new ReaderLoanService(
                this.readerLoanRepositoryMock.Object, 
                this.bookLoanDetailRepositoryMock.Object,
                this.bookSampleRepositoryMock.Object, 
                this.librarySettingsService, 
                this.userRepositoryMock.Object, 
                this.logger);
        }

        /// <summary>
        /// The Invalid_Null_BookLoan_Test.
        /// </summary>
        [Test]
        public void Invalid_Null_BookLoan_Test()
        {
            var ex = Assert.Throws<NullReferenceException>(() => this.readerLoanService.Insert(null));
            Assert.That(ex.Message, Is.EqualTo("Object reference not set to an instance of an object."));
            Assert.Pass();
        }

        /// <summary>
        /// The Invalid_BookLoan_MissingBooksDetails_Test.
        /// </summary>
        [Test]
        public void Invalid_BookLoan_MissingBooksDetails_Test()
        {
            var readerLoan = new ReaderLoan
            {
                StaffId = 1,
                ReaderId = 1,
                LoanDate = new DateTime(),
                BorrowedBooks = 1
            };

            var ex = Assert.Throws<ArgumentException>(() => this.readerLoanService.Insert(readerLoan));
            Assert.That(ex.Message, Is.EqualTo("Cannot add new loan, entity is invalid"));
            Assert.Pass();
        }

        /// <summary>
        /// The Invalid_BookLoan_EmptyFields_Test.
        /// </summary>
        [Test]
        public void Invalid_BookLoan_EmptyFields_Test()
        {
            var readerLoan = new ReaderLoan
            {
                BorrowedBooks = 1,
                BookLoanDetails = new List<BookLoanDetail>
                {
                    new BookLoanDetail
                    {
                        Id = 1,
                        BookSampleId = 101,
                        BookEditionId = 201,
                        BookId = 301,
                        ReaderLoanId = 401,
                        LoanDate = DateTime.Now,
                        ExpectedReturnDate = DateTime.Now.AddDays(14),
                        EffectiveReturnDate = null
                    }
                }
            };

            var ex = Assert.Throws<ArgumentException>(() => this.readerLoanService.Insert(readerLoan));
            Assert.That(ex.Message, Is.EqualTo("Cannot add new loan, entity is invalid"));
            Assert.Pass();
        }

        /// <summary>
        /// The Invalid_BookLoan_IncorrectCount_Test.
        /// </summary>
        [Test]
        public void Invalid_BookLoan_IncorrectCount_Test()
        {
            var readerLoan = new ReaderLoan
            {
                StaffId = 1,
                ReaderId = 1,
                LoanDate = new DateTime(),
                BorrowedBooks = 2,
                BookLoanDetails = new List<BookLoanDetail>
                {
                    new BookLoanDetail
                    {
                        Id = 1,
                        BookSampleId = 101,
                        BookEditionId = 201,
                        BookId = 301,
                        ReaderLoanId = 401,
                        LoanDate = DateTime.Now,
                        ExpectedReturnDate = DateTime.Now.AddDays(14),
                        EffectiveReturnDate = null
                    }
                }
            };

            var ex = Assert.Throws<ArgumentException>(() => this.readerLoanService.Insert(readerLoan));
            Assert.That(ex.Message, Is.EqualTo("Cannot add new loan, entity is invalid"));
            Assert.Pass();
        }

        /// <summary>
        /// The Invalid_BookLoan_MissingReader_Test.
        /// </summary>
        [Test]
        public void Invalid_BookLoan_MissingReader_Test()
        {
            var readerLoan = new ReaderLoan
            {
                StaffId = 1,
                ReaderId = 50,
                LoanDate = new DateTime(),
                BorrowedBooks = 1,
                BookLoanDetails = new List<BookLoanDetail>
                {
                    new BookLoanDetail
                    {
                        Id = 1,
                        BookSampleId = 101,
                        BookEditionId = 201,
                        BookId = 301,
                        ReaderLoanId = 401,
                        LoanDate = DateTime.Now,
                        ExpectedReturnDate = DateTime.Now.AddDays(14),
                        EffectiveReturnDate = null
                    }
                }
            };

            var ex = Assert.Throws<ArgumentException>(() => this.readerLoanService.Insert(readerLoan));
            Assert.That(ex.Message, Is.EqualTo("Cannot add new loan, reader is missing"));
            Assert.Pass();
        }

        /// <summary>
        /// The Invalid_BookLoan_UserIsNotReader_Test.
        /// </summary>
        [Test]
        public void Invalid_BookLoan_UserIsNotReader_Test()
        {
            this.UserRepoGetSetup(new List<User>
            {
                new User { Id = 1, Email = "test@email.com" }
            });

            var readerLoan = new ReaderLoan
            {
                StaffId = 1,
                ReaderId = 1,
                LoanDate = new DateTime(),
                BorrowedBooks = 1,
                BookLoanDetails = new List<BookLoanDetail>
                {
                    new BookLoanDetail
                    {
                        Id = 1,
                        BookSampleId = 101,
                        BookEditionId = 201,
                        BookId = 301,
                        ReaderLoanId = 401,
                        LoanDate = DateTime.Now,
                        ExpectedReturnDate = DateTime.Now.AddDays(14),
                        EffectiveReturnDate = null
                    }
                }
            };

            var ex = Assert.Throws<ArgumentException>(() => this.readerLoanService.Insert(readerLoan));
            Assert.That(ex.Message, Is.EqualTo("Cannot add new loan, reader is missing"));
            Assert.Pass();
        }

        /// <summary>
        /// The UserRepoGetSetup.
        /// </summary>
        /// <param name="newUsers">The newUsers<see cref="List{User}"/>.</param>
        private void UserRepoGetSetup(List<User> newUsers)
        {
            this.userRepositoryMock.Setup(x => x.Get(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
                It.IsAny<string>()))
                .Returns<Expression<Func<User, bool>>, Func<IQueryable<User>, IOrderedQueryable<User>>, string>((filter, orderBy, includeProperties) =>
                {
                    var users = newUsers;

                    if (filter != null)
                    {
                        users = users.Where(filter.Compile()).ToList();
                    }

                    if (orderBy != null)
                    {
                        users = orderBy(users.AsQueryable()).ToList();
                    }

                    return users.AsQueryable();
                });
        }
    }
}
