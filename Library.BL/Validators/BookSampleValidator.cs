using FluentValidation;
using Library.DAL.DomainModel;

namespace Library.BL.Validators
{
    public class BookSampleValidator: AbstractValidator<BookSample>
    {
        public BookSampleValidator()
        {
            RuleFor(book => book.BookEditionId).NotEmpty();
            RuleFor(book => book)
             .Must(book => book.AvailableForLoan || book.AvailableForHall)
             .WithMessage("At least one of AvailableForLoan or AvailableForHall should be true.");
        }
    }
}
