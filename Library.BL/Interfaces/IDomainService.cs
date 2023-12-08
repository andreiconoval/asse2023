using Library.DAL.DomainModel;

namespace Library.BL.Interfaces
{
    public interface IDomainService : IService<Domain>
    {
        void Delete(Domain domain, bool hardDelete);
    }
}
