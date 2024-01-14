//------------------------------------------------------------------------------
// <copyright file="DomainServiceTests.cs" company="Transilvania University of Brasov">
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
    using Library.BL.Infrastructure;
    using Library.BL.Interfaces;
    using Library.BL.Services;
    using Library.DAL.DomainModel;
    using Library.DAL.Interfaces;
    using Moq;
    using NUnit.Framework;

    /// <summary>
    /// Defines the <see cref="DomainServiceTests" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class DomainServiceTests
    {
        /// <summary>
        /// Defines the domainService.
        /// </summary>
        private IDomainService domainService;

        /// <summary>
        /// Defines the logger.
        /// </summary>
        private Microsoft.Extensions.Logging.ILogger<IDomainService> logger;

        /// <summary>
        /// Defines the domainRepositoryMock.
        /// </summary>
        private Mock<IDomainRepository> domainRepositoryMock;

        /// <summary>
        /// Defines the bookDomainServiceMock.
        /// </summary>
        private Mock<IBookDomainService> bookDomainServiceMock;

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainServiceTests"/> class.
        /// </summary>
        public DomainServiceTests()
        {
            this.logger = LoggerExtensions.TestLoggingInstance<IDomainService>();
        }

        /// <summary>
        /// The Setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.domainRepositoryMock = new Mock<IDomainRepository>();
            this.bookDomainServiceMock = new Mock<IBookDomainService>();
            this.domainService = new DomainService(this.domainRepositoryMock.Object, this.logger, this.bookDomainServiceMock.Object);
        }

        /// <summary>
        /// The Constructor_Test.
        /// </summary>
        [Test]
        public void Constructor_Test()
        {
            Assert.That(this.domainService, Is.Not.Null);
            Assert.Pass();
        }

        /// <summary>
        /// The AddDomain_NullArgumentException_Test.
        /// </summary>
        [Test]
        public void AddDomain_NullArgumentException_Test()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => this.domainService.Insert(null));
            Assert.That(ex, Is.Not.Null);
            Assert.Pass();
        }

        /// <summary>
        /// The AddDomain_InvalidDomainId_Test.
        /// </summary>
        [Test]
        public void AddDomain_InvalidDomainId_Test()
        {
            var ex = Assert.Throws<ArgumentException>(() => this.domainService.Insert(new Domain() { DomainName = "DomainName" }));
            Assert.That(ex.Message, Is.EqualTo("Cannot add new domain, entity is invalid"));
            Assert.Pass();
        }

        /// <summary>
        /// The AddDomain_InvalidDomainIdEqualToId_Test.
        /// </summary>
        [Test]
        public void AddDomain_InvalidDomainIdEqualToId_Test()
        {
            var ex = Assert.Throws<ArgumentException>(() => this.domainService.Insert(new Domain() { DomainName = "DomainName", DomainId = 1, Id = 1 }));
            Assert.That(ex.Message, Is.EqualTo("Cannot add new domain, entity is invalid"));
            Assert.Pass();
        }

        /// <summary>
        /// The AddDomain_InvalidDomainName_Test.
        /// </summary>
        [Test]
        public void AddDomain_InvalidDomainName_Test()
        {
            var domain = new Domain
            {
                DomainName = "",
                DomainId = 1,
                Id = 1
            };

            var ex = Assert.Throws<ArgumentException>(() => this.domainService.Insert(domain));
            Assert.That(ex.Message, Is.EqualTo("Cannot add new domain, entity is invalid"));
            Assert.Pass();
        }

        /// <summary>
        /// The AddDomain_InvalidDomainNameTooShort_Test.
        /// </summary>
        [Test]
        public void AddDomain_InvalidDomainNameTooShort_Test()
        {
            var domain = new Domain
            {
                DomainName = "12",
                DomainId = 1,
                Id = 1
            };

            var ex = Assert.Throws<ArgumentException>(() => this.domainService.Insert(domain));
            Assert.That(ex.Message, Is.EqualTo("Cannot add new domain, entity is invalid"));
            Assert.Pass();
        }

        /// <summary>
        /// The AddDomain_InvalidDomainNameTooLong_Test.
        /// </summary>
        [Test]
        public void AddDomain_InvalidDomainNameTooLong_Test()
        {
            var domain = new Domain
            {
                DomainName = new string('1', 256),
                DomainId = 1,
                Id = 1
            };

            var ex = Assert.Throws<ArgumentException>(() => this.domainService.Insert(domain));
            Assert.That(ex.Message, Is.EqualTo("Cannot add new domain, entity is invalid"));
            Assert.Pass();
        }

        /// <summary>
        /// The AddDomain_DomainExist_Test.
        /// </summary>
        [Test]
        public void AddDomain_DomainExist_Test()
        {
            var domain = new Domain()
            {
                DomainName = "DomainName",
                DomainId = 2,
                ParentDomain = new Domain(),
                Subdomains = new List<Domain>()
            };

            this.SetUpGetDomain(new List<Domain>() { domain });

            var ex = Assert.Throws<ArgumentException>(() => this.domainService.Insert(domain));
            Assert.That(ex.Message, Is.EqualTo("Cannot add new domain, entity already exists"));
            Assert.Pass();
        }

        /// <summary>
        /// The AddDomain_Success_Test.
        /// </summary>
        [Test]
        public void AddDomain_Success_Test()
        {
            var domain = new Domain()
            {
                DomainName = "DomainName",
                DomainId = 2
            };

            this.SetUpGetDomain(new List<Domain>());

            var result = this.domainService.Insert(domain);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.Pass();
        }

        /// <summary>
        /// The UpdateDomain_NullArgumentException_Test.
        /// </summary>
        [Test]
        public void UpdateDomain_NullArgumentException_Test()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => this.domainService.Update(null));
            Assert.That(ex, Is.Not.Null);
            Assert.Pass();
        }

        /// <summary>
        /// The UpdateDomain_InvalidEntity_Test.
        /// </summary>
        [Test]
        public void UpdateDomain_InvalidEntity_Test()
        {
            var ex = Assert.Throws<ArgumentException>(() => this.domainService.Update(new Domain()));
            Assert.That(ex, Is.Not.Null);
            Assert.That(ex.Message, Is.EqualTo("Cannot update domain, invalid entity"));
            Assert.Pass();
        }

        /// <summary>
        /// The UpdateDomain_DomainDoesNotExist_Test.
        /// </summary>
        [Test]
        public void UpdateDomain_DomainDoesNotExist_Test()
        {
            var domain = new Domain()
            {
                DomainName = "DomainName",
                DomainId = 2
            };

            this.SetUpGetDomain(new List<Domain>());

            var ex = Assert.Throws<ArgumentException>(() => this.domainService.Update(domain));
            Assert.That(ex.Message, Is.EqualTo("Cannot update domain, entity is missing"));
            Assert.Pass();
        }

        /// <summary>
        /// The UpdateDomain_Success_Test.
        /// </summary>
        [Test]
        public void UpdateDomain_Success_Test()
        {
            var domain = new Domain()
            {
                DomainName = "DomainName",
                DomainId = 2
            };

            this.SetUpGetDomain(new List<Domain>() { domain });

            var result = this.domainService.Update(domain);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.Pass();
        }

        /// <summary>
        /// The DeleteDomain_DomainDoesNotExist_Test.
        /// </summary>
        [Test]
        public void DeleteDomain_DomainDoesNotExist_Test()
        {
            var domain = new Domain()
            {
                DomainName = "DomainName",
                DomainId = 2,
                Id = 1
            };

            this.SetUpGetDomain(new List<Domain>());

            var ex = Assert.Throws<ArgumentException>(() => this.domainService.Delete(domain, false));
            Assert.That(ex.Message, Is.EqualTo("Cannot delete domain, entity is missing"));
            Assert.Pass();
        }

        /// <summary>
        /// The DeleteDomain_DomainNull_Test.
        /// </summary>
        [Test]
        public void DeleteDomain_DomainNull_Test()
        {
            Domain domain = null;
            var ex = Assert.Throws<ArgumentException>(() => this.domainService.Delete(domain, false));
            Assert.That(ex.Message, Is.EqualTo("Cannot delete domain, domain is missing"));
            Assert.Pass();
        }

        /// <summary>
        /// The DeleteDomain_HasBooks_Test.
        /// </summary>
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

            this.SetUpGetDomain(new List<Domain>() { domain });

            var ex = Assert.Throws<ArgumentException>(() => this.domainService.Delete(domain, false));
            Assert.That(ex.Message, Is.EqualTo("Cannot delete domain, entity has relations"));
            Assert.Pass();
        }

        /// <summary>
        /// The DeleteDomain_HasBooks_HardDelete_Test.
        /// </summary>
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

            this.bookDomainServiceMock.Setup(x => x.Delete(It.IsAny<BookDomain>()));

            this.SetUpGetDomain(new List<Domain>() { domain });

            this.domainService.Delete(domain, true);
            this.domainRepositoryMock.Verify(x => x.Delete(It.IsAny<Domain>()), Times.Once);
            this.bookDomainServiceMock.Verify(x => x.Delete(It.IsAny<BookDomain>()), Times.Once);
            Assert.Pass();
        }

        /// <summary>
        /// The DeleteDomain_HasSubdomains_HardDelete_Test.
        /// </summary>
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

            this.bookDomainServiceMock.Setup(x => x.Delete(It.IsAny<BookDomain>()));

            this.SetUpGetDomain(new List<Domain>() { domain, subDomain });

            this.domainService.Delete(domain, true);
            this.domainRepositoryMock.Verify(x => x.Delete(It.IsAny<Domain>()), Times.Exactly(2));
            this.bookDomainServiceMock.Verify(x => x.Delete(It.IsAny<BookDomain>()), Times.Once);
            Assert.Pass();
        }

        /// <summary>
        /// The SetUpGetDomain.
        /// </summary>
        /// <param name="domains">The domains<see cref="List{Domain}"/>.</param>
        private void SetUpGetDomain(List<Domain> domains)
        {
            this.domainRepositoryMock.Setup(i => i.Get(It.IsAny<Expression<Func<Domain, bool>>>(), It.IsAny<Func<IQueryable<Domain>, IOrderedQueryable<Domain>>>(), It.IsAny<string>()))
                .Returns<Expression<Func<Domain, bool>>, Func<IQueryable<Domain>, IOrderedQueryable<Domain>>, string>((filter, orderBy, includeProperties) =>
                {
                    var users = domains;

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
    }
}
