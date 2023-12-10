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
    public class BookEditionServiceTests
    {
        #region Private fields

        IBookEditionService _service;
        private readonly Microsoft.Extensions.Logging.ILogger<IBookEditionService> _logger;
        Mock<IBookRepository> _bookRepositoryMock;
        Mock<IBookEditionRepository> _bookEditionRepositoryMock;

        #endregion

        public BookEditionServiceTests()
        {
            _logger = LoggerExtensions.TestLoggingInstance<IBookEditionService>();
        }

        [SetUp]
        public void Setup()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _bookEditionRepositoryMock = new Mock<IBookEditionRepository>();
            _service = new BookEditionService(_bookEditionRepositoryMock.Object, _logger, _bookRepositoryMock.Object);
        }

        [Test]
        public void Constructor_Test()
        {
            Assert.That(_service, Is.Not.Null);
            Assert.Pass();
        }

        [Test]
        public void AddBookEdition_NullArgumetException_Test()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _service.Insert(null));
            Assert.That(ex, Is.Not.Null);
            Assert.Pass();
        }

        [Test]
        public void AddBookEdition_InvalidEntity_Test()
        {
            var ex = Assert.Throws<ArgumentException>(() => _service.Insert(new BookEdition()));
            Assert.That(ex.Message, Is.EqualTo("Cannot add new book edition, entity is invalid"));
            Assert.Pass();
        }

        [Test]
        public void AddBookEdition_BookEditionExist_Test()
        {
            SetUpGetBookEdition(new List<BookEdition>() { NewBookEdition });

            var ex = Assert.Throws<ArgumentException>(() => _service.Insert(NewBookEdition));
            Assert.That(ex.Message, Is.EqualTo("Cannot add new book edition, entity already exists"));
            Assert.Pass();
        }

        [Test]
        public void AddBookEdition_BookDoesNotExist_Test()
        {
            SetUpGetBookEdition(new List<BookEdition>());

            var ex = Assert.Throws<ArgumentException>(() => _service.Insert(NewBookEdition));
            Assert.That(ex.Message, Is.EqualTo("Cannot add new book edition, book is missing"));
            Assert.Pass();
        }

        [Test]
        public void AddBookEdition_Success_Test()
        {
            SetUpGetBookEdition(new List<BookEdition>());
            SetUpGetBook(new List<Book>() { new Book() });

            var result = _service.Insert(NewBookEdition);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.Pass();
        }

        [Test]
        public void UpdateBookEdition_NullArgumetException_Test()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _service.Update(null));
            Assert.That(ex, Is.Not.Null);
            Assert.Pass();
        }

        [Test]
        public void UpdateBookEdition_InvalidEntity_Test()
        {
            var ex = Assert.Throws<ArgumentException>(() => _service.Update(new BookEdition()));
            Assert.That(ex, Is.Not.Null);
            Assert.That(ex.Message, Is.EqualTo("Cannot update book edition, invalid entity"));
            Assert.Pass();
        }

        [Test]
        public void UpdateBookEdition_BookEditionDoesNotExist_Test()
        {
            SetUpGetBookEdition(new List<BookEdition>());

            var ex = Assert.Throws<ArgumentException>(() => _service.Update(NewBookEdition));
            Assert.That(ex.Message, Is.EqualTo("Cannot update book edition, entity is missing"));
            Assert.Pass();
        }

        [Test]
        public void UpdateBookEdition_BookIdChanged_Test()
        {
            var bookEdition = NewBookEdition;
            bookEdition.BookId = 2;

            SetUpGetBookEdition(new List<BookEdition>() { bookEdition });

            var ex = Assert.Throws<ArgumentException>(() => _service.Update(NewBookEdition));
            Assert.That(ex.Message, Is.EqualTo("Cannot add update book edition, book id was changed"));
            Assert.Pass();
        }

        [Test]
        public void UpdateBookEdition_Success_Test()
        {
            SetUpGetBookEdition(new List<BookEdition>() { NewBookEdition });

            var result = _service.Update(NewBookEdition);
            
            _bookEditionRepositoryMock.Verify(x => x.Update(It.IsAny<BookEdition>()), Times.Once);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.Pass();
        }

        [Test]
        public void DeleteBookEdition_BookEditionDoesNotExist_Test()
        {
            SetUpGetBookEdition(new List<BookEdition>());

            var ex = Assert.Throws<ArgumentException>(() => _service.Delete(NewBookEdition));
            Assert.That(ex.Message, Is.EqualTo("Cannot delete book edition, entity is missing"));
            Assert.Pass();
        }

        [Test]
        public void DeleteBookEdition_HasBookSamples_Test()
        {
            var bookEdition = NewBookEdition;
            bookEdition.BookSamples = new List<BookSample>() { new BookSample() };

            SetUpGetBookEdition(new List<BookEdition>() { bookEdition });

            var ex = Assert.Throws<ArgumentException>(() => _service.Delete(bookEdition));
            Assert.That(ex.Message, Is.EqualTo("Cannot delete book edition, entity has relations"));
            Assert.Pass();
        }

        [Test]
        public void DeleteBookEdition_Succes_Test()
        {
            SetUpGetBookEdition(new List<BookEdition>() { NewBookEdition });

            _service.Delete(NewBookEdition);
            _bookEditionRepositoryMock.Verify(x => x.Delete(It.IsAny<BookEdition>()), Times.Once);
            Assert.Pass();
        }


        private void SetUpGetBookEdition(List<BookEdition> bookEditions)
        {
            _bookEditionRepositoryMock.Setup(i => i.Get(It.IsAny<Expression<Func<BookEdition, bool>>>(), It.IsAny<Func<IQueryable<BookEdition>, IOrderedQueryable<BookEdition>>>(), It.IsAny<string>())).Returns(bookEditions);
        }

        private void SetUpGetBook(List<Book> books)
        {
            _bookRepositoryMock.Setup(i => i.Get(It.IsAny<Expression<Func<Book, bool>>>(), It.IsAny<Func<IQueryable<Book>, IOrderedQueryable<Book>>>(), It.IsAny<string>())).Returns(books);
        }


        public BookEdition NewBookEdition => new BookEdition()
        {
            BookId = 1,
            BookType = "Type",
            Id = 1,
            PageNumber = 1,
            Edition = "Edition",
            ReleaseYear = 2023
        };
    }
}
