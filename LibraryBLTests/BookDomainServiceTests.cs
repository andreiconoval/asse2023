//------------------------------------------------------------------------------
// <copyright file = "BookDomainServiceTests.cs" company = "Transilvania University of Brasov">
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
    /// Defines the <see cref = "BookDomainServiceTests" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class BookDomainServiceTests
    {
        /// <summary>
        /// Defines the this.librarySettingsServices.
        /// </summary>
        private LibrarySettingsService librarySettingsService;

        /// <summary>
        /// Defines the this.service.
        /// </summary>
        private IBookDomainService service;

        /// <summary>
        /// Defines the logger.
        /// </summary>
        private Microsoft.Extensions.Logging.ILogger<IBookDomainService> logger;

        /// <summary>
        /// Defines the this.domainRepositoryMock.
        /// </summary>
        private Mock<IDomainRepository> domainRepositoryMock;

        /// <summary>
        /// Defines the this.bookDomainRepositoryMock.
        /// </summary>
        private Mock<IBookDomainRepository> bookDomainRepositoryMock;

        /// <summary>
        /// Initializes a new instance of the <see cref = "BookDomainServiceTests"/> class.
        /// </summary>
        public BookDomainServiceTests()
        {
            this.logger = LoggerExtensions.TestLoggingInstance<IBookDomainService>();
        }

        /// <summary>
        /// Gets the NewBookDomain.
        /// </summary>
        public BookDomain NewBookDomain => new BookDomain()
        {
            BookId = 1,
            DomainId = 2,
            Book = new Book(),
            Domain = new Domain()
        };

        /// <summary>
        /// The Setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.domainRepositoryMock = new Mock<IDomainRepository>();
            this.bookDomainRepositoryMock = new Mock<IBookDomainRepository>();
            var librarySettingsRepositoryMock = new Mock<ILibrarySettingsRepository>();
            this.librarySettingsService = new LibrarySettingsService(librarySettingsRepositoryMock.Object);
            this.service = new BookDomainService(this.bookDomainRepositoryMock.Object, this.domainRepositoryMock.Object, this.librarySettingsService, this.logger);
        }

        /// <summary>
        /// The Constructor_Test.
        /// </summary>
        [Test]
        public void Constructor_Test()
        {
            Assert.That(this.service, Is.Not.Null);
            Assert.Pass();
        }

        /// <summary>
        /// The AddBookDomain_NullArgumentException_Test.
        /// </summary>
        [Test]
        public void AddBookDomain_NullArgumentException_Test()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => this.service.Insert(null));
            Assert.That(ex, Is.Not.Null);
            Assert.Pass();
        }

        /// <summary>
        /// The AddBookDomain_InvalidEntity_DomainId_Test.
        /// </summary>
        [Test]
        public void AddBookDomain_InvalidEntity_DomainId_Test()
        {
            var ex = Assert.Throws<ArgumentException>(() => this.service.Insert(new BookDomain() { BookId = 1 }));
            Assert.That(ex.Message, Is.EqualTo("Cannot add book domain, invalid entity"));
            Assert.Pass();
        }

        /// <summary>
        /// The AddBookDomain_InvalidEntity_BookId_Test.
        /// </summary>
        [Test]
        public void AddBookDomain_InvalidEntity_BookId_Test()
        {
            var ex = Assert.Throws<ArgumentException>(() => this.service.Insert(new BookDomain() { DomainId = 1 }));
            Assert.That(ex.Message, Is.EqualTo("Cannot add book domain, invalid entity"));
            Assert.Pass();
        }

        /// <summary>
        /// The AddBookDomain_BookDomainExist_Test.
        /// </summary>
        [Test]
        public void AddBookDomain_BookDomainExist_Test()
        {
            this.SetUpGetBookDomain(new List<BookDomain>() { this.NewBookDomain });

            var ex = Assert.Throws<ArgumentException>(() => this.service.Insert(this.NewBookDomain));
            Assert.That(ex.Message, Is.EqualTo("Cannot add book domain, book domain already exists"));
            Assert.Pass();
        }

        /// <summary>
        /// The AddBookDomain_BookDoesNotExist_Test.
        /// </summary>
        [Test]
        public void AddBookDomain_BookDoesNotExist_Test()
        {
            this.SetUpGetBookDomain(new List<BookDomain>());

            var ex = Assert.Throws<ArgumentException>(() => this.service.Insert(this.NewBookDomain));
            Assert.That(ex.Message, Is.EqualTo("Cannot add book domain, the domain is not valid!"));
            Assert.Pass();
        }

        /// <summary>
        /// The AddBookDomain_Success_Test.
        /// </summary>
        [Test]
        public void AddBookDomain_Success_Test()
        {
            var domains = new List<Domain>()
            {
                new Domain { Id = 2, DomainId = 1 },
                new Domain { Id = 1, DomainId = 3 }
            };

            this.SetUpGetBookDomain(new List<BookDomain>());
            this.SetUpGetDomain(domains);

            this.librarySettingsService.LibrarySettings = new LibrarySettings { MaxDomains = 4 };

            var result = this.service.Insert(this.NewBookDomain);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.Pass();
        }

        /// <summary>
        /// The AddBookDomain_SameAncestor_Test.
        /// </summary>
        [Test]
        public void AddBookDomain_SameAncestor_Test()
        {
            var bookDomains = new List<BookDomain>()
            {
                new BookDomain { BookId = 1, DomainId = 1, Domain = new Domain { Id = 1 } }
            };
            var domains = new List<Domain>()
            {
                new Domain { Id = 2, DomainId = 1 },
                new Domain { Id = 1, DomainId = 3 },
                new Domain { Id = 3, DomainId = null },
            };

            this.SetUpGetBookDomain(bookDomains);
            this.SetUpGetDomain(domains);

            var ex = Assert.Throws<ArgumentException>(() => this.service.Insert(this.NewBookDomain));
            Assert.That(ex.Message, Is.EqualTo("Cannot add book domain, the ancestor-descendant relationship is not valid!"));
            Assert.Pass();
        }

        /// <summary>
        /// The AddBookDomain_DifferentAncestor_Test.
        /// </summary>
        [Test]
        public void AddBookDomain_DifferentAncestor_Test()
        {
            var bookDomains = new List<BookDomain>()
            {
                new BookDomain
                {
                    BookId = 1,
                    DomainId = 5,
                    Domain = new Domain { Id = 5 }
                }
            };

            var domains = new List<Domain>()
            {
                new Domain { Id = 2, DomainId = 1 },
                new Domain { Id = 1, DomainId = 3 },
                new Domain { Id = 5, DomainId = 6 },
                new Domain { Id = 3, DomainId = null },
                new Domain { Id = 6, DomainId = null },
            };

            this.SetUpGetBookDomain(bookDomains);
            this.SetUpGetDomain(domains);

            this.librarySettingsService.LibrarySettings = new LibrarySettings { MaxDomains = 4 };

            var result = this.service.Insert(this.NewBookDomain);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.Pass();
        }

        /// <summary>
        /// The AddBookDomain_ExceededDomainsLimit_Test.
        /// </summary>
        [Test]
        public void AddBookDomain_ExceededDomainsLimit_Test()
        {
            var bookDomains = new List<BookDomain>()
            {
                new BookDomain
                {
                    BookId = 1,
                    DomainId = 5,
                    Domain = new Domain { Id = 5 }
                }
            };

            var domains = new List<Domain>()
            {
                new Domain { Id = 2, DomainId = 1 },
                new Domain { Id = 1, DomainId = 3 },
                new Domain { Id = 5, DomainId = 6 },
                new Domain { Id = 3, DomainId = null },
                new Domain { Id = 6, DomainId = null },
            };

            this.librarySettingsService.LibrarySettings = new LibrarySettings { MaxDomains = 1 };

            this.SetUpGetBookDomain(bookDomains);
            this.SetUpGetDomain(domains);

            var ex = Assert.Throws<ArgumentException>(() => this.service.Insert(this.NewBookDomain));
            Assert.That(ex.Message, Is.EqualTo("Cannot add book domain, the Domains limit was exceeded!"));
            Assert.Pass();
        }

        /// <summary>
        /// The DeleteBookDomain_BookDomainNull_Test.
        /// </summary>
        [Test]
        public void DeleteBookDomain_BookDomainNull_Test()
        {
            var ex = Assert.Throws<ArgumentException>(() => this.service.Delete(null));
            Assert.That(ex.Message, Is.EqualTo("Cannot delete book domain, bookDomain is null"));
            Assert.Pass();
        }

        /// <summary>
        /// The SetUpGetBookDomain.
        /// </summary>
        /// <param name = "bookDomains">The bookDomains<see cref = "List{BookDomain}"/>.</param>
        private void SetUpGetBookDomain(List<BookDomain> bookDomains)
        {
            this.bookDomainRepositoryMock.Setup(i => i.Get(It.IsAny<Expression<Func<BookDomain, bool>>>(), It.IsAny<Func<IQueryable<BookDomain>, IOrderedQueryable<BookDomain>>>(), It.IsAny<string>())).Returns(bookDomains);
        }

        /// <summary>
        /// The SetUpGetDomain.
        /// </summary>
        /// <param name = "domains">The domains<see cref = "List{Domain}"/>.</param>
        private void SetUpGetDomain(List<Domain> domains)
        {
            this.domainRepositoryMock.Setup(i => i.Get(It.IsAny<Expression<Func<Domain, bool>>>(), It.IsAny<Func<IQueryable<Domain>, IOrderedQueryable<Domain>>>(), It.IsAny<string>())).Returns(domains);
        }
    }
}
