using Library.BL.Interfaces;
using Library.BL.Validators;
using Library.DAL.DomainModel;
using Library.DAL.Interfaces;

namespace Library.BL.Services
{
    public class DomainService : BaseService<Domain, IDomainRepository>, IDomainService
    {
        public DomainService(IDomainRepository repository) : base(repository, new DomainValidator())
        {
        }
    }
}
