using Library.BL.Infrastructure;
using Library.BL.Interfaces;
using Library.BL.Services;
using Library.DAL.DomainModel;
using Library.DAL.Interfaces;
using Library.DAL.Repositories;
using Moq;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace LibraryBLTests
{
    [ExcludeFromCodeCoverage]
    public class BookDomainServiceTests
    {
        #region Private fields

        IBookDomainService _service;
        private readonly Microsoft.Extensions.Logging.ILogger<IBookDomainService> _logger;
        Mock<IDomainRepository> _domainRepositoryMock;
        Mock<IBookDomainRepository> _bookDomainRepositoryMock;

        #endregion

        public BookDomainServiceTests()
        {
            _logger = LoggerExtensions.TestLoggingInstance<IBookDomainService>();
        }

        [SetUp]
        public void Setup()
        {
            _domainRepositoryMock = new Mock<IDomainRepository>();
            _bookDomainRepositoryMock = new Mock<IBookDomainRepository>();
            _service = new BookDomainService(_bookDomainRepositoryMock.Object, _domainRepositoryMock.Object, _logger);
        }

        [Test]
        public void Constructor_Test()
        {
            Assert.That(_service, Is.Not.Null);
            Assert.Pass();
        }

        [Test]
        public void AddBookDomain_NullArgumetException_Test()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _service.Insert(null));
            Assert.That(ex, Is.Not.Null);
            Assert.Pass();
        }

        [Test]
        public void AddBookDomain_InvalidEntity_Test()
        {
            var ex = Assert.Throws<ArgumentException>(() => _service.Insert(new BookDomain()));
            Assert.That(ex.Message, Is.EqualTo("Cannot add book domain, invalid entity"));
            Assert.Pass();
        }

        [Test]
        public void AddBookDomain_BookDomainExist_Test()
        {
            SetUpGetBookDomain(new List<BookDomain>() { NewBookDomain });

            var ex = Assert.Throws<ArgumentException>(() => _service.Insert(NewBookDomain));
            Assert.That(ex.Message, Is.EqualTo("Cannot add book domain, book domain already exists"));
            Assert.Pass();
        }

        [Test]
        public void AddBookDomain_BookDoesNotExist_Test()
        {
            SetUpGetBookDomain(new List<BookDomain>());

            var ex = Assert.Throws<ArgumentException>(() => _service.Insert(NewBookDomain));
            Assert.That(ex.Message, Is.EqualTo("Cannot add book domain, the domain is not valid!"));
            Assert.Pass();
        }

        [Test]
        public void AddBookDomain_Success_Test()
        {
            var bookDomains = new List<BookDomain>()
            {
                new BookDomain { BookId=1, DomainId=1 }
            };
            var domains = new List<Domain>()
            {
                new Domain { Id=2, DomainId=1 },
                new Domain { Id=1, DomainId=3 }
            };

            SetUpGetBookDomain(new List<BookDomain>());
            SetUpGetDomain(domains);

            var result = _service.Insert(NewBookDomain);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.Pass();
        }

        [Test]
        public void AddBookDomain_SameAncestor_Test()
        {
            var bookDomains = new List<BookDomain>()
            {
                new BookDomain { BookId=1, DomainId=1, Domain= new Domain{Id= 1 } }
            };
            var domains = new List<Domain>()
            {
                new Domain { Id=2, DomainId=1 },
                new Domain { Id=1, DomainId=3 },
                new Domain { Id=3, DomainId=null },
            };

            SetUpGetBookDomain(bookDomains);
            SetUpGetDomain(domains);

            var ex = Assert.Throws<ArgumentException>(() => _service.Insert(NewBookDomain));
            Assert.That(ex.Message, Is.EqualTo("Cannot add book domain, the ancestor-descendant relationship is not valid!"));
            Assert.Pass();
        }

        [Test]
        public void AddBookDomain_DiferentAncestor_Test()
        {
            var bookDomains = new List<BookDomain>()
            {
                new BookDomain { BookId=1, DomainId=5, Domain= new Domain{Id= 5 } }
            };
            var domains = new List<Domain>()
            {
                new Domain { Id=2, DomainId=1 },
                new Domain { Id=1, DomainId=3 },
                new Domain { Id=5, DomainId=6 },
                new Domain { Id=3, DomainId=null },
                new Domain { Id=6, DomainId=null },

            };

            SetUpGetBookDomain(bookDomains);
            SetUpGetDomain(domains);

            var result = _service.Insert(NewBookDomain);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.Pass();
        }

        private void SetUpGetBookDomain(List<BookDomain> bookDomains)
        {
            _bookDomainRepositoryMock.Setup(i => i.Get(It.IsAny<Expression<Func<BookDomain, bool>>>(), It.IsAny<Func<IQueryable<BookDomain>, IOrderedQueryable<BookDomain>>>(), It.IsAny<string>())).Returns(bookDomains);
        }

        private void SetUpGetDomain(List<Domain> domains)
        {
            _domainRepositoryMock.Setup(i => i.Get(It.IsAny<Expression<Func<Domain, bool>>>(), It.IsAny<Func<IQueryable<Domain>, IOrderedQueryable<Domain>>>(), It.IsAny<string>())).Returns(domains);
        }


        public BookDomain NewBookDomain => new BookDomain()
        {
            BookId = 1,
            DomainId = 2
        };
    }
}
