using Library.BL.Interfaces;
using Library.BL.Validators;
using Library.DAL.DomainModel;
using Library.DAL.Interfaces;

namespace Library.BL.Services
{
    public class BookSampleService : BaseService<BookSample, IBookSampleRepository>, IBookSampleService
    {
        public BookSampleService(IBookSampleRepository repository) : base(repository, new BookSampleValidator())
        {
        }
    }
}
