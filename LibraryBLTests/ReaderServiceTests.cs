using Library.BL.Infrastructure;
using Library.BL.Interfaces;
using Library.BL.Services;
using Library.DAL.DomainModel;
using Library.DAL.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;

namespace LibraryBLTests
{
    [ExcludeFromCodeCoverage]
    public class ReaderServiceTests
    {

        #region Private fields

        IReaderService _service;
        private readonly Microsoft.Extensions.Logging.ILogger<IReaderService> _logger;
        Mock<IReaderRepository> _repositoryMock;

        #endregion

        public ReaderServiceTests()
        {
            _logger = LoggerExtensions.TestLoggingInstance<IReaderService>();
        }

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<IReaderRepository>();
            _service = new ReaderService(_repositoryMock.Object, _logger);
        }

        [Test]
        public void Constructor_Test()
        {
            Assert.That(_service, Is.Not.Null);
            Assert.Pass();
        }

        [Test]
        public void Insert_NullArgumetException_Test()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _service.Insert(null));
            Assert.That(ex, Is.Not.Null);
            Assert.Pass();
        }

        [Test]
        public void Insert_InvalidEntity_Test()
        {
            var result = _service.Insert(new Reader());
            Assert.That(result.IsValid, Is.EqualTo(false));
            Assert.Pass();
        }

        [Test]
        public void Insert_Succes_Test()
        {
            var result = _service.Insert(new Reader() { UserId = 1 });
            Assert.That(result.IsValid, Is.EqualTo(true));
            _repositoryMock.Verify(i => i.Insert(It.IsAny<Reader>()), Times.Once);
            Assert.Pass();
        }

        [Test]
        public void Update_Succes_Test()
        {
            var result = _service.Update(new Reader() { UserId = 1 });
            Assert.That(result.IsValid, Is.EqualTo(true));
            _repositoryMock.Verify(i => i.Update(It.IsAny<Reader>()), Times.Once);
            Assert.Pass();
        }

        [Test]
        public void GetAll_Test()
        {
            SetUpGetReader(new List<Reader>() { new Reader() });

            var result = _service.GetAll();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.Pass();
        }

        private void SetUpGetReader(List<Reader> readers)
        {
            _repositoryMock.Setup(i => i.Get(It.IsAny<Expression<Func<Reader, bool>>>(), It.IsAny<Func<IQueryable<Reader>, IOrderedQueryable<Reader>>>(), It.IsAny<string>()))
                .Returns(readers);
        }
    }
}
