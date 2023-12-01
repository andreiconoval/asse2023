using Library.BL.Interfaces;
using Library.BL.Validators;
using Library.DAL.DomainModel;
using Library.DAL.Interfaces;

namespace Library.BL.Services
{
    public class ReaderService : BaseService<Reader, IReaderRepository>, IReaderService
    {
        public ReaderService(IReaderRepository repository) : base(repository, new ReaderValidator())
        {
        }
    }
}
