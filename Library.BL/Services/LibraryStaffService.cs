using Library.BL.Interfaces;
using Library.BL.Validators;
using Library.DAL.DomainModel;
using Library.DAL.Interfaces;
using Microsoft.Extensions.Logging;

namespace Library.BL.Services
{
    public class LibraryStaffService : BaseService<LibraryStaff, ILibraryStaffRepository>, ILibraryStaffService
    {
        public LibraryStaffService(ILibraryStaffRepository repository, ILogger logger) : base(repository, new LibraryStaffValidator(), logger)
        {
        }
    }
}
