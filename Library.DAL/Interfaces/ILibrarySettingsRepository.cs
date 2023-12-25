using Library.DAL.DomainModel;

namespace Library.DAL.Interfaces;

public interface ILibrarySettingsRepository
{
    LibrarySettings Get();
}