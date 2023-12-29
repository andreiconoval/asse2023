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
    public class BookServiceTests
    {
        #region Private fields

        IBookService _service;
        private readonly Microsoft.Extensions.Logging.ILogger<IBookService> _logger;
        Mock<IBookRepository> _bookRepositoryMock;
        Mock<IBookDomainService> _bookDomainServiceMock;
        Mock<IBookAuthorService> _bookAuthorServiceMock;

        #endregion

        public BookServiceTests()
        {
            _logger = LoggerExtensions.TestLoggingInstance<IBookService>();
        }

        [SetUp]
        public void Setup()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _bookDomainServiceMock = new Mock<IBookDomainService>();
            _bookAuthorServiceMock = new Mock<IBookAuthorService>();
            _service = new BookService(_bookRepositoryMock.Object, _logger, _bookDomainServiceMock.Object, _bookAuthorServiceMock.Object);
        }

        [Test]
        public void Constructor_Test()
        {
            Assert.That(_service, Is.Not.Null);
            Assert.Pass();
        }

        [Test]
        public void AddBook_NullArgumetException_Test()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _service.Insert(null));
            Assert.That(ex, Is.Not.Null);
            Assert.Pass();
        }

        [Test]
        public void AddBook_InvalidEntity_Test()
        {
            var ex = Assert.Throws<ArgumentException>(() => _service.Insert(new Book()));
            Assert.That(ex.Message, Is.EqualTo("Cannot add new book, entity is invalid"));
            Assert.Pass();
        }

        [Test]
        public void AddBook_BookExist_Test()
        {
            SetUpGetBook(new List<Book>() { NewBook });

            var ex = Assert.Throws<ArgumentException>(() => _service.Insert(NewBook));
            Assert.That(ex.Message, Is.EqualTo("Cannot add new book, entity already exists"));
            Assert.Pass();
        }

        [Test]
        public void AddBook_Success_Test()
        {
            SetUpGetBook(new List<Book>());

            var result = _service.Insert(NewBook);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.Pass();
        }

        [Test]
        public void UpdateBook_NullArgumetException_Test()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _service.Update(null));
            Assert.That(ex, Is.Not.Null);
            Assert.Pass();
        }

        [Test]
        public void UpdateBook_InvalidEntity_Test()
        {
            var ex = Assert.Throws<ArgumentException>(() => _service.Update(new Book()));
            Assert.That(ex, Is.Not.Null);
            Assert.That(ex.Message, Is.EqualTo("Cannot update book, invalid entity"));
            Assert.Pass();
        }

        [Test]
        public void UpdateBook_BookDoesNotExist_Test()
        {
            SetUpGetBook(new List<Book>());

            var ex = Assert.Throws<ArgumentException>(() => _service.Update(NewBook));
            Assert.That(ex.Message, Is.EqualTo("Cannot update book, entity is missing"));
            Assert.Pass();
        }

        [Test]
        public void UpdateBook_Success_Test()
        {
            SetUpGetBook(new List<Book>() { NewBook });

            var result = _service.Update(NewBook);

            _bookRepositoryMock.Verify(x => x.Update(It.IsAny<Book>()), Times.Once);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.Pass();
        }

        [Test]
        public void DeleteBook_BookDoesNotExist_Test()
        {
            SetUpGetBook(new List<Book>());

            var ex = Assert.Throws<ArgumentException>(() => _service.Delete(NewBook));
            Assert.That(ex.Message, Is.EqualTo("Cannot delete book, entity is missing"));
            Assert.Pass();
        }

        [Test]
        public void DeleteBook_HasBookAuthors_Test()
        {
            var book = NewBook;
            book.BookAuthors = new List<BookAuthor>() { new BookAuthor() };

            SetUpGetBook(new List<Book>() { book });

            var ex = Assert.Throws<ArgumentException>(() => _service.Delete(book));
            Assert.That(ex.Message, Is.EqualTo("Cannot delete book, entity has relations"));
            Assert.Pass();
        }


        [Test]
        public void DeleteBook_HasBookDomains_Test()
        {
            var book = NewBook;
            book.BookDomains = new List<BookDomain>() { new BookDomain() };

            SetUpGetBook(new List<Book>() { book });

            var ex = Assert.Throws<ArgumentException>(() => _service.Delete(book));
            Assert.That(ex.Message, Is.EqualTo("Cannot delete book, entity has relations"));
            Assert.Pass();
        }

        [Test]
        public void DeleteBook_HasLinkedEditions_Test()
        {
            var book = NewBook;
            book.BookEditions = new List<BookEdition>() { new BookEdition() };


            SetUpGetBook(new List<Book>() { book });

            var ex = Assert.Throws<ArgumentException>(() => _service.Delete(book, true));
            Assert.That(ex.Message, Is.EqualTo("Cannot delete book with editions, mark archived instead"));
            Assert.Pass();
        }

        [Test]
        public void DeleteBook_Success_Test()
        {
            var book = NewBook;
            book.BookAuthors = new List<BookAuthor>() { new BookAuthor() };
            book.BookDomains = new List<BookDomain>() { new BookDomain() };

            SetUpGetBook(new List<Book>() { book });
            _bookAuthorServiceMock.Setup(i => i.Delete(It.IsAny<BookAuthor>()));
            _bookDomainServiceMock.Setup(i => i.Delete(It.IsAny<BookDomain>()));

            _service.Delete(book, true);
            _bookRepositoryMock.Verify(x => x.Delete(It.IsAny<Book>()), Times.Once);
            _bookAuthorServiceMock.Verify(x => x.Delete(It.IsAny<BookAuthor>()), Times.Once);
            _bookDomainServiceMock.Verify(x => x.Delete(It.IsAny<BookDomain>()), Times.Once);
            Assert.Pass();
        }

        [Test]
        public void DeleteBook_NoLinkedEntities_Success_Test()
        {
            SetUpGetBook(new List<Book>() { NewBook });

            _service.Delete(NewBook, true);
            _bookRepositoryMock.Verify(x => x.Delete(It.IsAny<Book>()), Times.Once);
            Assert.Pass();
        }

        [Test]
        public void Delete_NoLinkedEntities_Success_Test()
        {
            SetUpGetBook(new List<Book>() { NewBook });

            _service.Delete(NewBook);
            _bookRepositoryMock.Verify(x => x.Delete(It.IsAny<Book>()), Times.Once);
            Assert.Pass();
        }

        private void SetUpGetBook(List<Book> books)
        {
            _bookRepositoryMock.Setup(i => i.Get(It.IsAny<Expression<Func<Book, bool>>>(), It.IsAny<Func<IQueryable<Book>, IOrderedQueryable<Book>>>(), It.IsAny<string>())).Returns(books);
        }

        public Book NewBook => new Book()
        {
            Title = "Title",
            YearPublication = 2020
        };
    }
}