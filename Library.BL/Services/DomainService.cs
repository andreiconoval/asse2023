using Library.BL.Interfaces;
using Library.BL.Validators;
using Library.DAL.DomainModel;
using Library.DAL.Interfaces;
using Microsoft.Extensions.Logging;

namespace Library.BL.Services
{
    public class DomainService : BaseService<Domain, IDomainRepository>, IDomainService
    {
        public DomainService(IDomainRepository repository, ILogger logger) : base(repository, new DomainValidator(), logger)
        {
        }
    }
}
