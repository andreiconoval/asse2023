using Library.BL.Infrastructure;
using Library.BL.Interfaces;
using Library.BL.Services;
using Library.DAL.DomainModel;
using Library.DAL.Interfaces;
using Moq;
using System.Linq.Expressions;

namespace LibraryBLTests
{
    public class UserUnitTests
    {
        IUserService _userService;
        private readonly Microsoft.Extensions.Logging.ILogger<IUserService> _logger;
        Mock<IUserRepository> _userRepositoryMock;

        #region

        private int ID = 1;

        #endregion

        public UserUnitTests()
        {
            _logger = LoggerExtensions.TestLoggingInstance<IUserService>();
        }

        [SetUp]
        public void Setup()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userService = new UserService(_userRepositoryMock.Object, _logger);
        }

        [Test]
        public void Constructor_Test()
        {
            _userService = new UserService(_userRepositoryMock.Object, _logger);
            Assert.That(_userService, Is.Not.Null);
            Assert.Pass();
        }



        [Test]
        public void DeleteUser_InvalidId_Test()
        {
            _userRepositoryMock.Setup(x => x.GetByID(It.IsAny<object>())).Returns<User>(null);

            var ex = Assert.Throws<ArgumentException>(() => _userService.DeleteUser(ID));
            Assert.That(ex.Message, Is.EqualTo($"Cannot delete user with id: {ID}, id is invalid"));
            Assert.Pass();
        }

        [Test]
        public void DeleteUser_Success_Test()
        {
            var user = new User { Id = ID };
            _userRepositoryMock.Setup(x => x.GetByID(It.IsAny<object>())).Returns(user);

            _userService.DeleteUser(ID);
            Assert.Pass();
        }


        [Test]
        public void AddUser_InvalidAuthor_Test()
        {
            var ex = Assert.Throws<ArgumentException>(() => _userService.AddUser(new User()));
            Assert.That(ex.Message, Is.EqualTo("Cannot add user, invalid entity"));
            Assert.Pass();
        }

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
            _userRepositoryMock.Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(), It.IsAny<string>())).Returns(new List<User>() { user });

            var ex = Assert.Throws<ArgumentException>(() => _userService.AddUser(user));
            Assert.That(ex.Message, Is.EqualTo("Cannot add user, user already exists"));
            Assert.Pass();
        }

        [Test]
        public void AddUser_Success_Test()
        {
            var user = new User()
            {
                Id = ID,
                FirstName = "FirstName",
                LastName = "LastName",
                Address = "Address",
                Phone = "000000",
                Email = "Email"
            };

            var result = _userService.AddUser(user);
            Assert.That(result, Is.EqualTo(ID));
            Assert.Pass();
        }


        [Test]
        public void UpdateUser_InvalidUser_Test()
        {
            var ex = Assert.Throws<ArgumentException>(() => _userService.UpdateUser(new User()));
            Assert.That(ex.Message, Is.EqualTo("Cannot update user, invalid entity"));
            Assert.Pass();
        }


        [Test]
        public void UdateAuthor_Success_Test()
        {
            var user = new User()
            {
                Id = ID,
                FirstName = "FirstName",
                LastName = "LastName",
                Address = "Address",
                Phone = "000000",
                Email = "Email"
            };

            _userRepositoryMock.Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(), It.IsAny<string>())).Returns(new List<User>() { user });

            _userService.UpdateUser(user);
            Assert.Pass();
        }

        [Test]
        public void UdateAuthor_MissingAuthor_Test()
        {
            var user = new User()
            {
                Id = ID,
                FirstName = "FirstName",
                LastName = "LastName",
                Address = "Address",
                Phone = "000000",
                Email = "Email"
            };
            _userRepositoryMock.Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(), It.IsAny<string>())).Returns(new List<User>() { });

            var ex = Assert.Throws<ArgumentException>(() => _userService.UpdateUser(user));
            Assert.That(ex.Message, Is.EqualTo("Cannot update user, entity is missing"));
            Assert.Pass();
        }

    }
}
