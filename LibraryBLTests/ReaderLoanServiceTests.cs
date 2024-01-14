//------------------------------------------------------------------------------
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
    using System.Net;
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
        /// Defines the librarySettingsServiceMock.
        /// </summary>
        private Mock<ILibrarySettingsService> librarySettingsServiceMock;

        /// <summary>
        /// Defines the readerLoanService.
        /// </summary>
        private IReaderLoanService readerLoanService;

        /// <summary>
        /// Defines the logger.
        /// </summary>
        private Microsoft.Extensions.Logging.ILogger<IReaderLoanService> logger;

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
            this.librarySettingsServiceMock = new Mock<ILibrarySettingsService>();
            this.readerLoanService = new ReaderLoanService(
                this.readerLoanRepositoryMock.Object,
                this.bookLoanDetailRepositoryMock.Object,
                this.bookSampleRepositoryMock.Object,
                this.librarySettingsServiceMock.Object,
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
        /// The Insert_BookLoan_MissingBooksDetails_Test.
        /// </summary>
        [Test]
        public void Insert_BookLoan_MissingBooksDetails_Test()
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
        /// The Insert_MissingReaderId_Test.
        /// </summary>
        [Test]
        public void Insert_MissingReaderId_Test()
        {
            var readerLoan = new ReaderLoan
            {
                StaffId = 1,
                LoanDate = new DateTime(),
                BorrowedBooks = 1
            };

            var ex = Assert.Throws<ArgumentException>(() => this.readerLoanService.Insert(readerLoan));
            Assert.That(ex.Message, Is.EqualTo("Cannot add new loan, entity is invalid"));
            Assert.Pass();
        }

        /// <summary>
        /// The Insert_MissingStaffId_Test.
        /// </summary>
        [Test]
        public void Insert_MissingStaffId_Test()
        {
            var readerLoan = new ReaderLoan
            {
                ReaderId = 1,
                LoanDate = new DateTime(),
                BorrowedBooks = 1
            };

            var ex = Assert.Throws<ArgumentException>(() => this.readerLoanService.Insert(readerLoan));
            Assert.That(ex.Message, Is.EqualTo("Cannot add new loan, entity is invalid"));
            Assert.Pass();
        }

        /// <summary>
        /// The Insert_MissingLoanDate_Test.
        /// </summary>
        [Test]
        public void Insert_MissingLoanDate_Test()
        {
            var readerLoan = new ReaderLoan
            {
                ReaderId = 1,
                StaffId = 1,
                BorrowedBooks = 1
            };

            var ex = Assert.Throws<ArgumentException>(() => this.readerLoanService.Insert(readerLoan));
            Assert.That(ex.Message, Is.EqualTo("Cannot add new loan, entity is invalid"));
            Assert.Pass();
        }

        /// <summary>
        /// The Insert_InvalidLoanDate_Test.
        /// </summary>
        [Test]
        public void Insert_InvalidLoanDate_Test()
        {
            var readerLoan = new ReaderLoan
            {
                ReaderId = 1,
                StaffId = 1,
                BorrowedBooks = 1,
                LoanDate = new DateTime(1999, 5, 13),
            };

            var ex = Assert.Throws<ArgumentException>(() => this.readerLoanService.Insert(readerLoan));
            Assert.That(ex.Message, Is.EqualTo("Cannot add new loan, entity is invalid"));
            Assert.Pass();
        }

        /// <summary>
        /// The Insert_BookLoan_EmptyFields_Test.
        /// </summary>
        [Test]
        public void Insert_BookLoan_EmptyFields_Test()
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
                        EffectiveReturnDate = null,
                        ReaderLoan = null
                    }
                }
            };

            var ex = Assert.Throws<ArgumentException>(() => this.readerLoanService.Insert(readerLoan));
            Assert.That(ex.Message, Is.EqualTo("Cannot add new loan, entity is invalid"));
            Assert.Pass();
        }

        /// <summary>
        /// The Insert_BookLoan_IncorrectCount_Test.
        /// </summary>
        [Test]
        public void Insert_BookLoan_IncorrectCount_Test()
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
                        LoanDate = DateTime.Now
                    }
                }
            };

            var ex = Assert.Throws<ArgumentException>(() => this.readerLoanService.Insert(readerLoan));
            Assert.That(ex.Message, Is.EqualTo("Cannot add new loan, entity is invalid"));
            Assert.Pass();
        }

        /// <summary>
        /// The Insert_BookLoan_MissingReader_Test.
        /// </summary>
        [Test]
        public void Insert_BookLoan_MissingReader_Test()
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
                },
                Reader = new Reader(),
                ExtensionsGranted = 1,
                Id = 1
            };

            var ex = Assert.Throws<ArgumentException>(() => this.readerLoanService.Insert(readerLoan));
            Assert.That(ex.Message, Is.EqualTo("Cannot add new loan, reader is missing"));
            Assert.Pass();
        }

        /// <summary>
        /// The Insert_BookLoan_UserIsNotReader_Test.
        /// </summary>
        [Test]
        public void Insert_BookLoan_UserIsNotReader_Test()
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
        /// The Insert_BookLoan_UserIsNotReader_Test.
        /// </summary>
        [Test]
        public void Insert_BookLoan_UserIsMissing_Test()
        {
            this.UserRepoGetSetup(new List<User>());

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
        /// The Insert_BookLoan_StaffIsMissing_Test.
        /// </summary>
        [Test]
        public void Insert_BookLoan_StaffIsMissing_Test()
        {
            this.UserRepoGetSetup(new List<User>
            {
                new User { Id = 1, Email = "test@email.com", Reader = new Reader()}
            });

            var readerLoan = new ReaderLoan
            {
                StaffId = 2,
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
            Assert.That(ex.Message, Is.EqualTo("Cannot add new loan, staff is missing"));
            Assert.Pass();
        }

        /// <summary>
        /// The Insert_BookLoan_StaffIsMissing_Test.
        /// </summary>
        [Test]
        public void Insert_BookLoan_StaffIsMissingNull_Test()
        {
            this.UserRepoGetSetup(new List<User>
            {
                new User { Id = 1, Email = "test@email.com", Reader = new Reader()},
                new User { Id = 2, Email = "test@email.com"}
            });

            var readerLoan = new ReaderLoan
            {
                StaffId = 2,
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
            Assert.That(ex.Message, Is.EqualTo("Cannot add new loan, staff is missing"));
            Assert.Pass();
        }

        #region Validate Loan Details

        /// <summary>
        /// The Insert_BookLoanDetailInvalid_BookSampleId_Test.
        /// </summary>
        [Test]
        public void Insert_BookLoanDetailInvalid_BookSampleId_Test()
        {
            this.UserRepoGetSetup(new List<User>
            {
                new User { Id = 1, Email = "test@email.com", Reader = new Reader()},
                new User { Id = 2, Email = "test@email.com", LibraryStaff = new LibraryStaff()}
            });
            var bookLoanDetail1 = new BookLoanDetail
            {
                Id = 1,
                BookSampleId = 0,
                BookEditionId = 201,
                BookId = 301,
                ReaderLoanId = 401,
                LoanDate = DateTime.Now,
                ExpectedReturnDate = DateTime.Now.AddDays(14),
                EffectiveReturnDate = null
            };

            var readerLoan = new ReaderLoan
            {
                StaffId = 2,
                ReaderId = 1,
                LoanDate = new DateTime(),
                BorrowedBooks = 1,
                BookLoanDetails = new List<BookLoanDetail>() { bookLoanDetail1 }
            };

            var ex = Assert.Throws<ArgumentException>(() => this.readerLoanService.Insert(readerLoan));
            Assert.That(ex.Message, Is.EqualTo("Cannot add new book loan detail, entity is invalid"));
            Assert.Pass();
        }

        /// <summary>
        /// The Insert_BookLoanDetailInvalid_ReaderLoanId_Test.
        /// </summary>
        [Test]
        public void Insert_BookLoanDetailInvalid_ReaderLoanId_Test()
        {
            this.UserRepoGetSetup(new List<User>
            {
                new User { Id = 1, Email = "test@email.com", Reader = new Reader()},
                new User { Id = 2, Email = "test@email.com", LibraryStaff = new LibraryStaff()}
            });

            var bookLoanDetail1 = new BookLoanDetail
            {
                Id = 1,
                BookSampleId = 101,
                BookEditionId = 201,
                BookId = 301,
                ReaderLoanId = 0,
                LoanDate = DateTime.Now,
                ExpectedReturnDate = DateTime.Now.AddDays(14),
                EffectiveReturnDate = null
            };
            var readerLoan = new ReaderLoan
            {
                StaffId = 2,
                ReaderId = 1,
                LoanDate = new DateTime(),
                BorrowedBooks = 1,
                BookLoanDetails = new List<BookLoanDetail>() { bookLoanDetail1 }
            };

            var ex = Assert.Throws<ArgumentException>(() => this.readerLoanService.Insert(readerLoan));
            Assert.That(ex.Message, Is.EqualTo("Cannot add new book loan detail, entity is invalid"));
            Assert.Pass();
        }

        /// <summary>
        /// The Insert_BookLoanDetailInvalid_ExpectedReturnDate_Test.
        /// </summary>
        [Test]
        public void Insert_BookLoanDetailInvalid_ExpectedReturnDate_Test()
        {
            this.UserRepoGetSetup(new List<User>
            {
                new User { Id = 1, Email = "test@email.com", Reader = new Reader()},
                new User { Id = 2, Email = "test@email.com", LibraryStaff = new LibraryStaff()}
            });

            var bookLoanDetail1 = new BookLoanDetail
            {
                Id = 1,
                BookSampleId = 101,
                BookEditionId = 201,
                BookId = 301,
                ReaderLoanId = 401,
                ExpectedReturnDate = DateTime.Now.AddDays(-14),
                EffectiveReturnDate = null
            };
            var readerLoan = new ReaderLoan
            {
                StaffId = 2,
                ReaderId = 1,
                LoanDate = new DateTime(),
                BorrowedBooks = 1,
                BookLoanDetails = new List<BookLoanDetail>() { bookLoanDetail1 }
            };

            var ex = Assert.Throws<ArgumentException>(() => this.readerLoanService.Insert(readerLoan));
            Assert.That(ex.Message, Is.EqualTo("Cannot add new book loan detail, entity is invalid"));
            Assert.Pass();
        }

        /// <summary>
        /// The Insert_BookLoanDetailInvalid_EffectiveReturnDate_Test.
        /// </summary>
        [Test]
        public void Insert_BookLoanDetailInvalid_EffectiveReturnDate_Test()
        {
            this.UserRepoGetSetup(new List<User>
            {
                new User { Id = 1, Email = "test@email.com", Reader = new Reader()},
                new User { Id = 2, Email = "test@email.com", LibraryStaff = new LibraryStaff()}
            });

            var bookLoanDetail1 = new BookLoanDetail
            {
                Id = 1,
                BookSampleId = 101,
                BookEditionId = 201,
                BookId = 301,
                ReaderLoanId = 401,
                ExpectedReturnDate = DateTime.Now.AddDays(14),
                EffectiveReturnDate = DateTime.Now.AddDays(-14)
            };
            var readerLoan = new ReaderLoan
            {
                StaffId = 2,
                ReaderId = 1,
                LoanDate = new DateTime(),
                BorrowedBooks = 1,
                BookLoanDetails = new List<BookLoanDetail>() { bookLoanDetail1 }
            };

            var ex = Assert.Throws<ArgumentException>(() => this.readerLoanService.Insert(readerLoan));
            Assert.That(ex.Message, Is.EqualTo("Cannot add new book loan detail, entity is invalid"));
            Assert.Pass();
        }

        #endregion

        /// <summary>
        /// The Insert_BookSampleNotExist_Test.
        /// </summary>
        [Test]
        public void Insert_BookSampleNotExist_Test()
        {
            this.UserRepoGetSetup(new List<User>
            {
                new User { Id = 1, Email = "test@email.com", Reader = new Reader()},
                new User { Id = 2, Email = "test@email.com", LibraryStaff = new LibraryStaff()}
            });
            var bookLoanDetail1 = new BookLoanDetail
            {
                Id = 1,
                BookSampleId = 101,
                BookEditionId = 201,
                BookId = 301,
                ReaderLoanId = 401,
                LoanDate = DateTime.Now,
                ExpectedReturnDate = DateTime.Now.AddDays(14),
                EffectiveReturnDate = null
            };

            var readerLoan = new ReaderLoan
            {
                StaffId = 2,
                ReaderId = 1,
                LoanDate = new DateTime(),
                BorrowedBooks = 1,
                BookLoanDetails = new List<BookLoanDetail>() { bookLoanDetail1 }
            };

            var ex = Assert.Throws<ArgumentException>(() => this.readerLoanService.Insert(readerLoan));
            Assert.That(ex.Message, Is.EqualTo("Cannot add new book loan detail, bookSample is invalid"));
            Assert.Pass();
        }

        /// <summary>
        /// The Insert_CannotBorrowBooks_Test.
        /// </summary>
        [Test]
        public void Insert_CannotBorrowBooks_Test()
        {
            this.UserRepoGetSetup(new List<User>
            {
                new User { Id = 1, Email = "test@email.com", Reader = new Reader()},
                new User { Id = 2, Email = "test@email.com", LibraryStaff = new LibraryStaff()}
            });

            this.BookSampleRepoGetSetup(new List<BookSample>
            {
                new BookSample { Id = 101, BookEditionId = 201, AvailableForLoan = true, BookEdition= new BookEdition() }
            });

            this.librarySettingsServiceMock.Setup(i => i.CheckIfUserCanBorrowBooks(It.IsAny<User>(), It.IsAny<ReaderLoan>(), It.IsAny<List<ReaderLoan>>(), It.IsAny<int>()))
                .Throws(new Exception("User cannot borrow book because of the rules"));

            var bookLoanDetail1 = new BookLoanDetail
            {
                Id = 1,
                BookSampleId = 101,
                BookEditionId = 201,
                BookId = 301,
                ReaderLoanId = 401,
                LoanDate = DateTime.Now,
                ExpectedReturnDate = DateTime.Now.AddDays(14),
                EffectiveReturnDate = null
            };
            var readerLoan = new ReaderLoan
            {
                StaffId = 2,
                ReaderId = 1,
                LoanDate = new DateTime(),
                BorrowedBooks = 1,
                BookLoanDetails = new List<BookLoanDetail>() { bookLoanDetail1 }
            };

            var ex = Assert.Throws<Exception>(() => this.readerLoanService.Insert(readerLoan));
            Assert.That(ex.Message, Is.EqualTo("User cannot borrow book because of the rules"));
            Assert.Pass();
        }

        /// <summary>
        /// The Insert_NoBookLoanDetails_Test.
        /// </summary>
        [Test]
        public void Insert_NoBookLoanDetails_Test()
        {
            this.UserRepoGetSetup(new List<User>
            {
                new User { Id = 1, Email = "test@email.com", Reader = new Reader()},
                new User { Id = 2, Email = "test@email.com", LibraryStaff = new LibraryStaff()}
            });

            this.BookSampleRepoGetSetup(new List<BookSample>
            {
                new BookSample { Id = 101, BookEditionId = 201, AvailableForLoan = true, BookEdition= new BookEdition() }
            });
            var readerLoan = new ReaderLoan
            {
                StaffId = 2,
                ReaderId = 1,
                LoanDate = new DateTime(),
                BorrowedBooks = 0,
                BookLoanDetails = null
            };

            var result = this.readerLoanService.Insert(readerLoan);
            Assert.That(result, Is.Not.Null);
            Assert.Pass();
        }

        /// <summary>
        /// The Insert_Success_Test.
        /// </summary>
        [Test]
        public void Insert_Success_Test()
        {
            this.UserRepoGetSetup(new List<User>
            {
                new User { Id = 1, Email = "test@email.com", Reader = new Reader()},
                new User { Id = 2, Email = "test@email.com", LibraryStaff = new LibraryStaff()}
            });

            this.BookSampleRepoGetSetup(new List<BookSample>
            {
                new BookSample { Id = 101, BookEditionId = 201, AvailableForLoan = true, BookEdition= new BookEdition() }
            });

            var bookLoanDetail1 = new BookLoanDetail
            {
                Id = 1,
                BookSampleId = 101,
                BookEditionId = 201,
                BookId = 301,
                ReaderLoanId = 401,
                LoanDate = DateTime.Now,
                ExpectedReturnDate = DateTime.Now.AddDays(14),
                EffectiveReturnDate = null
            };
            var readerLoan = new ReaderLoan
            {
                StaffId = 2,
                ReaderId = 1,
                LoanDate = new DateTime(),
                BorrowedBooks = 1,
                BookLoanDetails = new List<BookLoanDetail>() { bookLoanDetail1 }
            };

            var result = this.readerLoanService.Insert(readerLoan);
            Assert.That(result, Is.Not.Null);
            Assert.Pass();
        }

        /// <summary>
        /// The ReturnBooks_InvalidReaderLoanId_Test.
        /// </summary>
        [Test]
        public void ReturnBooks_InvalidReaderLoanId_Test()
        {
            var ex = Assert.Throws<ArgumentException>(() => this.readerLoanService.ReturnBooks(1));
            Assert.That(ex.Message, Is.EqualTo("Cannot return books, loan is invalid"));
            Assert.Pass();
        }

        /// <summary>
        /// The ReturnBooks_NoLoanBook_Test.
        /// </summary>
        [Test]
        public void ReturnBooks_NoLoanBook_Test()
        {
            this.ReaderLoanRepoGetSetup(new List<ReaderLoan>
            {
                new ReaderLoan { Id = 1}
            });

            var ex = Assert.Throws<ArgumentException>(() => this.readerLoanService.ReturnBooks(1));
            Assert.That(ex.Message, Is.EqualTo("Cannot return books, loan doesn't have any books"));
            Assert.Pass();
        }


        /// <summary>
        /// The ReturnBooks_Succes_Test.
        /// </summary>
        [Test]
        public void ReturnBooks_Succes_Test()
        {
            this.ReaderLoanRepoGetSetup(new List<ReaderLoan>
            {
                new ReaderLoan {
                    Id = 1,
                    BookLoanDetails = new List<BookLoanDetail>
                    {
                        new BookLoanDetail { EffectiveReturnDate = new DateTime(2000,1,1)},
                        new BookLoanDetail ()
                    }
            }});

            this.readerLoanService.ReturnBooks(1);
            bookLoanDetailRepositoryMock.Verify(i => i.Update(It.IsAny<BookLoanDetail>()), Times.Once);
            Assert.Pass();
        }

        /// <summary>
        /// The ReturnBook_InvalidReaderLoanId_Test.
        /// </summary>
        [Test]
        public void ReturnBook_InvalidReaderLoanId_Test()
        {
            var ex = Assert.Throws<ArgumentException>(() => this.readerLoanService.ReturnBook(1, 1));
            Assert.That(ex.Message, Is.EqualTo("Cannot return book, loan is invalid"));
            Assert.Pass();
        }

        /// <summary>
        /// The ReturnBook_NoLoanBook_Test.
        /// </summary>
        [Test]
        public void ReturnBook_NoLoanBook_Test()
        {
            this.ReaderLoanRepoGetSetup(new List<ReaderLoan>
            {
                new ReaderLoan { Id = 1}
            });

            var ex = Assert.Throws<ArgumentException>(() => this.readerLoanService.ReturnBook(1, 1));
            Assert.That(ex.Message, Is.EqualTo("Cannot return book, loan doesn't have any books"));
            Assert.Pass();
        }


        /// <summary>
        /// The ReturnBook_BookIsNotBorrowed_Test.
        /// </summary>
        [Test]
        public void ReturnBook_BookIsNotBorrowed_Test()
        {
            this.ReaderLoanRepoGetSetup(new List<ReaderLoan>
            {
                new ReaderLoan {
                    Id = 1,
                    BookLoanDetails = new List<BookLoanDetail>
                    {
                        new BookLoanDetail { BookId = 1, EffectiveReturnDate = new DateTime(2000,1,1)},
                        new BookLoanDetail { BookId = 2 }
                    }
            }});

            var ex = Assert.Throws<ArgumentException>(() => this.readerLoanService.ReturnBook(1, 3));
            Assert.That(ex.Message, Is.EqualTo("Cannot return book, book is not present in current loan"));
            Assert.Pass();
        }

        /// <summary>
        /// The ReturnBook_BookIsAlreadyReturned_Test.
        /// </summary>
        [Test]
        public void ReturnBook_BookIsAlreadyReturned_Test()
        {
            this.ReaderLoanRepoGetSetup(new List<ReaderLoan>
            {
                new ReaderLoan {
                    Id = 1,
                    BookLoanDetails = new List<BookLoanDetail>
                    {
                        new BookLoanDetail { BookId = 1, EffectiveReturnDate = new DateTime(2000,1,1)},
                        new BookLoanDetail { BookId = 2 }
                    }
            }});

            var ex = Assert.Throws<ArgumentException>(() => this.readerLoanService.ReturnBook(1, 1));
            Assert.That(ex.Message, Is.EqualTo("Cannot return book, book is already returned"));
            Assert.Pass();
        }

        /// <summary>
        /// The ReturnBook_Success_Test.
        /// </summary>
        [Test]
        public void ReturnBook_Success_Test()
        {
            this.ReaderLoanRepoGetSetup(new List<ReaderLoan>
            {
                new ReaderLoan {
                    Id = 1,
                    BookLoanDetails = new List<BookLoanDetail>
                    {
                        new BookLoanDetail { BookId = 1, EffectiveReturnDate = new DateTime(2000,1,1)},
                        new BookLoanDetail { BookId = 2 }
                    }
            }});

            this.readerLoanService.ReturnBook(1, 2);
            bookLoanDetailRepositoryMock.Verify(i => i.Update(It.IsAny<BookLoanDetail>()), Times.Once);
            Assert.Pass();
        }

        /// <summary>
        /// The SetExtensionsForLoan_BookLoanInvalid_Test.
        /// </summary>
        [Test]
        public void SetExtensionsForLoan_BookLoanInvalid_Test()
        {
            var ex = Assert.Throws<ArgumentException>(() => this.readerLoanService.SetExtensionsForLoan(1));
            Assert.That(ex.Message, Is.EqualTo("Cannot update book loan detail, book loan is invalid"));
            Assert.Pass();
        }

        /// <summary>
        /// The SetExtensionsForLoan_MaximumExtensionsExceded_Test.
        /// </summary>
        [Test]
        public void SetExtensionsForLoan_MaximumExtensionsExceded_Test()
        {
            var bookLoanDetails = new List<BookLoanDetail>
            {
                new BookLoanDetail { Id = 1, BookId = 1, ReaderLoan = new ReaderLoan{ ReaderId = 1 } }
            };

            var bookLoans = new List<ReaderLoan>
            {
                new ReaderLoan {ReaderId = 1, ExtensionsGranted = 1, LoanDate = DateTime.Today.AddMonths(-2) },
                new ReaderLoan {ReaderId = 1, ExtensionsGranted = 2, LoanDate = DateTime.Today.AddMonths(-2) }
            };

            this.RepoGetSetup<BookLoanDetail>(bookLoanDetails, this.bookLoanDetailRepositoryMock.As<IRepository<BookLoanDetail>>());
            this.RepoGetSetup<ReaderLoan>(bookLoans, this.readerLoanRepositoryMock.As<IRepository<ReaderLoan>>());
            this.librarySettingsServiceMock.Setup(i => i.CheckIfUserCanExtendForLoan(It.IsAny<User>(), 3)).Returns(false);

            var ex = Assert.Throws<ArgumentException>(() => this.readerLoanService.SetExtensionsForLoan(1));
            Assert.That(ex.Message, Is.EqualTo("Exceeded maximum allowed extensions for borrowed books in the last three months."));
            Assert.Pass();
        }

        /// <summary>
        /// The SetExtensionsForLoan_MaximumExtensionsExceeded2_Test.
        /// </summary>
        [Test]
        public void SetExtensionsForLoan_MaximumExtensionsExceeded2_Test()
        {
            var bookLoanDetails = new List<BookLoanDetail>
            {
                new BookLoanDetail { Id = 1, BookId = 1, ReaderLoan = new ReaderLoan{ ReaderId = 1 } }
            };

            var bookLoans = new List<ReaderLoan>
            {
                new ReaderLoan {ReaderId = 1, ExtensionsGranted = 4, LoanDate = DateTime.Today.AddMonths(-2) },
                new ReaderLoan {ReaderId = 1, ExtensionsGranted = 2, LoanDate = DateTime.Today.AddMonths(-4) }
            };

            this.RepoGetSetup<BookLoanDetail>(bookLoanDetails, this.bookLoanDetailRepositoryMock.As<IRepository<BookLoanDetail>>());
            this.RepoGetSetup<ReaderLoan>(bookLoans, this.readerLoanRepositoryMock.As<IRepository<ReaderLoan>>());
            this.librarySettingsServiceMock.Setup(i => i.CheckIfUserCanExtendForLoan(It.IsAny<User>(), 1)).Returns(false);

            var ex = Assert.Throws<ArgumentException>(() => this.readerLoanService.SetExtensionsForLoan(1));
            Assert.That(ex.Message, Is.EqualTo("Exceeded maximum allowed extensions for borrowed books in the last three months."));
            Assert.Pass();
        }

        /// <summary>
        /// The SetExtensionsForLoan_Success_Test.
        /// </summary>
        [Test]
        public void SetExtensionsForLoan_Success_Test()
        {
            var bookLoanDetails = new List<BookLoanDetail>
            {
                new BookLoanDetail { Id = 1, BookId = 1, ReaderLoan = new ReaderLoan{ ReaderId = 1 } }
            };

            var bookLoans = new List<ReaderLoan>
            {
                new ReaderLoan {ReaderId = 1, ExtensionsGranted = 4, LoanDate = DateTime.Today.AddMonths(-2) },
                new ReaderLoan {ReaderId = 1, ExtensionsGranted = 2, LoanDate = DateTime.Today.AddMonths(-4) }
            };

            this.RepoGetSetup<BookLoanDetail>(bookLoanDetails, this.bookLoanDetailRepositoryMock.As<IRepository<BookLoanDetail>>());
            this.RepoGetSetup<ReaderLoan>(bookLoans, this.readerLoanRepositoryMock.As<IRepository<ReaderLoan>>());
            this.librarySettingsServiceMock.Setup(i => i.CheckIfUserCanExtendForLoan(It.IsAny<User>(), 4)).Returns(true);

            this.readerLoanService.SetExtensionsForLoan(1);
            this.readerLoanRepositoryMock.Verify(i => i.Update(It.IsAny<ReaderLoan>()), Times.Once);
            this.bookLoanDetailRepositoryMock.Verify(i => i.Update(It.IsAny<BookLoanDetail>()), Times.Once);
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

        /// <summary>
        /// The BookSampleRepoGetSetup.
        /// </summary>
        /// <param name="newBookSamples">The newBookSamples<see cref="List{BookSample}"/>.</param>
        private void BookSampleRepoGetSetup(List<BookSample> newBookSamples)
        {
            this.RepoGetSetup<BookSample>(newBookSamples, this.bookSampleRepositoryMock.As<IRepository<BookSample>>());
        }


        /// <summary>
        /// The ReaderLoanRepoGetSetup.
        /// </summary>
        /// <param name="readerLoans">The readerLoans<see cref="List{ReaderLoan}"/>.</param>
        private void ReaderLoanRepoGetSetup(List<ReaderLoan> readerLoans)
        {
            this.RepoGetSetup<ReaderLoan>(readerLoans, this.readerLoanRepositoryMock.As<IRepository<ReaderLoan>>());
        }

        /// <summary>
        /// The RepoGetSetup.
        /// </summary>
        /// <param name="entries">The entries<see cref="List{T}"/>.</param>
        /// <param name="repo">The repository<see cref="Mock{IRepository{T}}"/>.</param>
        private void RepoGetSetup<T>(List<T> entries, Mock<IRepository<T>> repo)
        {
            repo.Setup(x => x.Get(
                It.IsAny<Expression<Func<T, bool>>>(),
                It.IsAny<Func<IQueryable<T>, IOrderedQueryable<T>>>(),
                It.IsAny<string>()))
                .Returns<Expression<Func<T, bool>>, Func<IQueryable<T>, IOrderedQueryable<T>>, string>((filter, orderBy, includeProperties) =>
                {
                    if (filter != null)
                    {
                        entries = entries.Where(filter.Compile()).ToList();
                    }

                    if (orderBy != null)
                    {
                        entries = orderBy(entries.AsQueryable()).ToList();
                    }

                    return entries.AsQueryable();
                });
        }
    }
}
