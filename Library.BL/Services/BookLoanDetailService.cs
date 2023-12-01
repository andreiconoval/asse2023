using Library.BL.Interfaces;
using Library.BL.Validators;
using Library.DAL.DomainModel;
using Library.DAL.Interfaces;

namespace Library.BL.Services
{
    public class BookLoanDetailService : BaseService<BookLoanDetail, IBookLoanDetailRepository>, IBookLoanDetailService
    {
        public BookLoanDetailService(IBookLoanDetailRepository repository) : base(repository, new BookLoanDetailValidator())
        {
        }
    }
}
