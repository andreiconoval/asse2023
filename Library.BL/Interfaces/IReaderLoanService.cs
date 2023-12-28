using Library.DAL.DomainModel;

namespace Library.BL.Interfaces;

public interface IReaderLoanService
{
    ReaderLoan Insert(ReaderLoan loan);
}
