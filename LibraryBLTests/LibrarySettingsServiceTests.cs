using Library.BL.Infrastructure;
using Library.BL.Interfaces;
using Library.BL.Services;
using Library.DAL.DomainModel;
using Library.DAL.Interfaces;
using log4net;
using Moq;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using static System.Reflection.Metadata.BlobBuilder;

namespace LibraryBLTests
{
    [ExcludeFromCodeCoverage]
    public class LibrarySettingsServiceTests
    {
        #region Private fields

        ILibrarySettingsService _service;
        Mock<ILibrarySettingsRepository> _repositoryMock;

        #endregion

        public LibrarySettingsServiceTests()
        {
        }

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<ILibrarySettingsRepository>();
            _service = new LibrarySettingsService(_repositoryMock.Object);
        }

        [Test]
        public void Constructor_Test()
        {
            Assert.That(_service, Is.Not.Null);
            Assert.Pass();
        }

        [Test]
        public void Constructor_ArgumentNullException_Test()
        {
            Assert.Throws<ArgumentNullException>(() => new LibrarySettingsService(null));
            Assert.Pass();
        }

        [Test]
        public void CheckIfUserCanBorrowBooks_MaximumBooksBorrowed_UserInd2_Test()
        {
            var librarySettings = GetLibrarySettings();
            librarySettings.MaxBookBorrowed = 1;
            SetLibrarySettings(librarySettings);

            var previousLoans = PreviousLoans;
            previousLoans.Add(previousLoans.First());

            var ex = Assert.Throws<ArgumentException>(() => _service.CheckIfUserCanBorrowBooks(User, NewLoan, previousLoans, StaffLendCount));
            Assert.That(ex, Is.Not.Null);
            Assert.That(ex.Message, Is.EqualTo("Exceeded maximum books borrowed in the specified period."));
            Assert.Pass();
        }

        [Test]
        public void CheckIfUserCanBorrowBooks_MaximumBooksBorrowed_UserInd1_Test()
        {
            SetLibrarySettings(GetLibrarySettings());

            var user = User;
            user.LibraryStaff = new LibraryStaff();

            var ex = Assert.Throws<ArgumentException>(() => _service.CheckIfUserCanBorrowBooks(user, NewLoan, PreviousLoans, StaffLendCount));
            Assert.That(ex, Is.Not.Null);
            Assert.That(ex.Message, Is.EqualTo("Exceeded maximum books borrowed in the specified period."));
            Assert.Pass();
        }

        [Test]
        public void CheckIfUserCanBorrowBooks_MaximumBooksBorrowedPerTime_UserInd2_Test()
        {
            SetLibrarySettings(GetLibrarySettings());

            var ex = Assert.Throws<ArgumentException>(() => _service.CheckIfUserCanBorrowBooks(User, NewLoan, PreviousLoans, StaffLendCount));
            Assert.That(ex, Is.Not.Null);
            Assert.That(ex.Message, Is.EqualTo("Exceeded maximum books borrowed per time."));
            Assert.Pass();
        }

        [Test]
        public void CheckIfUserCanBorrowBooks_MaximumBooksBorrowedPerTime_UserInd1_Test()
        {
            SetLibrarySettings(GetLibrarySettings());

            var user = User;
            user.LibraryStaff = new LibraryStaff();

            var ex = Assert.Throws<ArgumentException>(() => _service.CheckIfUserCanBorrowBooks(User, NewLoan, PreviousLoans, StaffLendCount));
            Assert.That(ex, Is.Not.Null);
            Assert.That(ex.Message, Is.EqualTo("Exceeded maximum books borrowed per time."));
            Assert.Pass();
        }

        [Test]
        public void CheckIfUserCanBorrowBooks_NotDistinctCategories_Test()
        {
            var librarySettings = GetLibrarySettings();
            librarySettings.MaxBooksBorrowedPerTime = 10;
            SetLibrarySettings(librarySettings);

            var ex = Assert.Throws<ArgumentException>(() => _service.CheckIfUserCanBorrowBooks(User, NewLoan, PreviousLoans, StaffLendCount));
            Assert.That(ex, Is.Not.Null);
            Assert.That(ex.Message, Is.EqualTo("At least 2 distinct categories are required for borrowing 3 or more books."));
            Assert.Pass();
        }

        [Test]
        public void CheckIfUserCanBorrowBooks_DistinctCategories_Test()
        {
            var librarySettings = GetLibrarySettings();
            librarySettings.MaxBooksBorrowedPerTime = 10;
            librarySettings.MaxAllowedBooksPerDomain = 1;
            SetLibrarySettings(librarySettings);

            var newLoan = NewLoan;
            newLoan.BookLoanDetails.First().BookSample.BookEdition.Book.BookDomains.First().DomainId = 2;

            var ex = Assert.Throws<ArgumentException>(() => _service.CheckIfUserCanBorrowBooks(User, newLoan, PreviousLoans, StaffLendCount));
            Assert.That(ex, Is.Not.Null);
            Assert.That(ex.Message, Is.EqualTo("Exceeded maximum allowed books from the same domain in the specified period."));
            Assert.Pass();
        }

        [Test]
        public void CheckIfUserCanBorrowBooks_MaxAllowedBooksPerDomain_Test()
        {
            var librarySettings = GetLibrarySettings();
            librarySettings.MaxBooksBorrowedPerTime = 10;
            SetLibrarySettings(librarySettings);

            var newLoan = NewLoan;
            newLoan.BookLoanDetails.First().BookSample.BookEdition.Book.BookDomains.First().DomainId = 2;

            var previousLoans = PreviousLoans;
            previousLoans.First().BookLoanDetails.First().LoanDate = DateTime.Now;

            var ex = Assert.Throws<ArgumentException>(() => _service.CheckIfUserCanBorrowBooks(User, newLoan, previousLoans, StaffLendCount));
            Assert.That(ex, Is.Not.Null);
            Assert.That(ex.Message, Is.EqualTo("Cannot borrow the same book within the specified interval."));
            Assert.Pass();
        }

        [Test]
        public void CheckIfUserCanBorrowBooks_LimitBookLend_Test()
        {
            var librarySettings = GetLibrarySettings();
            librarySettings.MaxBooksBorrowedPerTime = 10;
            SetLibrarySettings(librarySettings);

            var newLoan = NewLoan;
            newLoan.BookLoanDetails.First().BookSample.BookEdition.Book.BookDomains.First().DomainId = 2;

            var ex = Assert.Throws<ArgumentException>(() => _service.CheckIfUserCanBorrowBooks(User, newLoan, PreviousLoans, 11));
            Assert.That(ex, Is.Not.Null);
            Assert.That(ex.Message, Is.EqualTo("Exceeded maximum books that library staff can lend in a day."));
            Assert.Pass();
        }

        [Test]
        public void CheckIfUserCanBorrowBooks_MaxBorrowedBooksPerDay_Test()
        {
            var librarySettings = GetLibrarySettings();
            librarySettings.MaxBooksBorrowedPerTime = 10;
            SetLibrarySettings(librarySettings);

            var newLoan = NewLoan;
            newLoan.BookLoanDetails.First().BookSample.BookEdition.Book.BookDomains.First().DomainId = 2;

            var ex = Assert.Throws<ArgumentException>(() => _service.CheckIfUserCanBorrowBooks(User, newLoan, PreviousLoans, StaffLendCount));
            Assert.That(ex, Is.Not.Null);
            Assert.That(ex.Message, Is.EqualTo("Reader exceed limit for today"));
            Assert.Pass();
        }


        [Test]
        public void CheckIfUserCanBorrowBooks_Success_Test()
        {
            var librarySettings = GetLibrarySettings();
            librarySettings.MaxBooksBorrowedPerTime = 10;
            librarySettings.SameBookRepeatBorrowingLimt = 5;
            librarySettings.MaxBookBorrowed = 4;
            SetLibrarySettings(librarySettings);

            var user = User;
            user.LibraryStaff = new LibraryStaff();

            var newLoan = NewLoan;
            newLoan.BookLoanDetails.First().BookSample.BookEdition.Book.BookDomains.First().DomainId = 2;

            _service.CheckIfUserCanBorrowBooks(user, newLoan, PreviousLoans, StaffLendCount);
            Assert.Pass();
        }

        private LibrarySettings GetLibrarySettings()
        {
            return new LibrarySettings()
            {
                MaxBookBorrowed = 2,
                BorrowedBooksPeriod = 10,
                MaxBooksBorrowedPerTime = 1,
                MaxAllowedBooksPerDomain = 3,
                SameBookRepeatBorrowingLimt = 10,
                LimitBookLend = 10,
            };
        }

        private void SetLibrarySettings(LibrarySettings librarySettings)
        {
            _service.LibrarySettings = librarySettings;
        }

        private User User => new User() { };

        private ReaderLoan NewLoan => new ReaderLoan()
        {
            StaffId = 1,
            ReaderId = 1,
            LoanDate = new DateTime(),
            BorrowedBooks = 1,
            BookLoanDetails = new List<BookLoanDetail> { NewBookLoanDetail, NewBookLoanDetail, NewBookLoanDetail }
        };

        private List<ReaderLoan> PreviousLoans => new List<ReaderLoan>()
            {
                new ReaderLoan()
                {
                    LoanDate=DateTime.Now.AddDays(-4),
                    BookLoanDetails= new List<BookLoanDetail> { NewBookLoanDetail }
                }, new ReaderLoan()
                {
                    LoanDate=DateTime.Now.AddDays(-4),
                    BookLoanDetails= new List<BookLoanDetail> { NewBookLoanDetail }
                }
            };

        private int StaffLendCount => 2;

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

    }
}
