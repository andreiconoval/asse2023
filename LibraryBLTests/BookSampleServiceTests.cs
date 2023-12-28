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
    public class BookSampleServiceTests
    {

        #region Private fields

        IBookSampleService _service;
        private readonly Microsoft.Extensions.Logging.ILogger<IBookSampleService> _logger;
        Mock<IBookSampleRepository> _repositoryMock;
        Mock<IBookEditionRepository> _bookEditionRepositoryMock;
        Mock<IBookLoanDetailRepository> _bookLoanDetailRepositoryMock;

        #endregion

        public BookSampleServiceTests()
        {
            _logger = LoggerExtensions.TestLoggingInstance<IBookSampleService>();
        }

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<IBookSampleRepository>();
            _bookEditionRepositoryMock = new Mock<IBookEditionRepository>();
            _bookLoanDetailRepositoryMock = new Mock<IBookLoanDetailRepository>();
            _service = new BookSampleService(_repositoryMock.Object, _bookEditionRepositoryMock.Object, _bookLoanDetailRepositoryMock.Object, _logger);
        }

        [Test]
        public void Constructor_Test()
        {
            Assert.That(_service, Is.Not.Null);
            Assert.Pass();
        }

        [Test]
        public void Insert_NullArgumetException_Test()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _service.Insert(null));
            Assert.That(ex, Is.Not.Null);
            Assert.Pass();
        }

        [Test]
        public void Insert_InvalidEntity_Test()
        {
            var ex = Assert.Throws<ArgumentException>(() => _service.Insert(new BookSample()));
            Assert.That(ex.Message, Is.EqualTo("Cannot add book sample, invalid entity"));
            Assert.Pass();
        }

        [Test]
        public void Insert_InvalidId_Test()
        {
            var bookSample = NewBookSample;

            bookSample.BookEditionId = default;

            var ex = Assert.Throws<ArgumentException>(() => _service.Insert(bookSample));
            Assert.That(ex.Message, Is.EqualTo("Cannot add book sample, invalid entity"));
            Assert.Pass();
        }

        [Test]
        public void Insert_NotAvailableForLoanAndHall_Test()
        {
            var bookSample = NewBookSample;

            bookSample.AvailableForLoan = false;
            bookSample.AvailableForHall = false;

            var ex = Assert.Throws<ArgumentException>(() => _service.Insert(bookSample));
            Assert.That(ex.Message, Is.EqualTo("Cannot add book sample, invalid entity"));
            Assert.Pass();
        }

        [Test]
        public void Insert_BookEditionDoesNotExist_Test()
        {
            SetUpGetBookEdition(new List<BookEdition>());

            var ex = Assert.Throws<ArgumentException>(() => _service.Insert(NewBookSample));
            Assert.That(ex.Message, Is.EqualTo("Cannot add book sample, book edition do not exists"));
            Assert.Pass();
        }

        [Test]
        public void Insert_InvalidOperation_Test()
        {
            SetUpGetBookEdition(new List<BookEdition>() { new BookEdition() });
            _repositoryMock.Setup(i => i.Insert(It.IsAny<BookSample>())).Throws(new Exception("Invalid operation"));

            var ex = Assert.Throws<Exception>(() => _service.Insert(NewBookSample));
            Assert.That(ex.Message, Is.EqualTo("Invalid operation"));
            Assert.Pass();
        }

        [Test]
        public void Insert_Succes_Test()
        {
            SetUpGetBookEdition(new List<BookEdition>() { new BookEdition() });

            var result = _service.Insert(NewBookSample);
            Assert.That(result.IsValid, Is.EqualTo(true));
            _repositoryMock.Verify(i => i.Insert(It.IsAny<BookSample>()), Times.Once);
            Assert.Pass();
        }

        [Test]
        public void Update_NullArgumetException_Test()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _service.Update(null));
            Assert.That(ex, Is.Not.Null);
            Assert.Pass();
        }

        [Test]
        public void Update_InvalidEntity_Test()
        {
            var ex = Assert.Throws<ArgumentException>(() => _service.Update(new BookSample()));
            Assert.That(ex.Message, Is.EqualTo("Cannot update book sample, invalid entity"));
            Assert.Pass();
        }

        [Test]
        public void Update_InvalidId_Test()
        {
            var bookSample = NewBookSample;

            bookSample.BookEditionId = default;

            var ex = Assert.Throws<ArgumentException>(() => _service.Update(bookSample));
            Assert.That(ex.Message, Is.EqualTo("Cannot update book sample, invalid entity"));
            Assert.Pass();
        }

        [Test]
        public void Update_NotAvailableForLoanAndHall_Test()
        {
            var bookSample = NewBookSample;

            bookSample.AvailableForLoan = false;
            bookSample.AvailableForHall = false;

            var ex = Assert.Throws<ArgumentException>(() => _service.Update(bookSample));
            Assert.That(ex.Message, Is.EqualTo("Cannot update book sample, invalid entity"));
            Assert.Pass();
        }

        [Test]
        public void Update_BookSampleDoesntExist_Test()
        {
            var ex = Assert.Throws<ArgumentException>(() => _service.Update(NewBookSample));
            Assert.That(ex.Message, Is.EqualTo("Cannot update book sample, book sample do not exists"));
            Assert.Pass();
        }

        [Test]
        public void Update_BookEditionDoesntExist_Test()
        {
            SetUpGetBookSample(new List<BookSample>() { NewBookSample });

            var ex = Assert.Throws<ArgumentException>(() => _service.Update(NewBookSample));
            Assert.That(ex.Message, Is.EqualTo("Cannot add book sample, book edition do not exists"));
            Assert.Pass();
        }

        [Test]
        public void Update_InvalidOperation_Test()
        {
            SetUpGetBookSample(new List<BookSample>() { NewBookSample });
            SetUpGetBookEdition(new List<BookEdition>() { new BookEdition() });
            _repositoryMock.Setup(i => i.Update(It.IsAny<BookSample>())).Throws(new Exception("Invalid operation"));

            var ex = Assert.Throws<Exception>(() => _service.Update(NewBookSample));
            Assert.That(ex.Message, Is.EqualTo("Invalid operation"));
            Assert.Pass();
        }

        [Test]
        public void Update_Succes_Test()
        {
            SetUpGetBookSample(new List<BookSample>() { NewBookSample });
            SetUpGetBookEdition(new List<BookEdition>() { new BookEdition() });

            var result = _service.Update(NewBookSample);
            Assert.That(result.IsValid, Is.EqualTo(true));
            _repositoryMock.Verify(i => i.Update(It.IsAny<BookSample>()), Times.Once);
            Assert.Pass();
        }

        [Test]
        public void Delete_NullParameters_Test()
        {
            var ex = Assert.Throws<ArgumentException>(() => _service.Delete(null));
            Assert.That(ex.Message, Is.EqualTo("Cannot delete book sample, invalid entity"));
            Assert.Pass();
        }

        [Test]
        public void Delete_InvalidId_Test()
        {
            var ex = Assert.Throws<ArgumentException>(() => _service.Delete(new BookSample { BookEditionId = 0 }));
            Assert.That(ex.Message, Is.EqualTo("Cannot delete book sample, invalid entity"));
            Assert.Pass();
        }

        [Test]
        public void Delete_BookBookLoanExists_Test()
        {
            SetUpGetBookLoan(new List<BookLoanDetail>() { new BookLoanDetail() });

            var ex = Assert.Throws<ArgumentException>(() => _service.Delete(NewBookSample));
            Assert.That(ex.Message, Is.EqualTo("Cannot delete book sample, book is in use"));
            Assert.Pass();
        }

        [Test]
        public void Delete_Succes_Test()
        {
            _service.Delete(NewBookSample);
            _repositoryMock.Verify(i => i.Delete(It.IsAny<BookSample>()), Times.Once);
            Assert.Pass();
        }

        [Test]
        public void GetAll_Test()
        {
            SetUpGetBookSample(new List<BookSample>() { NewBookSample });

            var result = _service.GetAll();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.Pass();
        }

        private void SetUpGetBookEdition(List<BookEdition> bookEditions)
        {
            _bookEditionRepositoryMock.Setup(i => i.Get(It.IsAny<Expression<Func<BookEdition, bool>>>(), It.IsAny<Func<IQueryable<BookEdition>, IOrderedQueryable<BookEdition>>>(), It.IsAny<string>()))
                .Returns(bookEditions);
        }

        private void SetUpGetBookSample(List<BookSample> bookSamples)
        {
            _repositoryMock.Setup(i => i.Get(It.IsAny<Expression<Func<BookSample, bool>>>(), It.IsAny<Func<IQueryable<BookSample>, IOrderedQueryable<BookSample>>>(), It.IsAny<string>()))
                .Returns(bookSamples);
        }

        private void SetUpGetBookLoan(List<BookLoanDetail> bookLoanDetail)
        {
            _bookLoanDetailRepositoryMock.Setup(i => i.Get(It.IsAny<Expression<Func<BookLoanDetail, bool>>>(), It.IsAny<Func<IQueryable<BookLoanDetail>, IOrderedQueryable<BookLoanDetail>>>(), It.IsAny<string>()))
                .Returns(bookLoanDetail);
        }

        public BookSample NewBookSample => new BookSample()
        {
            BookEditionId = 1,
            Id = 1,
            AvailableForLoan = true,
            AvailableForHall = true
        };
    }
}
