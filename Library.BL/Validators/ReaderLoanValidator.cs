using FluentValidation;
using Library.DAL.DomainModel;

namespace Library.BL.Validators
{
    class ReaderLoanValidator : AbstractValidator<ReaderLoan>
    {
        public ReaderLoanValidator()
        {
            RuleFor(readerLoan => readerLoan.ReaderId).NotEmpty();
            RuleFor(readerLoan => readerLoan.LoanDate).NotEmpty().GreaterThanOrEqualTo(new DateTime(2000, 1, 15));
            RuleFor(readerLoan => readerLoan.ExpectedReturnDate).NotEmpty().GreaterThan(r => r.LoanDate);
            RuleFor(readerLoan => readerLoan.EffectiveReturnDate).GreaterThan(r => r.LoanDate);
            RuleFor(readerLoan => readerLoan.BorrowedBooks).NotEmpty();
            RuleFor(readerLoan => readerLoan.BookLoanDetails).NotEmpty().Must((readerLoan, readerLoanDetails) => readerLoanDetails == null || readerLoanDetails.Count == readerLoan.BorrowedBooks); ;
        }
    }
}
