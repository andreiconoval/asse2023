using Library.BL.Interfaces;
using Library.BL.Validators;
using Library.DAL.DomainModel;
using Library.DAL.Interfaces;

namespace Library.BL.Services
{
    public class LibraryStaffService : BaseService<LibraryStaff, ILibraryStaffRepository>, ILibraryStaffService
    {
        public LibraryStaffService(ILibraryStaffRepository repository) : base(repository, new LibraryStaffValidator())
        {
        }
    }
}
