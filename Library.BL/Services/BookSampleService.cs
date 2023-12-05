using Library.BL.Interfaces;
using Library.BL.Validators;
using Library.DAL.DomainModel;
using Library.DAL.Interfaces;
using Microsoft.Extensions.Logging;

namespace Library.BL.Services
{
    public class BookSampleService : BaseService<BookSample, IBookSampleRepository>, IBookSampleService
    {
        public BookSampleService(IBookSampleRepository repository, ILogger logger) : base(repository, new BookSampleValidator(), logger)
        {
        }
    }
}
