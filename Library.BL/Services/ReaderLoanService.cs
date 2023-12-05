using Library.BL.Interfaces;
using Library.BL.Validators;
using Library.DAL.DomainModel;
using Library.DAL.Interfaces;
using Microsoft.Extensions.Logging;

namespace Library.BL.Services
{
    public class ReaderLoanService : BaseService<ReaderLoan, IReaderLoanRepository>, IReaderLoanService
    {
        public ReaderLoanService(IReaderLoanRepository repository, ILogger logger) : base(repository, new ReaderLoanValidator(), logger)
        {
        }
    }
}
