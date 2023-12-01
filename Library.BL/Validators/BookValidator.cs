using FluentValidation;
using Library.DAL.DomainModel;

namespace Library.BL.Validators
{
    class BookValidator : AbstractValidator<Book>
    {
        public BookValidator()
        {
            RuleFor(book => book.Title).NotEmpty().MaximumLength(255).MinimumLength(3);
            RuleFor(book => book.YearPublication).NotEmpty().GreaterThan(0);
        }
    }
}
