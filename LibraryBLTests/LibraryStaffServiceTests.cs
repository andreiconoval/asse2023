//------------------------------------------------------------------------------
// <copyright file="LibraryBLTests.cs" company="Transilvania University of Brasov">
// Copyright (c) Conoval. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
//------------------------------------------------------------------------------

namespace LibraryBLTests
{
    using Library.BL.Interfaces;
    using Library.BL.Services;
    using Library.DAL.DomainModel;
    using Library.DAL.Interfaces;
    using Moq;
    using NUnit.Framework;
    using System.Diagnostics.CodeAnalysis;
    using Library.BL.Infrastructure;
    using System.Linq.Expressions;

    [ExcludeFromCodeCoverage]

    public class LibraryStaffServiceTests
    {
        /// <summary>
        /// Defines the service.
        /// </summary>
        private ILibraryStaffService service;

        /// <summary>
        /// Defines the logger.
        /// </summary>
        private Microsoft.Extensions.Logging.ILogger<ILibraryStaffService> logger;

        /// <summary>
        /// Defines the repositoryMock.
        /// </summary>
        private Mock<ILibraryStaffRepository> repositoryMock;

        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryStaffServiceTests"/> class.
        /// </summary>
        public LibraryStaffServiceTests()
        {
            this.logger = LoggerExtensions.TestLoggingInstance<ILibraryStaffService>();
        }

        /// <summary>
        /// The Setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.repositoryMock = new Mock<ILibraryStaffRepository>();
            this.service = new LibraryStaffService(this.repositoryMock.Object, this.logger);
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
            var result = this.service.Insert(new LibraryStaff());
            Assert.That(result.IsValid, Is.EqualTo(false));
            Assert.Pass();
        }

        /// <summary>
        /// The Insert_Success_Test.
        /// </summary>
        [Test]
        public void Insert_Success_Test()
        {
            var result = this.service.Insert(new LibraryStaff() { UserId = 1, User = new User() });
            Assert.That(result.IsValid, Is.EqualTo(true));
            this.repositoryMock.Verify(i => i.Insert(It.IsAny<LibraryStaff>()), Times.Once);
            Assert.Pass();
        }

        /// <summary>
        /// The Update_Success_Test.
        /// </summary>
        [Test]
        public void Update_Success_Test()
        {
            var result = this.service.Update(new LibraryStaff() { UserId = 1 });

            Assert.That(result.IsValid, Is.EqualTo(true));
            this.repositoryMock.Verify(i => i.Update(It.IsAny<LibraryStaff>()), Times.Once);
            Assert.Pass();
        }

        /// <summary>
        /// The GetAll_Test.
        /// </summary>
        [Test]
        public void GetAll_Test()
        {
            this.SetUpGetLibraryStaff(new List<LibraryStaff>() { new LibraryStaff() });

            var result = this.service.GetAll();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.Pass();
        }

        /// <summary>
        /// The SetUpGetLibraryStaff.
        /// </summary>
        /// <param name="libraryStaffs">The libraryStaffs<see cref="List{LibraryStaff}"/>.</param>
        private void SetUpGetLibraryStaff(List<LibraryStaff> libraryStaffs)
        {
            this.repositoryMock.Setup(i => i.Get(It.IsAny<Expression<Func<LibraryStaff, bool>>>(), It.IsAny<Func<IQueryable<LibraryStaff>, IOrderedQueryable<LibraryStaff>>>(), It.IsAny<string>()))
                .Returns(libraryStaffs);
        }

    }
}
