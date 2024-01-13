//------------------------------------------------------------------------------
// <copyright file="BookEditionServiceTests.cs" company="Transilvania University of Brasov">
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
    using System.Net;
    using Library.BL.Infrastructure;
    using Library.BL.Interfaces;
    using Library.BL.Services;
    using Library.DAL.DomainModel;
    using Library.DAL.Interfaces;
    using Moq;
    using NUnit.Framework;

    /// <summary>
    /// Defines the <see cref="BookEditionServiceTests" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class BookEditionServiceTests
    {
        /// <summary>
        /// Defines the service.
        /// </summary>
        private IBookEditionService service;

        /// <summary>
        /// Defines the logger.
        /// </summary>
        private Microsoft.Extensions.Logging.ILogger<IBookEditionService> logger;

        /// <summary>
        /// Defines the bookRepositoryMock.
        /// </summary>
        private Mock<IBookRepository> bookRepositoryMock;

        /// <summary>
        /// Defines the this.bookEditionRepositoryMock.
        /// </summary>
        private Mock<IBookEditionRepository> bookEditionRepositoryMock;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookEditionServiceTests"/> class.
        /// </summary>
        public BookEditionServiceTests()
        {
            this.logger = LoggerExtensions.TestLoggingInstance<IBookEditionService>();
        }

        /// <summary>
        /// Gets the this.NewBookEdition.
        /// </summary>
        public BookEdition NewBookEdition => new BookEdition()
        {
            BookId = 1,
            BookType = "Type",
            Id = 1,
            PageNumber = 1,
            Edition = "Edition",
            ReleaseYear = 2023
        };

        /// <summary>
        /// The Setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.bookRepositoryMock = new Mock<IBookRepository>();
            this.bookEditionRepositoryMock = new Mock<IBookEditionRepository>();
            this.service = new BookEditionService(this.bookEditionRepositoryMock.Object, this.logger, this.bookRepositoryMock.Object);
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
        /// The AddBookEdition_NullArgumentException_Test.
        /// </summary>
        [Test]
        public void AddBookEdition_NullArgumentException_Test()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => this.service.Insert(null));
            Assert.That(ex, Is.Not.Null);
            Assert.Pass();
        }

        /// <summary>
        /// The AddBookEdition_InvalidEntity_Test.
        /// </summary>
        [Test]
        public void AddBookEdition_InvalidEntity_Test()
        {
            var ex = Assert.Throws<ArgumentException>(() => this.service.Insert(new BookEdition()));
            Assert.That(ex.Message, Is.EqualTo("Cannot add new book edition, entity is invalid"));
            Assert.Pass();
        }

        /// <summary>
        /// The AddBookEdition_InvalidBookId_Test.
        /// </summary>
        [Test]
        public void AddBookEdition_InvalidBookId_Test()
        {
            var bookEdition = this.NewBookEdition;
            bookEdition.BookId = 0;

            var ex = Assert.Throws<ArgumentException>(() => this.service.Insert(bookEdition));
            Assert.That(ex.Message, Is.EqualTo("Cannot add new book edition, entity is invalid"));
            Assert.Pass();
        }

        /// <summary>
        /// The AddBookEdition_InvalidPageNumber_Test.
        /// </summary>
        [Test]
        public void AddBookEdition_InvalidPageNumber_Test()
        {
            var bookEdition = this.NewBookEdition;
            bookEdition.PageNumber = 0;

            var ex = Assert.Throws<ArgumentException>(() => this.service.Insert(bookEdition));
            Assert.That(ex.Message, Is.EqualTo("Cannot add new book edition, entity is invalid"));
            Assert.Pass();
        }

        /// <summary>
        /// The AddBookEdition_InvalidBookType_Test.
        /// </summary>
        [Test]
        public void AddBookEdition_InvalidBookType_Test()
        {
            var bookEdition = this.NewBookEdition;
            bookEdition.BookType = "";

            var ex = Assert.Throws<ArgumentException>(() => this.service.Insert(bookEdition));
            Assert.That(ex.Message, Is.EqualTo("Cannot add new book edition, entity is invalid"));
            Assert.Pass();
        }

        /// <summary>
        /// The AddBookEdition_InvalidBookTypeTooLong_Test.
        /// </summary>
        [Test]
        public void AddBookEdition_InvalidBookTypeTooLong_Test()
        {
            var bookEdition = this.NewBookEdition;
            bookEdition.BookType = new string('a', 256);

            var ex = Assert.Throws<ArgumentException>(() => this.service.Insert(bookEdition));
            Assert.That(ex.Message, Is.EqualTo("Cannot add new book edition, entity is invalid"));
            Assert.Pass();
        }

        /// <summary>
        /// The AddBookEdition_InvalidEdition_Test.
        /// </summary>
        [Test]
        public void AddBookEdition_InvalidEdition_Test()
        {
            var bookEdition = this.NewBookEdition;
            bookEdition.Edition = "";

            var ex = Assert.Throws<ArgumentException>(() => this.service.Insert(bookEdition));
            Assert.That(ex.Message, Is.EqualTo("Cannot add new book edition, entity is invalid"));
            Assert.Pass();
        }

        /// <summary>
        /// The AddBookEdition_InvalidEditionTooLong_Test.
        /// </summary>
        [Test]
        public void AddBookEdition_InvalidEditionTooLong_Test()
        {
            var bookEdition = this.NewBookEdition;
            bookEdition.Edition = new string('a', 256);

            var ex = Assert.Throws<ArgumentException>(() => this.service.Insert(bookEdition));
            Assert.That(ex.Message, Is.EqualTo("Cannot add new book edition, entity is invalid"));
            Assert.Pass();
        }

        /// <summary>
        /// The AddBookEdition_InvalidReleaseYear_Test.
        /// </summary>
        [Test]
        public void AddBookEdition_InvalidReleaseYear_Test()
        {
            var bookEdition = this.NewBookEdition;
            bookEdition.ReleaseYear = -1;

            var ex = Assert.Throws<ArgumentException>(() => this.service.Insert(bookEdition));
            Assert.That(ex.Message, Is.EqualTo("Cannot add new book edition, entity is invalid"));
            Assert.Pass();
        }

        /// <summary>
        /// The AddBookEdition_BookEditionExist_Test.
        /// </summary>
        [Test]
        public void AddBookEdition_BookEditionExist_Test()
        {
            this.SetUpGetBookEdition(new List<BookEdition>() { this.NewBookEdition });

            var ex = Assert.Throws<ArgumentException>(() => this.service.Insert(this.NewBookEdition));
            Assert.That(ex.Message, Is.EqualTo("Cannot add new book edition, entity already exists"));
            Assert.Pass();
        }

        /// <summary>
        /// The AddBookEdition_BookDoesNotExist_Test.
        /// </summary>
        [Test]
        public void AddBookEdition_BookDoesNotExist_Test()
        {
            this.SetUpGetBookEdition(new List<BookEdition>());

            var ex = Assert.Throws<ArgumentException>(() => this.service.Insert(this.NewBookEdition));
            Assert.That(ex.Message, Is.EqualTo("Cannot add new book edition, book is missing"));
            Assert.Pass();
        }

        /// <summary>
        /// The AddBookEdition_Success_Test.
        /// </summary>
        [Test]
        public void AddBookEdition_Success_Test()
        {
            this.SetUpGetBookEdition(new List<BookEdition>());
            this.SetUpGetBook(new List<Book>() { new Book() });

            var result = this.service.Insert(this.NewBookEdition);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.Pass();
        }

        /// <summary>
        /// The UpdateBookEdition_NullArgumentException_Test.
        /// </summary>
        [Test]
        public void UpdateBookEdition_NullArgumentException_Test()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => this.service.Update(null));
            Assert.That(ex, Is.Not.Null);
            Assert.Pass();
        }

        /// <summary>
        /// The UpdateBookEdition_InvalidEntity_Test.
        /// </summary>
        [Test]
        public void UpdateBookEdition_InvalidEntity_Test()
        {
            var ex = Assert.Throws<ArgumentException>(() => this.service.Update(new BookEdition()));
            Assert.That(ex, Is.Not.Null);
            Assert.That(ex.Message, Is.EqualTo("Cannot update book edition, invalid entity"));
            Assert.Pass();
        }

        /// <summary>
        /// The UpdateBookEdition_BookEditionDoesNotExist_Test.
        /// </summary>
        [Test]
        public void UpdateBookEdition_BookEditionDoesNotExist_Test()
        {
            this.SetUpGetBookEdition(new List<BookEdition>());

            var ex = Assert.Throws<ArgumentException>(() => this.service.Update(this.NewBookEdition));
            Assert.That(ex.Message, Is.EqualTo("Cannot update book edition, entity is missing"));
            Assert.Pass();
        }

        /// <summary>
        /// The UpdateBookEdition_BookIdChanged_Test.
        /// </summary>
        [Test]
        public void UpdateBookEdition_BookIdChanged_Test()
        {
            var bookEdition = this.NewBookEdition;
            bookEdition.BookId = 2;

            this.SetUpGetBookEdition(new List<BookEdition>() { bookEdition });

            var ex = Assert.Throws<ArgumentException>(() => this.service.Update(this.NewBookEdition));
            Assert.That(ex.Message, Is.EqualTo("Cannot add update book edition, book id was changed"));
            Assert.Pass();
        }

        /// <summary>
        /// The UpdateBookEdition_Success_Test.
        /// </summary>
        [Test]
        public void UpdateBookEdition_Success_Test()
        {
            this.SetUpGetBookEdition(new List<BookEdition>() { this.NewBookEdition });

            var result = this.service.Update(this.NewBookEdition);

            this.bookEditionRepositoryMock.Verify(x => x.Update(It.IsAny<BookEdition>()), Times.Once);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.Pass();
        }

        /// <summary>
        /// The DeleteBookEdition_BookEditionDoesNotExist_Test.
        /// </summary>
        [Test]
        public void DeleteBookEdition_BookEditionDoesNotExist_Test()
        {
            this.SetUpGetBookEdition(new List<BookEdition>());

            var ex = Assert.Throws<ArgumentException>(() => this.service.Delete(this.NewBookEdition));
            Assert.That(ex.Message, Is.EqualTo("Cannot delete book edition, entity is missing"));
            Assert.Pass();
        }

        /// <summary>
        /// The DeleteBookEdition_HasBookSamples_Test.
        /// </summary>
        [Test]
        public void DeleteBookEdition_HasBookSamples_Test()
        {
            var bookEdition = this.NewBookEdition;
            bookEdition.BookSamples = new List<BookSample>() { new BookSample() };

            this.SetUpGetBookEdition(new List<BookEdition>() { bookEdition });

            var ex = Assert.Throws<ArgumentException>(() => this.service.Delete(bookEdition));
            Assert.That(ex.Message, Is.EqualTo("Cannot delete book edition, entity has relations"));
            Assert.Pass();
        }

        /// <summary>
        /// The DeleteBookEdition_Success_Test.
        /// </summary>
        [Test]
        public void DeleteBookEdition_Success_Test()
        {
            this.SetUpGetBookEdition(new List<BookEdition>() { this.NewBookEdition });

            this.service.Delete(this.NewBookEdition);
            this.bookEditionRepositoryMock.Verify(x => x.Delete(It.IsAny<BookEdition>()), Times.Once);
            Assert.Pass();
        }

        /// <summary>
        /// The this.SetUpGetBookEdition.
        /// </summary>
        /// <param name="bookEditions">The bookEditions<see cref="List{BookEdition}"/>.</param>
        private void SetUpGetBookEdition(List<BookEdition> bookEditions)
        {
            this.bookEditionRepositoryMock.Setup(i => i.Get(It.IsAny<Expression<Func<BookEdition, bool>>>(), It.IsAny<Func<IQueryable<BookEdition>, IOrderedQueryable<BookEdition>>>(), It.IsAny<string>())).Returns(bookEditions);
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
