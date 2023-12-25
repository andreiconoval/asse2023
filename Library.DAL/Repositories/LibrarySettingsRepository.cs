using Library.DAL.DataMapper;
using Library.DAL.DomainModel;
using Library.DAL.Interfaces;

namespace Library.DAL.Repositories;

public class LibrarySettingsRepository : ILibrarySettingsRepository
{
    public LibrarySettingsRepository()
    {

    }

    public LibrarySettings Get()
    {
        using (var ctx = new LibraryDbContext())
        {
            return ctx.Set<LibrarySettings>().Find(1);
        }
    }
}
