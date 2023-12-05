using Library.BL.Infrastructure;
using Library.BL.Interfaces;
using Library.BL.Validators;
using Library.DAL.DomainModel;
using Library.DAL.Interfaces;

namespace Library.BL.Services
{
    public class AuthorService : BaseService<Author, IAuthorRepository>, IAuthorService
    {
        public AuthorService() : base(Injector.Get<IAuthorRepository>(), new AuthorValidator(), LoggerExtensions.LoggingInstance<IAuthorService>())
        {
        }
    }
}
