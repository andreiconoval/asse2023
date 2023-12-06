using Library.DAL.DomainModel;

namespace Library.BL.Interfaces
{
    public interface IAuthorService : IService<Author>
    {
        void DeleteAuthor(int id, bool hardDelete);

        IEnumerable<BookAuthor> GetAuthorBooks(int iD);
        
        int AddAuthor(Author author);

        void UpdateAuthor(Author author);

    }
}
