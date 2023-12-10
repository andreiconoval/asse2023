using Library.DAL.DomainModel;

namespace Library.BL.Interfaces
{
    public interface IBookService : IService<Book>
    {
        void Delete(Book book, bool hardDelete);
    }
}
