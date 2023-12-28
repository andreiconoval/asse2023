using FluentValidation;
using Library.BL.Infrastructure;
using Library.BL.Interfaces;
using Library.BL.Services;
using Library.DAL.DomainModel;
using Library.DAL.Interfaces;
using Moq;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace LibraryBLTests
{
    [ExcludeFromCodeCoverage]
    public class ReaderLoanServiceTests
    {

        #region Private fields

        private Mock<IBookLoanDetailRepository> _bookLoanDetailRepositoryMock;
        private Mock<IReaderLoanRepository> _readerLoanRepositoryMock;
        private Mock<IBookSampleRepository> _bookSampleRepositoryMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<ILibrarySettingsRepository> _librarySettingsRepositoryMock;
        private ILibrarySettingsService _librarySettingsService;
        private IReaderLoanService _readerLoanService;
        private readonly Microsoft.Extensions.Logging.ILogger<IReaderLoanService> _logger;

        #endregion

        #region Constants

        private User UserStaff = new User()
        {
            Id = 1,
            LibraryStaff = new LibraryStaff() { UserId = 1 }
        };

        private User User1Reader = new User()
        {
            Id = 2,
            Reader = new Reader() { UserId = 2 }
        };

        private User User2ReaderAndStaff = new User()
        {
            Id = 3,
            Reader = new Reader() { UserId = 3 },
            LibraryStaff = new LibraryStaff() { UserId = 3 }

        };

        private User User3Reader = new User()
        {
            Id = 4,
            Reader = new Reader() { UserId = 4 }
        };

        private LibrarySettings LibrarySettings = new LibrarySettings()
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
            SameBookRepeatBorrowingLimt = 2,
        };


        private List<ReaderLoan> ReaderLoan = new List<ReaderLoan>()
        {
            new ReaderLoan()
            {

            }
        };
        #endregion

        public ReaderLoanServiceTests()
        {
            _logger = LoggerExtensions.TestLoggingInstance<IReaderLoanService>();
        }



        private void UserRepoGetSetup(List<User> newUsers)
        {
            _userRepositoryMock.Setup(x => x.Get(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
                It.IsAny<string>()))
                .Returns<Expression<Func<User, bool>>, Func<IQueryable<User>, IOrderedQueryable<User>>, string>((filter, orderBy, includeProperties) =>
                {
                    var users = newUsers;

                    // Apply the filter if provided
                    if (filter != null)
                    {
                        users = users.Where(filter.Compile()).ToList();
                    }

                    // Apply ordering if provided
                    if (orderBy != null)
                    {
                        users = orderBy(users.AsQueryable()).ToList();
                    }

                    return users.AsQueryable();
                });
        }

        [SetUp]
        public void Setup()
        {
            _bookLoanDetailRepositoryMock = new Mock<IBookLoanDetailRepository>();
            _readerLoanRepositoryMock = new Mock<IReaderLoanRepository>();
            _bookSampleRepositoryMock = new Mock<IBookSampleRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _librarySettingsRepositoryMock = new Mock<ILibrarySettingsRepository>();
            _librarySettingsRepositoryMock.Setup(x => x.Get()).Returns(LibrarySettings);

            _librarySettingsService = new LibrarySettingsService(_librarySettingsRepositoryMock.Object);
            _readerLoanService = new ReaderLoanService(_readerLoanRepositoryMock.Object, _bookLoanDetailRepositoryMock.Object,
               _bookSampleRepositoryMock.Object, _librarySettingsService, _userRepositoryMock.Object, _logger);

        }


        [Test]
        public void Invalid_Null_BookLoan_Test()
        {
            var ex = Assert.Throws<NullReferenceException>(() => _readerLoanService.Insert(null));
            Assert.That(ex.Message, Is.EqualTo("Object reference not set to an instance of an object."));
            Assert.Pass();
        }

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

            var ex = Assert.Throws<ArgumentException>(() => _readerLoanService.Insert(readerLoan));
            Assert.That(ex.Message, Is.EqualTo("Cannot add new loan, entity is invalid"));
            Assert.Pass();
        }


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

            var ex = Assert.Throws<ArgumentException>(() => _readerLoanService.Insert(readerLoan));
            Assert.That(ex.Message, Is.EqualTo("Cannot add new loan, entity is invalid"));
            Assert.Pass();
        }


        [Test]
        public void Invalid_BookLoan_IncorectCount_Test()
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

            var ex = Assert.Throws<ArgumentException>(() => _readerLoanService.Insert(readerLoan));
            Assert.That(ex.Message, Is.EqualTo("Cannot add new loan, entity is invalid"));
            Assert.Pass();
        }


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

            var ex = Assert.Throws<ArgumentException>(() => _readerLoanService.Insert(readerLoan));
            Assert.That(ex.Message, Is.EqualTo("Cannot add new loan, reader is missing"));
            Assert.Pass();
        }


        [Test]
        public void Invalid_BookLoan_UserIsNotReader_Test()
        {
            UserRepoGetSetup(new List<User>
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

            var ex = Assert.Throws<ArgumentException>(() => _readerLoanService.Insert(readerLoan));
            Assert.That(ex.Message, Is.EqualTo("Cannot add new loan, reader is missing"));
            Assert.Pass();
        }
    }
}

