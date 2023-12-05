using Library.BL.Infrastructure;
using Library.BL.Interfaces;
using Library.BL.Services;
using Library.DAL.DomainModel;
using Library.DAL.Interfaces;
using Moq;
using System.Linq.Expressions;

namespace LibraryBLTests
{
    public class AuthorUnitTests
    {
        IAuthorService _authorService;
        private readonly Microsoft.Extensions.Logging.ILogger<IAuthorService> _logger;
        Mock<IAuthorRepository> _authorRepositoryMock;
        Mock<IBookAuthorService> _bookAuthorServiceMock;

        #region

        private int ID = 1;

        #endregion

        public AuthorUnitTests()
        {
            _logger = LoggerExtensions.TestLoggingInstance<IAuthorService>();
        }

        [SetUp]
        public void Setup()
        {
            _authorRepositoryMock = new Mock<IAuthorRepository>();
            _bookAuthorServiceMock = new Mock<IBookAuthorService>();
            _authorService = new AuthorService(_authorRepositoryMock.Object, _bookAuthorServiceMock.Object, _logger);
        }

        [Test]
        public void Constructor_Test()
        {
            _authorService = new AuthorService(_authorRepositoryMock.Object, _bookAuthorServiceMock.Object, _logger);
            Assert.That(_authorService, Is.Not.Null);
            Assert.Pass();
        }



        [Test]
        public void DeleteAuthor_InvalidId_Test()
        {
            _authorRepositoryMock.Setup(x => x.GetByID(It.IsAny<object>())).Returns<Author>(null);

            var ex = Assert.Throws<ArgumentException>(() => _authorService.DeleteAuthor(ID, false));
            Assert.That(ex.Message, Is.EqualTo($"Cannot delete author with id: {ID}, id is invalid"));
            Assert.Pass();
        }

        [Test]
        public void DeleteAuthor_HasBooks_Test()
        {
            _authorRepositoryMock.Setup(x => x.GetByID(It.IsAny<object>())).Returns(new Author());
            _authorRepositoryMock.Setup(x => x.Get(It.IsAny<Expression<Func<Author, bool>>>(), It.IsAny<Func<IQueryable<Author>, IOrderedQueryable<Author>>>(), It.IsAny<string>())).Returns(new List<Author>() { new Author() { BookAuthors = new List<BookAuthor>() { new BookAuthor() } } });

            var ex = Assert.Throws<ArgumentException>(() => _authorService.DeleteAuthor(ID, false));
            Assert.That(ex.Message, Is.EqualTo($"Cannot delete author with id: {ID}, there are books liked to it"));
            Assert.Pass();
        }

        [Test]
        public void DeleteAuthor_HasBooks_HardDelete_Test()
        {
            _authorRepositoryMock.Setup(x => x.GetByID(It.IsAny<object>())).Returns(new Author() { BookAuthors = new List<BookAuthor>() { new BookAuthor() } });
            _authorRepositoryMock.Setup(x => x.Get(It.IsAny<Expression<Func<Author, bool>>>(), It.IsAny<Func<IQueryable<Author>, IOrderedQueryable<Author>>>(), It.IsAny<string>())).Returns(new List<Author>() { new Author() { BookAuthors = new List<BookAuthor>() { new BookAuthor() } } });

            _authorService.DeleteAuthor(ID, true);

            _authorRepositoryMock.Verify(x => x.Delete(It.IsAny<Author>()), Times.Once);
            _bookAuthorServiceMock.Verify(x => x.Delete(It.IsAny<BookAuthor>()), Times.Once);
            Assert.Pass();
        }

        [Test]
        public void DeleteAuthor_HasntBooks_Test()
        {
            _authorRepositoryMock.Setup(x => x.GetByID(It.IsAny<object>())).Returns(new Author() { BookAuthors = new List<BookAuthor>() { new BookAuthor() } });
            _authorRepositoryMock.Setup(x => x.Get(It.IsAny<Expression<Func<Author, bool>>>(), It.IsAny<Func<IQueryable<Author>, IOrderedQueryable<Author>>>(), It.IsAny<string>())).Returns(new List<Author>() { new Author() });

            _authorService.DeleteAuthor(ID, true);

            _authorRepositoryMock.Verify(x => x.Delete(It.IsAny<Author>()), Times.Once);
            _bookAuthorServiceMock.Verify(x => x.Delete(It.IsAny<BookAuthor>()), Times.Never);
            Assert.Pass();
        }

        [Test]
        public void GetAuthorBooks_InvalidId_Test()
        {
            var ex = Assert.Throws<ArgumentException>(() => _authorService.GetAuthorBooks(ID));
            Assert.That(ex.Message, Is.EqualTo($"Author doesn't exist"));
            Assert.Pass();
        }

        [Test]
        public void GetAuthorBooks_ValidAuthor_Test()
        {
            _authorRepositoryMock.Setup(x => x.Get(It.IsAny<Expression<Func<Author, bool>>>(), It.IsAny<Func<IQueryable<Author>, IOrderedQueryable<Author>>>(), It.IsAny<string>())).Returns(new List<Author>() { new Author() { BookAuthors = new List<BookAuthor>() { new BookAuthor() } } });

            var result = _authorService.GetAuthorBooks(ID);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.Pass();
        }

        [Test]
        public void AddAuthor_InvalidAuthor_Test()
        {
            var ex = Assert.Throws<ArgumentException>(() => _authorService.AddAuthor(new Author()));
            Assert.That(ex.Message, Is.EqualTo("Cannot add author, invalid entity"));
            Assert.Pass();
        }

        [Test]
        public void AddAuthor_AuthorExists_Test()
        {
            var author = new Author() { FirstName = "FirstName", LastName = "LastName" };
            _authorRepositoryMock.Setup(x => x.Get(It.IsAny<Expression<Func<Author, bool>>>(), It.IsAny<Func<IQueryable<Author>, IOrderedQueryable<Author>>>(), It.IsAny<string>())).Returns(new List<Author>() { author });

            var ex = Assert.Throws<ArgumentException>(() => _authorService.AddAuthor(author));
            Assert.That(ex.Message, Is.EqualTo("Cannot add author, author already exists"));
            Assert.Pass();
        }

        [Test]
        public void AddAuthor_Success_Test()
        {
            var author = new Author() { FirstName = "FirstName", LastName = "LastName", Id = ID };

            var result = _authorService.AddAuthor(author);
            Assert.That(result, Is.EqualTo(ID));
            Assert.Pass();
        }
    }
}
