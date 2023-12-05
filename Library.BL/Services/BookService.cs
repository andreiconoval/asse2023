using Library.BL.Interfaces;
using Library.BL.Validators;
using Library.DAL.DomainModel;
using Library.DAL.Interfaces;
using Microsoft.Extensions.Logging;

namespace Library.BL.Services
{
    public class BookService : BaseService<Book, IBookRepository>, IBookService
    {
        public BookService(IBookRepository repository, ILogger logger) : base(repository, new BookValidator(), logger)
        {
        }
    }
}
