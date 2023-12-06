using Library.DAL.DomainModel;

namespace Library.BL.Interfaces
{
    public interface IUserService : IService<User>
    {
        void DeleteUser(int id);

        int AddUser(User user);

        void UpdateUser(User user);
    }
}
