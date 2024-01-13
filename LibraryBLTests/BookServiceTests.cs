//------------------------------------------------------------------------------
// <copyright file="BookServiceTests.cs" company="Transilvania University of Brasov">
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
    /// Defines the <see cref="BookServiceTests" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class BookServiceTests
    {
        /// <summary>
        /// Defines the service.
        /// </summary>
        private IBookService service;

        /// <summary>
        /// Defines the logger.
        /// </summary>
        private Microsoft.Extensions.Logging.ILogger<IBookService> logger;

        /// <summary>
        /// Defines the bookRepositoryMock.
        /// </summary>
        private Mock<IBookRepository> bookRepositoryMock;

        /// <summary>
        /// Defines the bookDomainServiceMock.
        /// </summary>
        private Mock<IBookDomainService> bookDomainServiceMock;

        /// <summary>
        /// Defines the bookAuthorServiceMock.
        /// </summary>
        private Mock<IBookAuthorService> bookAuthorServiceMock;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookServiceTests"/> class.
        /// </summary>
        public BookServiceTests()
        {
            this.logger = LoggerExtensions.TestLoggingInstance<IBookService>();
        }

        /// <summary>
        /// Gets the NewBook.
        /// </summary>
        public Book NewBook => new Book()
        {
            Title = "Title",
            YearPublication = 2020
        };

        /// <summary>
        /// The Setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.bookRepositoryMock = new Mock<IBookRepository>();
            this.bookDomainServiceMock = new Mock<IBookDomainService>();
            this.bookAuthorServiceMock = new Mock<IBookAuthorService>();
            this.service = new BookService(this.bookRepositoryMock.Object, this.logger, this.bookDomainServiceMock.Object, this.bookAuthorServiceMock.Object);
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
        /// The AddBook_NullArgumentException_Test.
        /// </summary>
        [Test]
        public void AddBook_NullArgumentException_Test()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => this.service.Insert(null));
            Assert.That(ex, Is.Not.Null);
            Assert.Pass();
        }

        /// <summary>
        /// The AddBook_InvalidEntity_YearPublication_Test.
        /// </summary>
        [Test]
        public void AddBook_InvalidEntity_YearPublication_Test()
        {
            var book = new Book() { Title = "1234" };

            var ex = Assert.Throws<ArgumentException>(() => this.service.Insert(book));
            Assert.That(ex.Message, Is.EqualTo("Cannot add new book, entity is invalid"));
            Assert.Pass();
        }

        /// <summary>
        /// The AddBook_InvalidEntity_Title_Test.
        /// </summary>
        [Test]
        public void AddBook_InvalidEntity_Title_Test()
        {
            var book = new Book() { YearPublication = 1 };

            var ex = Assert.Throws<ArgumentException>(() => this.service.Insert(book));
            Assert.That(ex.Message, Is.EqualTo("Cannot add new book, entity is invalid"));
            Assert.Pass();
        }

        /// <summary>
        /// The AddBook_InvalidEntity_TitleTooShort_Test.
        /// </summary>
        [Test]
        public void AddBook_InvalidEntity_TitleTooShort_Test()
        {
            var book = new Book() { YearPublication = 1, Title = "12" };

            var ex = Assert.Throws<ArgumentException>(() => this.service.Insert(book));
            Assert.That(ex.Message, Is.EqualTo("Cannot add new book, entity is invalid"));
            Assert.Pass();
        }

        /// <summary>
        /// The AddBook_InvalidEntity_TitleTooLong_Test.
        /// </summary>
        [Test]
        public void AddBook_InvalidEntity_TitleTooLong_Test()
        {
            var book = new Book() { YearPublication = 1, Title = new string('1', 256) };

            var ex = Assert.Throws<ArgumentException>(() => this.service.Insert(book));
            Assert.That(ex.Message, Is.EqualTo("Cannot add new book, entity is invalid"));
            Assert.Pass();
        }

        /// <summary>
        /// The AddBook_BookExist_Test.
        /// </summary>
        [Test]
        public void AddBook_BookExist_Test()
        {
            this.SetUpGetBook(new List<Book>() { this.NewBook });

            var ex = Assert.Throws<ArgumentException>(() => this.service.Insert(this.NewBook));
            Assert.That(ex.Message, Is.EqualTo("Cannot add new book, entity already exists"));
            Assert.Pass();
        }

        /// <summary>
        /// The AddBook_Success_Test.
        /// </summary>
        [Test]
        public void AddBook_Success_Test()
        {
            this.SetUpGetBook(new List<Book>());

            var result = this.service.Insert(this.NewBook);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.Pass();
        }

        /// <summary>
        /// The UpdateBook_NullArgumentException_Test.
        /// </summary>
        [Test]
        public void UpdateBook_NullArgumentException_Test()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => this.service.Update(null));
            Assert.That(ex, Is.Not.Null);
            Assert.Pass();
        }

        /// <summary>
        /// The UpdateBook_InvalidEntity_Test.
        /// </summary>
        [Test]
        public void UpdateBook_InvalidEntity_Test()
        {
            var ex = Assert.Throws<ArgumentException>(() => this.service.Update(new Book()));
            Assert.That(ex, Is.Not.Null);
            Assert.That(ex.Message, Is.EqualTo("Cannot update book, invalid entity"));
            Assert.Pass();
        }

        /// <summary>
        /// The UpdateBook_BookDoesNotExist_Test.
        /// </summary>
        [Test]
        public void UpdateBook_BookDoesNotExist_Test()
        {
            this.SetUpGetBook(new List<Book>());

            var ex = Assert.Throws<ArgumentException>(() => this.service.Update(this.NewBook));
            Assert.That(ex.Message, Is.EqualTo("Cannot update book, entity is missing"));
            Assert.Pass();
        }

        /// <summary>
        /// The UpdateBook_Success_Test.
        /// </summary>
        [Test]
        public void UpdateBook_Success_Test()
        {
            this.SetUpGetBook(new List<Book>() { this.NewBook });

            var result = this.service.Update(this.NewBook);

            this.bookRepositoryMock.Verify(x => x.Update(It.IsAny<Book>()), Times.Once);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.Pass();
        }

        /// <summary>
        /// The DeleteBook_BookDoesNotExist_Test.
        /// </summary>
        [Test]
        public void DeleteBook_BookDoesNotExist_Test()
        {
            this.SetUpGetBook(new List<Book>());

            var ex = Assert.Throws<ArgumentException>(() => this.service.Delete(this.NewBook));
            Assert.That(ex.Message, Is.EqualTo("Cannot delete book, entity is missing"));
            Assert.Pass();
        }

        /// <summary>
        /// The DeleteBook_HasBookAuthors_Test.
        /// </summary>
        [Test]
        public void DeleteBook_HasBookAuthors_Test()
        {
            var book = this.NewBook;
            book.BookAuthors = new List<BookAuthor>() { new BookAuthor() };

            this.SetUpGetBook(new List<Book>() { book });

            var ex = Assert.Throws<ArgumentException>(() => this.service.Delete(book));
            Assert.That(ex.Message, Is.EqualTo("Cannot delete book, entity has relations"));
            Assert.Pass();
        }

        /// <summary>
        /// The DeleteBook_HasBookDomains_Test.
        /// </summary>
        [Test]
        public void DeleteBook_HasBookDomains_Test()
        {
            var book = this.NewBook;
            book.BookDomains = new List<BookDomain>() { new BookDomain() };

            this.SetUpGetBook(new List<Book>() { book });

            var ex = Assert.Throws<ArgumentException>(() => this.service.Delete(book));
            Assert.That(ex.Message, Is.EqualTo("Cannot delete book, entity has relations"));
            Assert.Pass();
        }

        /// <summary>
        /// The DeleteBook_HasLinkedEditions_Test.
        /// </summary>
        [Test]
        public void DeleteBook_HasLinkedEditions_Test()
        {
            var book = this.NewBook;
            book.BookEditions = new List<BookEdition>() { new BookEdition() };

            this.SetUpGetBook(new List<Book>() { book });

            var ex = Assert.Throws<ArgumentException>(() => this.service.Delete(book, true));
            Assert.That(ex.Message, Is.EqualTo("Cannot delete book with editions, mark archived instead"));
            Assert.Pass();
        }

        /// <summary>
        /// The DeleteBook_Success_Test.
        /// </summary>
        [Test]
        public void DeleteBook_Success_Test()
        {
            var book = this.NewBook;
            book.BookAuthors = new List<BookAuthor>() { new BookAuthor() };
            book.BookDomains = new List<BookDomain>() { new BookDomain() };

            this.SetUpGetBook(new List<Book>() { book });
            this.bookAuthorServiceMock.Setup(i => i.Delete(It.IsAny<BookAuthor>()));
            this.bookDomainServiceMock.Setup(i => i.Delete(It.IsAny<BookDomain>()));

            this.service.Delete(book, true);
            this.bookRepositoryMock.Verify(x => x.Delete(It.IsAny<Book>()), Times.Once);
            this.bookAuthorServiceMock.Verify(x => x.Delete(It.IsAny<BookAuthor>()), Times.Once);
            this.bookDomainServiceMock.Verify(x => x.Delete(It.IsAny<BookDomain>()), Times.Once);
            Assert.Pass();
        }

        /// <summary>
        /// The DeleteBook_NoLinkedEntities_Success_Test.
        /// </summary>
        [Test]
        public void DeleteBook_NoLinkedEntities_Success_Test()
        {
            this.SetUpGetBook(new List<Book>() { this.NewBook });

            this.service.Delete(this.NewBook, true);
            this.bookRepositoryMock.Verify(x => x.Delete(It.IsAny<Book>()), Times.Once);
            Assert.Pass();
        }

        /// <summary>
        /// The Delete_NoLinkedEntities_Success_Test.
        /// </summary>
        [Test]
        public void Delete_NoLinkedEntities_Success_Test()
        {
            this.SetUpGetBook(new List<Book>() { this.NewBook });

            this.service.Delete(this.NewBook);
            this.bookRepositoryMock.Verify(x => x.Delete(It.IsAny<Book>()), Times.Once);
            Assert.Pass();
        }

        /// <summary>
        /// The SetUpGetBook.
        /// </summary>
        /// <param name="books">The books<see cref="List{Book}"/>.</param>
        private void SetUpGetBook(List<Book> books)
        {
            this.bookRepositoryMock.Setup(i => i.Get(It.IsAny<Expression<Func<Book, bool>>>(), It.IsAny<Func<IQueryable<Book>, IOrderedQueryable<Book>>>(), It.IsAny<string>())).Returns(books);
        }
    }
}
