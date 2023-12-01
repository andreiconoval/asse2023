using Library.BL.Interfaces;
using Library.BL.Validators;
using Library.DAL.DomainModel;
using Library.DAL.Repositories;

namespace Library.BL.Services
{
    public class BookAuthorService : BaseService<BookAuthor, BookAuthorRepository>, IBookAuthorService
    {
        public BookAuthorService(BookAuthorRepository repository) : base(repository, new BookAuthorValidator())
        {
        }
    }
}
