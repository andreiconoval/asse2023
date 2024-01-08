//------------------------------------------------------------------------------
// <copyright file="BookSampleServiceTests.cs" company="Transilvania University of Brasov">
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

    /// <summary>
    /// Defines the <see cref="BookSampleServiceTests" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class BookSampleServiceTests
    {
        /// <summary>
        /// Defines the _service.
        /// </summary>
        private IBookSampleService service;

        /// <summary>
        /// Defines the _logger.
        /// </summary>
        private Microsoft.Extensions.Logging.ILogger<IBookSampleService> logger;

        /// <summary>
        /// Defines the _repositoryMock.
        /// </summary>
        private Mock<IBookSampleRepository> repositoryMock;

        /// <summary>
        /// Defines the _bookEditionRepositoryMock.
        /// </summary>
        private Mock<IBookEditionRepository> bookEditionRepositoryMock;

        /// <summary>
        /// Defines the _bookLoanDetailRepositoryMock.
        /// </summary>
        private Mock<IBookLoanDetailRepository> bookLoanDetailRepositoryMock;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookSampleServiceTests"/> class.
        /// </summary>
        public BookSampleServiceTests()
        {
            this.logger = LoggerExtensions.TestLoggingInstance<IBookSampleService>();
        }

        /// <summary>
        /// Gets the this.NewBookSample.
        /// </summary>
        public BookSample NewBookSample => new BookSample()
        {
            BookEditionId = 1,
            Id = 1,
            AvailableForLoan = true,
            AvailableForHall = true
        };

        /// <summary>
        /// The Setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.repositoryMock = new Mock<IBookSampleRepository>();
            this.bookEditionRepositoryMock = new Mock<IBookEditionRepository>();
            this.bookLoanDetailRepositoryMock = new Mock<IBookLoanDetailRepository>();
            this.service = new BookSampleService(this.repositoryMock.Object, this.bookEditionRepositoryMock.Object, this.bookLoanDetailRepositoryMock.Object, this.logger);
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
        /// The Insert_InvalidEntity_Test.
        /// </summary>
        [Test]
        public void Insert_InvalidEntity_Test()
        {
            var ex = Assert.Throws<ArgumentException>(() => this.service.Insert(new BookSample()));
            Assert.That(ex.Message, Is.EqualTo("Cannot add book sample, invalid entity"));
            Assert.Pass();
        }

        /// <summary>
        /// The Insert_InvalidId_Test.
        /// </summary>
        [Test]
        public void Insert_InvalidId_Test()
        {
            var bookSample = this.NewBookSample;

            bookSample.BookEditionId = default;

            var ex = Assert.Throws<ArgumentException>(() => this.service.Insert(bookSample));
            Assert.That(ex.Message, Is.EqualTo("Cannot add book sample, invalid entity"));
            Assert.Pass();
        }

        /// <summary>
        /// The Insert_NotAvailableForLoanAndHall_Test.
        /// </summary>
        [Test]
        public void Insert_NotAvailableForLoanAndHall_Test()
        {
            var bookSample = this.NewBookSample;

            bookSample.AvailableForLoan = false;
            bookSample.AvailableForHall = false;

            var ex = Assert.Throws<ArgumentException>(() => this.service.Insert(bookSample));
            Assert.That(ex.Message, Is.EqualTo("Cannot add book sample, invalid entity"));
            Assert.Pass();
        }

        /// <summary>
        /// The Insert_BookEditionDoesNotExist_Test.
        /// </summary>
        [Test]
        public void Insert_BookEditionDoesNotExist_Test()
        {
            this.SetUpGetBookEdition(new List<BookEdition>());

            var ex = Assert.Throws<ArgumentException>(() => this.service.Insert(this.NewBookSample));
            Assert.That(ex.Message, Is.EqualTo("Cannot add book sample, book edition do not exists"));
            Assert.Pass();
        }

        /// <summary>
        /// The Insert_InvalidOperation_Test.
        /// </summary>
        [Test]
        public void Insert_InvalidOperation_Test()
        {
            this.SetUpGetBookEdition(new List<BookEdition>() { new BookEdition() });
            this.repositoryMock.Setup(i => i.Insert(It.IsAny<BookSample>())).Throws(new Exception("Invalid operation"));

            var ex = Assert.Throws<Exception>(() => this.service.Insert(this.NewBookSample));
            Assert.That(ex.Message, Is.EqualTo("Invalid operation"));
            Assert.Pass();
        }

        /// <summary>
        /// The Insert_Success_Test.
        /// </summary>
        [Test]
        public void Insert_Success_Test()
        {
            this.SetUpGetBookEdition(new List<BookEdition>() { new BookEdition() });

            var result = this.service.Insert(this.NewBookSample);
            Assert.That(result.IsValid, Is.EqualTo(true));
            this.repositoryMock.Verify(i => i.Insert(It.IsAny<BookSample>()), Times.Once);
            Assert.Pass();
        }

        /// <summary>
        /// The Update_NullArgumentException_Test.
        /// </summary>
        [Test]
        public void Update_NullArgumentException_Test()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => this.service.Update(null));
            Assert.That(ex, Is.Not.Null);
            Assert.Pass();
        }

        /// <summary>
        /// The Update_InvalidEntity_Test.
        /// </summary>
        [Test]
        public void Update_InvalidEntity_Test()
        {
            var ex = Assert.Throws<ArgumentException>(() => this.service.Update(new BookSample()));
            Assert.That(ex.Message, Is.EqualTo("Cannot update book sample, invalid entity"));
            Assert.Pass();
        }

        /// <summary>
        /// The Update_InvalidId_Test.
        /// </summary>
        [Test]
        public void Update_InvalidId_Test()
        {
            var bookSample = this.NewBookSample;

            bookSample.BookEditionId = default;

            var ex = Assert.Throws<ArgumentException>(() => this.service.Update(bookSample));
            Assert.That(ex.Message, Is.EqualTo("Cannot update book sample, invalid entity"));
            Assert.Pass();
        }

        /// <summary>
        /// The Update_NotAvailableForLoanAndHall_Test.
        /// </summary>
        [Test]
        public void Update_NotAvailableForLoanAndHall_Test()
        {
            var bookSample = this.NewBookSample;

            bookSample.AvailableForLoan = false;
            bookSample.AvailableForHall = false;

            var ex = Assert.Throws<ArgumentException>(() => this.service.Update(bookSample));
            Assert.That(ex.Message, Is.EqualTo("Cannot update book sample, invalid entity"));
            Assert.Pass();
        }

        /// <summary>
        /// The Update_BookSampleDoesNotExist_Test.
        /// </summary>
        [Test]
        public void Update_BookSampleDoesNotExist_Test()
        {
            var ex = Assert.Throws<ArgumentException>(() => this.service.Update(this.NewBookSample));
            Assert.That(ex.Message, Is.EqualTo("Cannot update book sample, book sample do not exists"));
            Assert.Pass();
        }

        /// <summary>
        /// The Update_BookEditionDoesNotExist_Test.
        /// </summary>
        [Test]
        public void Update_BookEditionDoesNotExist_Test()
        {
            this.SetUpGetBookSample(new List<BookSample>() { this.NewBookSample });

            var ex = Assert.Throws<ArgumentException>(() => this.service.Update(this.NewBookSample));
            Assert.That(ex.Message, Is.EqualTo("Cannot add book sample, book edition do not exists"));
            Assert.Pass();
        }

        /// <summary>
        /// The Update_InvalidOperation_Test.
        /// </summary>
        [Test]
        public void Update_InvalidOperation_Test()
        {
            this.SetUpGetBookSample(new List<BookSample>() { this.NewBookSample });
            this.SetUpGetBookEdition(new List<BookEdition>() { new BookEdition() });
            this.repositoryMock.Setup(i => i.Update(It.IsAny<BookSample>())).Throws(new Exception("Invalid operation"));

            var ex = Assert.Throws<Exception>(() => this.service.Update(this.NewBookSample));
            Assert.That(ex.Message, Is.EqualTo("Invalid operation"));
            Assert.Pass();
        }

        /// <summary>
        /// The Update_Success_Test.
        /// </summary>
        [Test]
        public void Update_Success_Test()
        {
            this.SetUpGetBookSample(new List<BookSample>() { this.NewBookSample });
            this.SetUpGetBookEdition(new List<BookEdition>() { new BookEdition() });

            var result = this.service.Update(this.NewBookSample);
            Assert.That(result.IsValid, Is.EqualTo(true));
            this.repositoryMock.Verify(i => i.Update(It.IsAny<BookSample>()), Times.Once);
            Assert.Pass();
        }

        /// <summary>
        /// The Delete_NullParameters_Test.
        /// </summary>
        [Test]
        public void Delete_NullParameters_Test()
        {
            var ex = Assert.Throws<ArgumentException>(() => this.service.Delete(null));
            Assert.That(ex.Message, Is.EqualTo("Cannot delete book sample, invalid entity"));
            Assert.Pass();
        }

        /// <summary>
        /// The Delete_InvalidId_Test.
        /// </summary>
        [Test]
        public void Delete_InvalidId_Test()
        {
            var ex = Assert.Throws<ArgumentException>(() => this.service.Delete(new BookSample { BookEditionId = 0 }));
            Assert.That(ex.Message, Is.EqualTo("Cannot delete book sample, invalid entity"));
            Assert.Pass();
        }

        /// <summary>
        /// The Delete_BookBookLoanExists_Test.
        /// </summary>
        [Test]
        public void Delete_BookBookLoanExists_Test()
        {
            this.SetUpGetBookLoan(new List<BookLoanDetail>() { new BookLoanDetail() });

            var ex = Assert.Throws<ArgumentException>(() => this.service.Delete(this.NewBookSample));
            Assert.That(ex.Message, Is.EqualTo("Cannot delete book sample, book is in use"));
            Assert.Pass();
        }

        /// <summary>
        /// The Delete_Success_Test.
        /// </summary>
        [Test]
        public void Delete_Success_Test()
        {
            this.service.Delete(this.NewBookSample);
            this.repositoryMock.Verify(i => i.Delete(It.IsAny<BookSample>()), Times.Once);
            Assert.Pass();
        }

        /// <summary>
        /// The GetAll_Test.
        /// </summary>
        [Test]
        public void GetAll_Test()
        {
            this.SetUpGetBookSample(new List<BookSample>() { this.NewBookSample });

            var result = this.service.GetAll();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.Pass();
        }

        /// <summary>
        /// The SetUpGetBookEdition.
        /// </summary>
        /// <param name="bookEditions">The bookEditions<see cref="List{BookEdition}"/>.</param>
        private void SetUpGetBookEdition(List<BookEdition> bookEditions)
        {
            this.bookEditionRepositoryMock.Setup(i => i.Get(It.IsAny<Expression<Func<BookEdition, bool>>>(), It.IsAny<Func<IQueryable<BookEdition>, IOrderedQueryable<BookEdition>>>(), It.IsAny<string>()))
                .Returns(bookEditions);
        }

        /// <summary>
        /// The SetUpGetBookSample.
        /// </summary>
        /// <param name="bookSamples">The bookSamples<see cref="List{BookSample}"/>.</param>
        private void SetUpGetBookSample(List<BookSample> bookSamples)
        {
            this.repositoryMock.Setup(i => i.Get(It.IsAny<Expression<Func<BookSample, bool>>>(), It.IsAny<Func<IQueryable<BookSample>, IOrderedQueryable<BookSample>>>(), It.IsAny<string>()))
                .Returns(bookSamples);
        }

        /// <summary>
        /// The this.SetUpGetBookLoan.
        /// </summary>
        /// <param name="bookLoanDetail">The bookLoanDetail<see cref="List{BookLoanDetail}"/>.</param>
        private void SetUpGetBookLoan(List<BookLoanDetail> bookLoanDetail)
        {
            this.bookLoanDetailRepositoryMock.Setup(i => i.Get(It.IsAny<Expression<Func<BookLoanDetail, bool>>>(), It.IsAny<Func<IQueryable<BookLoanDetail>, IOrderedQueryable<BookLoanDetail>>>(), It.IsAny<string>()))
                .Returns(bookLoanDetail);
        }
    }
}
