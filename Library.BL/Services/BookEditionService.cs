using Library.BL.Interfaces;
using Library.BL.Validators;
using Library.DAL.DomainModel;
using Library.DAL.Interfaces;

namespace Library.BL.Services
{
    public class BookEditionService : BaseService<BookEdition, IBookEditionRepository>, IBookEditionService
    {
        public BookEditionService(IBookEditionRepository repository) : base(repository, new BookEditionValidator())
        {
        }
    }
}
