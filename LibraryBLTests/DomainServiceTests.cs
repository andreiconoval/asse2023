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
    public class DomainServiceTests
    {
        #region Private fields

        IDomainService _domainService;
        private readonly Microsoft.Extensions.Logging.ILogger<IDomainService> _logger;
        Mock<IDomainRepository> _domainRepositoryMock;
        Mock<IBookDomainService> _bookDomainServiceMock;

        #endregion

        public DomainServiceTests()
        {
            _logger = LoggerExtensions.TestLoggingInstance<IDomainService>();
        }

        [SetUp]
        public void Setup()
        {
            _domainRepositoryMock = new Mock<IDomainRepository>();
            _bookDomainServiceMock = new Mock<IBookDomainService>();
            _domainService = new DomainService(_domainRepositoryMock.Object, _logger, _bookDomainServiceMock.Object);
        }

        [Test]
        public void Constructor_Test()
        {
            Assert.That(_domainService, Is.Not.Null);
            Assert.Pass();
        }

        [Test]
        public void AddDomain_NullArgumetException_Test()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _domainService.Insert(null));
            Assert.That(ex, Is.Not.Null);
            Assert.Pass();
        }

        [Test]
        public void AddDomain_InvalidEntity_Test()
        {
            var ex = Assert.Throws<ArgumentException>(() => _domainService.Insert(new Domain()));
            Assert.That(ex.Message, Is.EqualTo("Cannot add new domain, entity is invalid"));
            Assert.Pass();
        }

        [Test]
        public void AddDomain_DomainExist_Test()
        {
            var domain = new Domain()
            {
                DomainName = "DomainName",
                DomainId = 2
            };

            SetUpGetDomain(new List<Domain>() { domain });

            var ex = Assert.Throws<ArgumentException>(() => _domainService.Insert(domain));
            Assert.That(ex.Message, Is.EqualTo("Cannot add new domain, entity already exists"));
            Assert.Pass();
        }

        [Test]
        public void AddDomain_Success_Test()
        {
            var domain = new Domain()
            {
                DomainName = "DomainName",
                DomainId = 2
            };

            SetUpGetDomain(new List<Domain>());

            var result = _domainService.Insert(domain);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.Pass();
        }

        [Test]
        public void UpdateDomain_NullArgumetException_Test()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _domainService.Update(null));
            Assert.That(ex, Is.Not.Null);
            Assert.Pass();
        }

        [Test]
        public void UpdateDomain_InvalidEntity_Test()
        {
            var ex = Assert.Throws<ArgumentException>(() => _domainService.Update(new Domain()));
            Assert.That(ex, Is.Not.Null);
            Assert.That(ex.Message, Is.EqualTo("Cannot update domain, invalid entity"));
            Assert.Pass();
        }

        [Test]
        public void UpdateDomain_DomainDoesNotExist_Test()
        {
            var domain = new Domain()
            {
                DomainName = "DomainName",
                DomainId = 2
            };

            SetUpGetDomain(new List<Domain>());

            var ex = Assert.Throws<ArgumentException>(() => _domainService.Update(domain));
            Assert.That(ex.Message, Is.EqualTo("Cannot update domain, entity is missing"));
            Assert.Pass();
        }

        [Test]
        public void UpdateDomain_Success_Test()
        {
            var domain = new Domain()
            {
                DomainName = "DomainName",
                DomainId = 2
            };

            SetUpGetDomain(new List<Domain>() { domain });

            var result = _domainService.Update(domain);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.Pass();
        }


        [Test]
        public void DeleteDomain_DomainDoesNotExist_Test()
        {
            var domain = new Domain()
            {
                DomainName = "DomainName",
                DomainId = 2,
                Id = 1
            };

            SetUpGetDomain(new List<Domain>());

            var ex = Assert.Throws<ArgumentException>(() => _domainService.Delete(domain, false));
            Assert.That(ex.Message, Is.EqualTo("Cannot delete domain, entity is missing"));
            Assert.Pass();
        }

        [Test]
        public void DeleteDomain_HasBooks_Test()
        {
            var domain = new Domain()
            {
                DomainName = "DomainName",
                DomainId = 2,
                Id = 1,
                Subdomains = new List<Domain>() { new Domain() },
                BookDomains = new List<BookDomain>() { new BookDomain() },
            };

            SetUpGetDomain(new List<Domain>() { domain });

            var ex = Assert.Throws<ArgumentException>(() => _domainService.Delete(domain, false));
            Assert.That(ex.Message, Is.EqualTo("Cannot delete domain, entity has relations"));
            Assert.Pass();
        }

        [Test]
        public void DeleteDomain_HasBooks_HardDelete_Test()
        {
            var domain = new Domain()
            {
                DomainName = "DomainName",
                DomainId = 2,
                Id = 1,
                BookDomains = new List<BookDomain>() { new BookDomain() },
                Subdomains = new List<Domain>(),
            };

            _bookDomainServiceMock.Setup(x => x.Delete(It.IsAny<BookDomain>()));

            SetUpGetDomain(new List<Domain>() { domain });

            _domainService.Delete(domain, true);
            _domainRepositoryMock.Verify(x => x.Delete(It.IsAny<Domain>()), Times.Once);
            _bookDomainServiceMock.Verify(x => x.Delete(It.IsAny<BookDomain>()), Times.Once);
            Assert.Pass();
        }

        [Test]
        public void DeleteDomain_HasSubdomains_HardDelete_Test()
        {
            var domain = new Domain()
            {
                DomainName = "DomainName",
                DomainId = 2,
                Id = 1,
                BookDomains = new List<BookDomain>() { new BookDomain() },
                Subdomains = new List<Domain>() { new Domain() { Id = 3 } },
            };

            var subDomain = new Domain()
            {
                DomainName = "DomainName",
                DomainId = 4,
                Id = 3,
                BookDomains = new List<BookDomain>(),
                Subdomains = new List<Domain>(),
            };

            _bookDomainServiceMock.Setup(x => x.Delete(It.IsAny<BookDomain>()));

            SetUpGetDomain(new List<Domain>() { domain, subDomain });

            _domainService.Delete(domain, true);
            _domainRepositoryMock.Verify(x => x.Delete(It.IsAny<Domain>()), Times.Exactly(2));
            _bookDomainServiceMock.Verify(x => x.Delete(It.IsAny<BookDomain>()), Times.Once);
            Assert.Pass();
        }

        private void SetUpGetDomain(List<Domain> domains)
        {
            _domainRepositoryMock.Setup(i => i.Get(It.IsAny<Expression<Func<Domain, bool>>>(), It.IsAny<Func<IQueryable<Domain>, IOrderedQueryable<Domain>>>(), It.IsAny<string>())).
                Returns<Expression<Func<Domain, bool>>, Func<IQueryable<Domain>, IOrderedQueryable<Domain>>, string>((filter, orderBy, includeProperties) =>
                {
                    var users = domains;

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

    }
}
