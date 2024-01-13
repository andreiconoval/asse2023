//------------------------------------------------------------------------------
// <copyright file="UserServiceTests.cs" company="Transilvania University of Brasov">
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
    /// Defines the <see cref="UserUnitTests" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class UserServiceTests
    {
        /// <summary>
        /// Defines the userService.
        /// </summary>
        private IUserService userService;

        /// <summary>
        /// Defines the logger.
        /// </summary>
        private Microsoft.Extensions.Logging.ILogger<IUserService> logger;

        /// <summary>
        /// Defines the userRepositoryMock.
        /// </summary>
        private Mock<IUserRepository> userRepositoryMock;

        /// <summary>
        /// Defines the ID.
        /// </summary>
        private int id = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserServiceTests"/> class.
        /// </summary>
        public UserServiceTests()
        {
            this.logger = LoggerExtensions.TestLoggingInstance<IUserService>();
        }

        /// <summary>
        /// The Setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.userRepositoryMock = new Mock<IUserRepository>();
            this.userService = new UserService(this.userRepositoryMock.Object, this.logger);
        }

        /// <summary>
        /// The Constructor_Test.
        /// </summary>
        [Test]
        public void Constructor_Test()
        {
            this.userService = new UserService(this.userRepositoryMock.Object, this.logger);
            Assert.That(this.userService, Is.Not.Null);
            Assert.Pass();
        }

        /// <summary>
        /// The DeleteUser_InvalidId_Test.
        /// </summary>
        [Test]
        public void DeleteUser_InvalidId_Test()
        {
            this.userRepositoryMock.Setup(x => x.GetByID(It.IsAny<object>())).Returns<User>(null);

            var ex = Assert.Throws<ArgumentException>(() => this.userService.DeleteUser(this.id));
            Assert.That(ex.Message, Is.EqualTo($"Cannot delete user with id: {this.id}, id is invalid"));
            Assert.Pass();
        }

        /// <summary>
        /// The DeleteUser_Success_Test.
        /// </summary>
        [Test]
        public void DeleteUser_Success_Test()
        {
            var user = new User { Id = this.id };
            this.userRepositoryMock.Setup(x => x.GetByID(It.IsAny<object>())).Returns(user);

            this.userService.DeleteUser(this.id);
            Assert.Pass();
        }

        /// <summary>
        /// The AddUser_InvalidAuthor_FirstName_Test.
        /// </summary>
        [Test]
        public void AddUser_InvalidAuthor_FirstName_Test()
        {
            var user = new User()
            {
                LastName = "LastName",
                Address = "Address",
                Phone = "000000",
                Email = "Email"
            };

            var ex = Assert.Throws<ArgumentException>(() => this.userService.AddUser(user));
            Assert.That(ex.Message, Is.EqualTo("Cannot add user, invalid entity"));
            Assert.Pass();
        }

        /// <summary>
        /// The AddUser_InvalidAuthor_FirstNameTooLong_Test.
        /// </summary>
        [Test]
        public void AddUser_InvalidAuthor_FirstNameTooLong_Test()
        {
            var user = new User()
            {
                FirstName = new string('1', 256),
                LastName = "LastName",
                Address = "Address",
                Phone = "000000",
                Email = "Email"
            };

            var ex = Assert.Throws<ArgumentException>(() => this.userService.AddUser(user));
            Assert.That(ex.Message, Is.EqualTo("Cannot add user, invalid entity"));
            Assert.Pass();
        }

        /// <summary>
        /// The AddUser_InvalidAuthor_LastName_Test.
        /// </summary>
        [Test]
        public void AddUser_InvalidAuthor_LastName_Test()
        {
            var user = new User()
            {
                FirstName = "FirstName",
                Address = "Address",
                Phone = "000000",
                Email = "Email"
            };
            var ex = Assert.Throws<ArgumentException>(() => this.userService.AddUser(new User()));
            Assert.That(ex.Message, Is.EqualTo("Cannot add user, invalid entity"));
            Assert.Pass();
        }

        /// <summary>
        /// The AddUser_InvalidAuthor_LastNameTooLong_Test.
        /// </summary>
        [Test]
        public void AddUser_InvalidAuthor_LastNameTooLong_Test()
        {
            var user = new User()
            {
                FirstName = "FirstName",
                LastName = new string('1', 256),
                Address = "Address",
                Phone = "000000",
                Email = "Email"
            };
            var ex = Assert.Throws<ArgumentException>(() => this.userService.AddUser(new User()));
            Assert.That(ex.Message, Is.EqualTo("Cannot add user, invalid entity"));
            Assert.Pass();
        }

        /// <summary>
        /// The AddUser_InvalidAuthor_Address_Test.
        /// </summary>
        [Test]
        public void AddUser_InvalidAuthor_Address_Test()
        {
            var user = new User()
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Phone = "000000",
                Email = "Email"
            };
            var ex = Assert.Throws<ArgumentException>(() => this.userService.AddUser(new User()));
            Assert.That(ex.Message, Is.EqualTo("Cannot add user, invalid entity"));
            Assert.Pass();
        }

        /// <summary>
        /// The AddUser_InvalidAuthor_AddressTooLong_Test.
        /// </summary>
        [Test]
        public void AddUser_InvalidAuthor_AddressTooLong_Test()
        {
            var user = new User()
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Address = new string('1', 256),
                Phone = "000000",
                Email = "Email"
            };
            var ex = Assert.Throws<ArgumentException>(() => this.userService.AddUser(new User()));
            Assert.That(ex.Message, Is.EqualTo("Cannot add user, invalid entity"));
            Assert.Pass();
        }

        /// <summary>
        /// The AddUser_InvalidAuthor_PhoneTooLong_Test.
        /// </summary>
        [Test]
        public void AddUser_InvalidAuthor_PhoneTooLong_Test()
        {
            var user = new User()
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Phone = new string('1', 16),
                Address = "Address",
                Email = "Email"
            };
            var ex = Assert.Throws<ArgumentException>(() => this.userService.AddUser(new User()));
            Assert.That(ex.Message, Is.EqualTo("Cannot add user, invalid entity"));
            Assert.Pass();
        }

        /// <summary>
        /// The AddUser_InvalidAuthor_EmailTooLong_Test.
        /// </summary>
        [Test]
        public void AddUser_InvalidAuthor_EmailTooLong_Test()
        {
            var user = new User()
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Phone = new string('1', 14),
                Address = "Address",
                Email = new string('1', 256)
            };
            var ex = Assert.Throws<ArgumentException>(() => this.userService.AddUser(new User()));
            Assert.That(ex.Message, Is.EqualTo("Cannot add user, invalid entity"));
            Assert.Pass();
        }

        /// <summary>
        /// The AddUser_UserExists_Test.
        /// </summary>
        [Test]
        public void AddUser_UserExists_Test()
        {
            var user = new User()
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Address = "Address",
                Phone = "000000",
                Email = "Email"
            };
            this.userRepositoryMock.Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(), It.IsAny<string>())).Returns(new List<User>() { user });

            var ex = Assert.Throws<ArgumentException>(() => this.userService.AddUser(user));
            Assert.That(ex.Message, Is.EqualTo("Cannot add user, user already exists"));
            Assert.Pass();
        }

        /// <summary>
        /// The AddUser_Success_Test.
        /// </summary>
        [Test]
        public void AddUser_Success_Test()
        {
            var user = new User()
            {
                Id = this.id,
                FirstName = "FirstName",
                LastName = "LastName",
                Address = "Address",
                Phone = "000000",
                Email = "Email"
            };

            var result = this.userService.AddUser(user);
            Assert.That(result, Is.EqualTo(this.id));
            Assert.Pass();
        }

        /// <summary>
        /// The UpdateUser_InvalidUser_Test.
        /// </summary>
        [Test]
        public void UpdateUser_InvalidUser_Test()
        {
            var ex = Assert.Throws<ArgumentException>(() => this.userService.UpdateUser(new User()));
            Assert.That(ex.Message, Is.EqualTo("Cannot update user, invalid entity"));
            Assert.Pass();
        }

        /// <summary>
        /// The UpdateAuthor_Success_Test.
        /// </summary>
        [Test]
        public void UpdateAuthor_Success_Test()
        {
            var user = new User()
            {
                Id = this.id,
                FirstName = "FirstName",
                LastName = "LastName",
                Address = "Address",
                Phone = "000000",
                Email = "Email"
            };

            this.userRepositoryMock.Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(), It.IsAny<string>())).Returns(new List<User>() { user });

            this.userService.UpdateUser(user);
            Assert.Pass();
        }

        /// <summary>
        /// The UpdateAuthor_MissingAuthor_Test.
        /// </summary>
        [Test]
        public void UpdateAuthor_MissingAuthor_Test()
        {
            var user = new User()
            {
                Id = this.id,
                FirstName = "FirstName",
                LastName = "LastName",
                Address = "Address",
                Phone = "000000",
                Email = "Email"
            };
            this.userRepositoryMock.Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(), It.IsAny<string>())).Returns(new List<User>() { });
            var ex = Assert.Throws<ArgumentException>(() => this.userService.UpdateUser(user));
            Assert.That(ex.Message, Is.EqualTo("Cannot update user, entity is missing"));
            Assert.Pass();
        }
    }
}
