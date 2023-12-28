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
    public class BookAuthorServiceTests
    {
        #region Private fields

        IBookAuthorService _service;
        private readonly Microsoft.Extensions.Logging.ILogger<IBookAuthorService> _logger;
        Mock<IBookAuthorRepository> _repositoryMock;
        Mock<IAuthorRepository> _authorRepositoryMock;
        Mock<IBookRepository> _bookRepositoryMock;

        #endregion

        public BookAuthorServiceTests()
        {
            _logger = LoggerExtensions.TestLoggingInstance<IBookAuthorService>();
        }

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<IBookAuthorRepository>();
            _authorRepositoryMock = new Mock<IAuthorRepository>();
            _bookRepositoryMock = new Mock<IBookRepository>();
            _service = new BookAuthorService(_repositoryMock.Object, _authorRepositoryMock.Object, _bookRepositoryMock.Object, _logger);
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
            var ex = Assert.Throws<ArgumentException>(() => _service.Insert(new BookAuthor()));
            Assert.That(ex.Message, Is.EqualTo("Cannot add book author, invalid entity"));
            Assert.Pass();
        }

        [Test]
        public void Insert_BookAuthorExist_Test()
        {
            SetUpGetBookAuthor(new List<BookAuthor>() { NewBookAuthor });

            var ex = Assert.Throws<ArgumentException>(() => _service.Insert(NewBookAuthor));
            Assert.That(ex.Message, Is.EqualTo("Cannot add book author, book author already exists"));
            Assert.Pass();
        }

        [Test]
        public void Insert_BookNotexist_Test()
        {
            SetUpGetBook(new List<Book>());

            var ex = Assert.Throws<ArgumentException>(() => _service.Insert(NewBookAuthor));
            Assert.That(ex.Message, Is.EqualTo("Cannot add book author, book does not exist"));
            Assert.Pass();
        }

        [Test]
        public void Insert_AutrhorNotexist_Test()
        {
            SetUpGetBook(new List<Book>() { new Book() });
            SetUpGetAuthor(new List<Author>());

            var ex = Assert.Throws<ArgumentException>(() => _service.Insert(NewBookAuthor));
            Assert.That(ex.Message, Is.EqualTo("Cannot add book author, author does not exist"));
            Assert.Pass();
        }

        [Test]
        public void Insert_InvalidException_Test()
        {
            SetUpGetBook(new List<Book>() { new Book() });
            SetUpGetAuthor(new List<Author>() { new Author() });

            _repositoryMock.Setup(i => i.Insert(It.IsAny<BookAuthor>())).Throws(new Exception("Invalid operation"));

            var ex = Assert.Throws<Exception>(() => _service.Insert(NewBookAuthor));
            Assert.That(ex.Message, Is.EqualTo("Invalid operation"));
            Assert.Pass();
        }

        [Test]
        public void Insert_Success_Test()
        {
            SetUpGetBook(new List<Book>() { new Book() });
            SetUpGetAuthor(new List<Author>() { new Author() });

            var result = _service.Insert(NewBookAuthor);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.Pass();
        }


        [Test]
        public void Delete_Success_Test()
        {
            _service.Delete(NewBookAuthor);
            Assert.That(_repositoryMock.Invocations.Count, Is.EqualTo(1));
            _repositoryMock.Verify(x => x.Delete(It.IsAny<BookAuthor>()), Times.Once);
            Assert.Pass();
        }

        [Test]
        public void Update_ThrowException_Test()
        {
            var ex = Assert.Throws<Exception>(() => _service.Update(NewBookAuthor));
            Assert.That(ex.Message, Is.EqualTo("To delete and add new Book author is the best approach"));
            Assert.Pass();
        }

        private void SetUpGetBookAuthor(List<BookAuthor> bookAuthors)
        {
            _repositoryMock.Setup(i => i.Get(It.IsAny<Expression<Func<BookAuthor, bool>>>(), It.IsAny<Func<IQueryable<BookAuthor>, IOrderedQueryable<BookAuthor>>>(), It.IsAny<string>()))
                .Returns<Expression<Func<BookAuthor, bool>>, Func<IQueryable<BookAuthor>, IOrderedQueryable<BookAuthor>>, string>((filter, orderBy, includeProperties) =>
                {
                    var users = bookAuthors;

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

        private void SetUpGetAuthor(List<Author> authors)
        {
            _authorRepositoryMock.Setup(i => i.Get(It.IsAny<Expression<Func<Author, bool>>>(), It.IsAny<Func<IQueryable<Author>, IOrderedQueryable<Author>>>(), It.IsAny<string>()))
                .Returns(authors);
        }

        private void SetUpGetBook(List<Book> books)
        {
            _bookRepositoryMock.Setup(i => i.Get(It.IsAny<Expression<Func<Book, bool>>>(), It.IsAny<Func<IQueryable<Book>, IOrderedQueryable<Book>>>(), It.IsAny<string>()))
                .Returns(books);
        }

        public BookAuthor NewBookAuthor => new BookAuthor()
        {
            BookId = 1,
            AuthorId = 1
        };
    }
}
