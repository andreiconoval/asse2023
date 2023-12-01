using Library.BL.Interfaces;
using Library.BL.Validators;
using Library.DAL.DomainModel;
using Library.DAL.Interfaces;

namespace Library.BL.Services
{
    public class ReaderLoanService : BaseService<ReaderLoan, IReaderLoanRepository>, IReaderLoanService
    {
        public ReaderLoanService(IReaderLoanRepository repository) : base(repository, new ReaderLoanValidator())
        {
        }
    }
}
