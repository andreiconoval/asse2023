using Library.DAL.DataMapper;
using Library.DAL.DomainModel;
using Library.DAL.Interfaces;

namespace Library.DAL.Repositories
{
    public class AuthorRepository : BaseRepository<Author>, IAuthorRepository
    {
    }
}
