using FluentValidation;
using Library.DAL.DomainModel;

namespace Library.BL.Validators
{
    class BookLoanDetailValidator : AbstractValidator<BookLoanDetail>
    {
        public BookLoanDetailValidator()
        {
            RuleFor(bookLoanDetail => bookLoanDetail.BookSampleId).GreaterThan(0);
            RuleFor(bookLoanDetail => bookLoanDetail.ReaderLoanId).GreaterThan(0);
            RuleFor(bookLoanDetail => bookLoanDetail.LoanDate).NotEmpty().GreaterThanOrEqualTo(new DateTime(2000, 1, 15));
            RuleFor(bookLoanDetail => bookLoanDetail.ExpectedReturnDate).NotEmpty().GreaterThan(r => r.LoanDate);
            RuleFor(bookLoanDetail => bookLoanDetail.EffectiveReturnDate).GreaterThan(r => r.LoanDate);
        }
    }
}
