using Library.BL.Interfaces;
using Library.BL.Validators;
using Library.DAL.DomainModel;
using Library.DAL.Interfaces;
using Microsoft.Extensions.Logging;

namespace Library.BL.Services
{
    public class UserService : BaseService<User, IUserRepository>, IUserService
    {
        public UserService(IUserRepository repository, ILogger<IUserService> logger) : base(repository, new UserValidator(), logger)
        {
        }
    }
}
