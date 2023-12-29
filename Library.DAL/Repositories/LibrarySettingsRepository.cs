using Library.DAL.DataMapper;
using Library.DAL.DomainModel;
using Library.DAL.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace Library.DAL.Repositories;

[ExcludeFromCodeCoverage]
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
