using Library.DAL.DataMapper;
using Library.DAL.DomainModel;
using Library.DAL.Interfaces;

namespace Library.DAL.Repositories
{
    public class BookDomainRepository : BaseRepository<BookDomain>, IBookDomainRepository
    {
    }
}
