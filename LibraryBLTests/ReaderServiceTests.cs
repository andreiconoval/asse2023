//------------------------------------------------------------------------------
// <copyright file="ReaderServiceTests.cs" company="Transilvania University of Brasov">
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
    /// Defines the <see cref="ReaderServiceTests" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ReaderServiceTests
    {
        /// <summary>
        /// Defines the service.
        /// </summary>
        private IReaderService service;

        /// <summary>
        /// Defines the logger.
        /// </summary>
        private Microsoft.Extensions.Logging.ILogger<IReaderService> logger;

        /// <summary>
        /// Defines the repositoryMock.
        /// </summary>
        private Mock<IReaderRepository> repositoryMock;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReaderServiceTests"/> class.
        /// </summary>
        public ReaderServiceTests()
        {
            this.logger = LoggerExtensions.TestLoggingInstance<IReaderService>();
        }

        /// <summary>
        /// The Setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.repositoryMock = new Mock<IReaderRepository>();
            this.service = new ReaderService(this.repositoryMock.Object, this.logger);
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
            var result = this.service.Insert(new Reader());
            Assert.That(result.IsValid, Is.EqualTo(false));
            Assert.Pass();
        }

        /// <summary>
        /// The Insert_Success_Test.
        /// </summary>
        [Test]
        public void Insert_Success_Test()
        {
            var result = this.service.Insert(new Reader() { UserId = 1 });
            Assert.That(result.IsValid, Is.EqualTo(true));
            this.repositoryMock.Verify(i => i.Insert(It.IsAny<Reader>()), Times.Once);
            Assert.Pass();
        }

        /// <summary>
        /// The Update_Success_Test.
        /// </summary>
        [Test]
        public void Update_Success_Test()
        {
            var result = this.service.Update(new Reader() { UserId = 1 });
            Assert.That(result.IsValid, Is.EqualTo(true));
            this.repositoryMock.Verify(i => i.Update(It.IsAny<Reader>()), Times.Once);
            Assert.Pass();
        }

        /// <summary>
        /// The GetAll_Test.
        /// </summary>
        [Test]
        public void GetAll_Test()
        {
            this.SetUpGetReader(new List<Reader>() { new Reader() });

            var result = this.service.GetAll();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.Pass();
        }

        /// <summary>
        /// The SetUpGetReader.
        /// </summary>
        /// <param name="readers">The readers<see cref="List{Reader}"/>.</param>
        private void SetUpGetReader(List<Reader> readers)
        {
            this.repositoryMock.Setup(i => i.Get(It.IsAny<Expression<Func<Reader, bool>>>(), It.IsAny<Func<IQueryable<Reader>, IOrderedQueryable<Reader>>>(), It.IsAny<string>()))
                .Returns(readers);
        }
    }
}
