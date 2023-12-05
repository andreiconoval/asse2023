using Library.BL.Interfaces;
using Library.BL.Validators;
using Library.DAL.DomainModel;
using Library.DAL.Interfaces;
using Microsoft.Extensions.Logging;

namespace Library.BL.Services
{
    public class BookLoanDetailService : BaseService<BookLoanDetail, IBookLoanDetailRepository>, IBookLoanDetailService
    {
        public BookLoanDetailService(IBookLoanDetailRepository repository, ILogger logger) : base(repository, new BookLoanDetailValidator(), logger)
        {
        }
    }
}
