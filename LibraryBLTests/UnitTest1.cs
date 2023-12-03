
using Library.BL.Services;
using Library.DAL.DomainModel;
using Library.DAL.Interfaces;
using Moq;
using Rhino.Mocks;

namespace LibraryBLTests
{
    public class Tests
    {
        UserService userService;
        Mock<IUserRepository> userRepositoryMock;

        public Tests()
        {
            userRepositoryMock = new Mock<IUserRepository>();
            userService = new UserService(userRepositoryMock.Object);
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            userRepositoryMock.Setup(x => x.GetByID(It.IsAny<object>())).Returns(new User() { Email="lll"});


            var user = userService.GetByID(1);
            Assert.IsNotNull(user);
            Assert.That(user.Email, Is.EqualTo("lll"));
            Assert.Pass();
        }
    }
}