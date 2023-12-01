using Library.BL.Interfaces;
using Library.BL.Validators;
using Library.DAL.DomainModel;
using Library.DAL.Interfaces;

namespace Library.BL.Services
{
    public class BookService : BaseService<Book, IBookRepository>, IBookService
    {
        public BookService(IBookRepository repository) : base(repository, new BookValidator())
        {
        }
    }
}
