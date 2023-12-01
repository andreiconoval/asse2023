using Library.BL.Interfaces;
using Library.BL.Validators;
using Library.DAL.DomainModel;
using Library.DAL.Repositories;

namespace Library.BL.Services
{
    public class AuthorService : BaseService<Author, AuthorRepository>, IAuthorService
    {
        public AuthorService(AuthorRepository repository) : base(repository, new AuthorValidator())
        {
        }
    }
}
