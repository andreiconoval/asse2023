using Library.BL.Interfaces;
using Library.BL.Validators;
using Library.DAL.DomainModel;
using Library.DAL.Interfaces;
using Microsoft.Extensions.Logging;

namespace Library.BL.Services
{
    public class BookDomainService : BaseService<BookDomain, IBookDomainRepository>, IBookDomainService
    {
        public BookDomainService(IBookDomainRepository repository, ILogger logger) : base(repository, new BookDomainValidator(), logger)
        {
        }
    }
}
