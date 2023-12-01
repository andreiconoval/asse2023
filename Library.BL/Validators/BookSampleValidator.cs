using FluentValidation;
using Library.DAL.DomainModel;

namespace Library.BL.Validators
{
    public class BookSampleValidator: AbstractValidator<BookSample>
    {
        public BookSampleValidator()
        {
            RuleFor(book => book.BookEditionId).NotEmpty();

        }
    }
}
