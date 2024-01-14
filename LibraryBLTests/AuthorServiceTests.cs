//------------------------------------------------------------------------------
// <copyright file="AuthorServiceTests.cs" company="Transilvania University of Brasov">
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
    /// Defines the <see cref="AuthorServiceTests" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class AuthorServiceTests
    {
        /// <summary>
        /// Defines the logger.
        /// </summary>
        private readonly Microsoft.Extensions.Logging.ILogger<IAuthorService> logger;

        /// <summary>
        /// Defines the authorService.
        /// </summary>
        private IAuthorService authorService;

        /// <summary>
        /// Defines the authorRepositoryMock.
        /// </summary>
        private Mock<IAuthorRepository> authorRepositoryMock;

        /// <summary>
        /// Defines the bookAuthorServiceMock.
        /// </summary>
        private Mock<IBookAuthorService> bookAuthorServiceMock;

        /// <summary>
        /// Defines the this.ID.
        /// </summary>
        private int id = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorServiceTests"/> class.
        /// </summary>
        public AuthorServiceTests()
        {
            this.logger = LoggerExtensions.TestLoggingInstance<IAuthorService>();
        }

        /// <summary>
        /// The Setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.authorRepositoryMock = new Mock<IAuthorRepository>();
            this.bookAuthorServiceMock = new Mock<IBookAuthorService>();
            this.authorService = new AuthorService(this.authorRepositoryMock.Object, this.bookAuthorServiceMock.Object, this.logger);
        }

        /// <summary>
        /// The Constructor_Test.
        /// </summary>
        [Test]
        public void Constructor_Test()
        {
            this.authorService = new AuthorService(this.authorRepositoryMock.Object, this.bookAuthorServiceMock.Object, this.logger);
            Assert.That(this.authorService, Is.Not.Null);
            Assert.Pass();
        }

        /// <summary>
        /// The DeleteAuthor_InvalidId_Test.
        /// </summary>
        [Test]
        public void DeleteAuthor_InvalidId_Test()
        {
            this.authorRepositoryMock.Setup(x => x.GetByID(It.IsAny<object>())).Returns<Author>(null);

            var ex = Assert.Throws<ArgumentException>(() => this.authorService.DeleteAuthor(this.id, false));
            Assert.That(ex.Message, Is.EqualTo($"Cannot delete author with id: {this.id}, id is invalid"));
            Assert.Pass();
        }

        /// <summary>
        /// The DeleteAuthor_HasBooks_Test.
        /// </summary>
        [Test]
        public void DeleteAuthor_HasBooks_Test()
        {
            this.authorRepositoryMock.Setup(x => x.GetByID(It.IsAny<object>())).Returns(new Author());
            this.authorRepositoryMock.Setup(x => x.Get(It.IsAny<Expression<Func<Author, bool>>>(), It.IsAny<Func<IQueryable<Author>, IOrderedQueryable<Author>>>(), It.IsAny<string>())).Returns(new List<Author>() { new Author() { BookAuthors = new List<BookAuthor>() { new BookAuthor() } } });

            var ex = Assert.Throws<ArgumentException>(() => this.authorService.DeleteAuthor(this.id, false));
            Assert.That(ex.Message, Is.EqualTo($"Cannot delete author with id: {this.id}, there are books liked to it"));
            Assert.Pass();
        }

        /// <summary>
        /// The DeleteAuthor_HasBooks_HardDelete_Test.
        /// </summary>
        [Test]
        public void DeleteAuthor_HasBooks_HardDelete_Test()
        {
            this.authorRepositoryMock.Setup(x => x.GetByID(It.IsAny<object>())).Returns(new Author() { BookAuthors = new List<BookAuthor>() { new BookAuthor() } });
            this.authorRepositoryMock.Setup(x => x.Get(It.IsAny<Expression<Func<Author, bool>>>(), It.IsAny<Func<IQueryable<Author>, IOrderedQueryable<Author>>>(), It.IsAny<string>())).Returns(new List<Author>() { new Author() { BookAuthors = new List<BookAuthor>() { new BookAuthor() } } });

            this.authorService.DeleteAuthor(this.id, true);

            this.authorRepositoryMock.Verify(x => x.Delete(It.IsAny<Author>()), Times.Once);
            this.bookAuthorServiceMock.Verify(x => x.Delete(It.IsAny<BookAuthor>()), Times.Once);
            Assert.Pass();
        }

        /// <summary>
        /// The DeleteAuthor_HasNotBooks_Test.
        /// </summary>
        [Test]
        public void DeleteAuthor_HasNotBooks_Test()
        {
            this.authorRepositoryMock.Setup(x => x.GetByID(It.IsAny<object>())).Returns(new Author() { BookAuthors = new List<BookAuthor>() { new BookAuthor() } });
            this.authorRepositoryMock.Setup(x => x.Get(It.IsAny<Expression<Func<Author, bool>>>(), It.IsAny<Func<IQueryable<Author>, IOrderedQueryable<Author>>>(), It.IsAny<string>())).Returns(new List<Author>() { new Author() });

            this.authorService.DeleteAuthor(this.id, true);

            this.authorRepositoryMock.Verify(x => x.Delete(It.IsAny<Author>()), Times.Once);
            this.bookAuthorServiceMock.Verify(x => x.Delete(It.IsAny<BookAuthor>()), Times.Never);
            Assert.Pass();
        }

        /// <summary>
        /// The GetAuthorBooks_InvalidId_Test.
        /// </summary>
        [Test]
        public void GetAuthorBooks_InvalidId_Test()
        {
            var ex = Assert.Throws<ArgumentException>(() => this.authorService.GetAuthorBooks(this.id));
            Assert.That(ex.Message, Is.EqualTo($"Author doesn't exist"));
            Assert.Pass();
        }

        /// <summary>
        /// The GetAuthorBooks_ValidAuthor_Test.
        /// </summary>
        [Test]
        public void GetAuthorBooks_ValidAuthor_Test()
        {
            this.authorRepositoryMock.Setup(x => x.Get(It.IsAny<Expression<Func<Author, bool>>>(), It.IsAny<Func<IQueryable<Author>, IOrderedQueryable<Author>>>(), It.IsAny<string>())).Returns(new List<Author>() { new Author() { BookAuthors = new List<BookAuthor>() { new BookAuthor() } } });

            var result = this.authorService.GetAuthorBooks(this.id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.Pass();
        }

        /// <summary>
        /// The AddAuthor_InvalidAuthor_Test.
        /// </summary>
        [Test]
        public void AddAuthor_InvalidAuthor_FirstNameInvalid_Test()
        {
            var ex = Assert.Throws<ArgumentException>(() => this.authorService.AddAuthor(new Author()));
            Assert.That(ex.Message, Is.EqualTo("Cannot add author, invalid entity"));
            Assert.Pass();
        }

        /// <summary>
        /// The AddAuthor_InvalidAuthor_Test.
        /// </summary>
        [Test]
        public void AddAuthor_InvalidAuthor_LastNameInvalid_Test()
        {
            var ex = Assert.Throws<ArgumentException>(() => this.authorService.AddAuthor(new Author() { FirstName = "FirstName" }));
            Assert.That(ex.Message, Is.EqualTo("Cannot add author, invalid entity"));
            Assert.Pass();
        }

        /// <summary>
        /// The AddAuthor_AuthorExists_Test.
        /// </summary>
        [Test]
        public void AddAuthor_AuthorExists_Test()
        {
            var author = new Author() { FirstName = "FirstName", LastName = "LastName" };
            this.authorRepositoryMock.Setup(x => x.Get(It.IsAny<Expression<Func<Author, bool>>>(), It.IsAny<Func<IQueryable<Author>, IOrderedQueryable<Author>>>(), It.IsAny<string>())).Returns(new List<Author>() { author });

            var ex = Assert.Throws<ArgumentException>(() => this.authorService.AddAuthor(author));
            Assert.That(ex.Message, Is.EqualTo("Cannot add author, author already exists"));
            Assert.Pass();
        }

        /// <summary>
        /// The AddAuthor_Success_Test.
        /// </summary>
        [Test]
        public void AddAuthor_Success_Test()
        {
            var author = new Author() { FirstName = "FirstName", LastName = "LastName", Id = this.id };

            var result = this.authorService.AddAuthor(author);
            Assert.That(result, Is.EqualTo(this.id));
            Assert.Pass();
        }

        /// <summary>
        /// The UpdateAuthor_InvalidAuthor_Test.
        /// </summary>
        [Test]
        public void UpdateAuthor_InvalidAuthor_Test()
        {
            var ex = Assert.Throws<ArgumentException>(() => this.authorService.UpdateAuthor(new Author()));
            Assert.That(ex.Message, Is.EqualTo("Cannot update author, invalid entity"));
            Assert.Pass();
        }

        /// <summary>
        /// The UpdateAuthor_Success_Test.
        /// </summary>
        [Test]
        public void UpdateAuthor_Success_Test()
        {
            var author = new Author() { FirstName = "FirstName", LastName = "LastName", Id = this.id };

            this.authorRepositoryMock.Setup(x => x.Get(It.IsAny<Expression<Func<Author, bool>>>(), It.IsAny<Func<IQueryable<Author>, IOrderedQueryable<Author>>>(), It.IsAny<string>())).Returns(new List<Author>() { author });

            this.authorService.UpdateAuthor(author);
            Assert.Pass();
        }

        /// <summary>
        /// The UpdateAuthor_MissingAuthor_Test.
        /// </summary>
        [Test]
        public void UpdateAuthor_MissingAuthor_Test()
        {
            var author = new Author() { FirstName = "FirstName", LastName = "LastName", Id = this.id };

            this.authorRepositoryMock.Setup(x => x.Get(It.IsAny<Expression<Func<Author, bool>>>(), It.IsAny<Func<IQueryable<Author>, IOrderedQueryable<Author>>>(), It.IsAny<string>())).Returns(new List<Author>() { });

            var ex = Assert.Throws<ArgumentException>(() => this.authorService.UpdateAuthor(author));
            Assert.That(ex.Message, Is.EqualTo("Cannot update author, entity is missing"));
            Assert.Pass();
        }

    }
}
