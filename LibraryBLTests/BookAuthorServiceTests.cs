//------------------------------------------------------------------------------
// <copyright file="BookAuthorServiceTests.cs" company="Transilvania University of Brasov">
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
    /// Defines the <see cref="BookAuthorServiceTests" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class BookAuthorServiceTests
    {
        /// <summary>
        /// Defines the service.
        /// </summary>
        private IBookAuthorService service;

        /// <summary>
        /// Defines the logger.
        /// </summary>
        private Microsoft.Extensions.Logging.ILogger<IBookAuthorService> logger;

        /// <summary>
        /// Defines the repositoryMock.
        /// </summary>
        private Mock<IBookAuthorRepository> repositoryMock;

        /// <summary>
        /// Defines the authorRepositoryMock.
        /// </summary>
        private Mock<IAuthorRepository> authorRepositoryMock;

        /// <summary>
        /// Defines the bookRepositoryMock.
        /// </summary>
        private Mock<IBookRepository> bookRepositoryMock;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookAuthorServiceTests"/> class.
        /// </summary>
        public BookAuthorServiceTests()
        {
            this.logger = LoggerExtensions.TestLoggingInstance<IBookAuthorService>();
        }

        /// <summary>
        /// Gets the NewBookAuthor.
        /// </summary>
        private BookAuthor NewBookAuthor => new BookAuthor()
        {
            BookId = 1,
            AuthorId = 1,
            Author = new Author(),
            Book = new Book()
        };

        /// <summary>
        /// The Setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.repositoryMock = new Mock<IBookAuthorRepository>();
            this.authorRepositoryMock = new Mock<IAuthorRepository>();
            this.bookRepositoryMock = new Mock<IBookRepository>();
            this.service = new BookAuthorService(this.repositoryMock.Object, this.authorRepositoryMock.Object, this.bookRepositoryMock.Object, this.logger);
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
        /// The Insert_NullArgumentException_Test.
        /// </summary>
        [Test]
        public void Insert_NullArgumentException_Test()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => this.service.Insert(null));
            Assert.That(ex, Is.Not.Null);
            Assert.Pass();
        }

        /// <summary>
        /// The Insert_InvalidEntity_AuthorIdInvalid_Test.
        /// </summary>
        [Test]
        public void Insert_InvalidEntity_AuthorIdInvalid_Test()
        {
            var ex = Assert.Throws<ArgumentException>(() => this.service.Insert(new BookAuthor() { BookId = 1 }));
            Assert.That(ex.Message, Is.EqualTo("Cannot add book author, invalid entity"));
            Assert.Pass();
        }

        /// <summary>
        /// The Insert_InvalidEntity_BookIdInvalid_Test.
        /// </summary>
        [Test]
        public void Insert_InvalidEntity_BookIdInvalid_Test()
        {
            var ex = Assert.Throws<ArgumentException>(() => this.service.Insert(new BookAuthor() { AuthorId = 1 }));
            Assert.That(ex.Message, Is.EqualTo("Cannot add book author, invalid entity"));
            Assert.Pass();
        }

        /// <summary>
        /// The Insert_BookAuthorExist_Test.
        /// </summary>
        [Test]
        public void Insert_BookAuthorExist_Test()
        {
            this.SetUpGetBookAuthor(new List<BookAuthor>() { this.NewBookAuthor });

            var ex = Assert.Throws<ArgumentException>(() => this.service.Insert(this.NewBookAuthor));
            Assert.That(ex.Message, Is.EqualTo("Cannot add book author, book author already exists"));
            Assert.Pass();
        }

        /// <summary>
        /// The Insert_BookNotExist_Test.
        /// </summary>
        [Test]
        public void Insert_BookNotExist_Test()
        {
            this.SetUpGetBook(new List<Book>());

            var ex = Assert.Throws<ArgumentException>(() => this.service.Insert(this.NewBookAuthor));
            Assert.That(ex.Message, Is.EqualTo("Cannot add book author, book does not exist"));
            Assert.Pass();
        }

        /// <summary>
        /// The Insert_AuthorNotExist_Test.
        /// </summary>
        [Test]
        public void Insert_AuthorNotExist_Test()
        {
            this.SetUpGetBook(new List<Book>() { new Book() });
            this.SetUpGetAuthor(new List<Author>());

            var ex = Assert.Throws<ArgumentException>(() => this.service.Insert(this.NewBookAuthor));
            Assert.That(ex.Message, Is.EqualTo("Cannot add book author, author does not exist"));
            Assert.Pass();
        }

        /// <summary>
        /// The Insert_InvalidException_Test.
        /// </summary>
        [Test]
        public void Insert_InvalidException_Test()
        {
            this.SetUpGetBook(new List<Book>() { new Book() });
            this.SetUpGetAuthor(new List<Author>() { new Author() });

            this.repositoryMock.Setup(i => i.Insert(It.IsAny<BookAuthor>())).Throws(new Exception("Invalid operation"));

            var ex = Assert.Throws<Exception>(() => this.service.Insert(this.NewBookAuthor));
            Assert.That(ex.Message, Is.EqualTo("Invalid operation"));
            Assert.Pass();
        }

        /// <summary>
        /// The Insert_Success_Test.
        /// </summary>
        [Test]
        public void Insert_Success_Test()
        {
            this.SetUpGetBook(new List<Book>() { new Book() });
            this.SetUpGetAuthor(new List<Author>() { new Author() });

            var result = this.service.Insert(this.NewBookAuthor);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.Pass();
        }

        /// <summary>
        /// The Delete_Success_Test.
        /// </summary>
        [Test]
        public void Delete_Success_Test()
        {
            this.service.Delete(this.NewBookAuthor);
            Assert.That(this.repositoryMock.Invocations.Count, Is.EqualTo(1));
            this.repositoryMock.Verify(x => x.Delete(It.IsAny<BookAuthor>()), Times.Once);
            Assert.Pass();
        }

        /// <summary>
        /// The Update_ThrowException_Test.
        /// </summary>
        [Test]
        public void Update_ThrowException_Test()
        {
            var ex = Assert.Throws<Exception>(() => this.service.Update(this.NewBookAuthor));
            Assert.That(ex.Message, Is.EqualTo("To delete and add new Book author is the best approach"));
            Assert.Pass();
        }


        /// <summary>
        /// The DeleteDomain_DomainNull_Test.
        /// </summary>
        [Test]
        public void DeleteBookAuthor_BookAuthorNull_Test()
        {
            BookAuthor bookAuthor = null;
            var ex = Assert.Throws<ArgumentException>(() => this.service.Delete(bookAuthor));
            Assert.That(ex.Message, Is.EqualTo("Cannot delete book author, book author is missing"));
            Assert.Pass();
        }

        /// <summary>
        /// The SetUpGetBookAuthor.
        /// </summary>
        /// <param name="bookAuthors">The bookAuthors<see cref="List{BookAuthor}"/>.</param>
        private void SetUpGetBookAuthor(List<BookAuthor> bookAuthors)
        {
            this.repositoryMock.Setup(i => i.Get(It.IsAny<Expression<Func<BookAuthor, bool>>>(), It.IsAny<Func<IQueryable<BookAuthor>, IOrderedQueryable<BookAuthor>>>(), It.IsAny<string>()))
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

        /// <summary>
        /// The SetUpGetAuthor.
        /// </summary>
        /// <param name="authors">The authors<see cref="List{Author}"/>.</param>
        private void SetUpGetAuthor(List<Author> authors)
        {
            this.authorRepositoryMock.Setup(i => i.Get(It.IsAny<Expression<Func<Author, bool>>>(), It.IsAny<Func<IQueryable<Author>, IOrderedQueryable<Author>>>(), It.IsAny<string>()))
                .Returns(authors);
        }

        /// <summary>
        /// The SetUpGetBook.
        /// </summary>
        /// <param name="books">The books<see cref="List{Book}"/>.</param>
        private void SetUpGetBook(List<Book> books)
        {
            this.bookRepositoryMock.Setup(i => i.Get(It.IsAny<Expression<Func<Book, bool>>>(), It.IsAny<Func<IQueryable<Book>, IOrderedQueryable<Book>>>(), It.IsAny<string>()))
                .Returns(books);
        }
    }
}
