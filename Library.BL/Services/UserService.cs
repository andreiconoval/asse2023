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

        public int AddUser(User user)
        {
            try
            {
                var result = _validator.Validate(user);

                if (user == null || !result.IsValid)
                {
                    _logger.LogInformation("Cannot add user, invalid entity");
                    throw new ArgumentException("Cannot add user, invalid entity");
                }

                var authorExists = _repository.Get(i => i.Email.Trim() == user.Email.Trim()).Any();

                if (authorExists)
                {
                    _logger.LogInformation("Cannot add user, user already exists");
                    throw new ArgumentException("Cannot add user, user already exists");
                }

                _repository.Insert(user);

                return user.Id;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw;
            }
        }

        public void DeleteUser(int id)
        {
            try
            {
                var user = GetByID(id);
                if (user == null)
                {
                    _logger.LogInformation($"Cannot delete user with id: {id}, id is invalid");
                    throw new ArgumentException($"Cannot delete user with id: {id}, id is invalid");
                }

                base.Delete(user);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw;
            }
        }

        public void UpdateUser(User user)
        {
            var result = _validator.Validate(user);

            if (user == null || user.Id == 0 || !result.IsValid)
            {
                _logger.LogInformation("Cannot update user, invalid entity");
                throw new ArgumentException("Cannot update user, invalid entity");
            }

            var databaseAuthor = _repository
                .Get(i => i.Id == user.Id || i.Email.Trim().ToLower() == user.Email.Trim().ToLower())
                .FirstOrDefault();

            if (databaseAuthor == null)
            {
                _logger.LogInformation("Cannot update user, entity is missing");
                throw new ArgumentException("Cannot update user, entity is missing");
            }

            databaseAuthor.FirstName = user.FirstName;
            databaseAuthor.LastName = user.LastName;
            databaseAuthor.Phone = user.Phone;
            databaseAuthor.Address = user.Address;

            _repository.Update(databaseAuthor);
        }
    }
}
