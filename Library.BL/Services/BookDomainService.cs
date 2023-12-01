using Library.BL.Interfaces;
using Library.BL.Validators;
using Library.DAL.DomainModel;
using Library.DAL.Interfaces;

namespace Library.BL.Services
{
    public class BookDomainService : BaseService<BookDomain, IBookDomainRepository>, IBookDomainService
    {
        public BookDomainService(IBookDomainRepository repository) : base(repository, new BookDomainValidator())
        {
        }
    }
}
