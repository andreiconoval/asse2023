using FluentValidation;
using Library.DAL.DomainModel;

namespace Library.BL.Validators
{
    class BookEditionValidator : AbstractValidator<BookEdition>
    {
        public BookEditionValidator()
        {
            RuleFor(bookEdition => bookEdition.BookId).GreaterThan(0);
            RuleFor(bookEdition => bookEdition.PageNumber).GreaterThan(0);
            RuleFor(bookEdition => bookEdition.BookType).NotEmpty().MaximumLength(255);
            RuleFor(bookEdition => bookEdition.Edition).NotEmpty().MaximumLength(255);
            RuleFor(bookEdition => bookEdition.ReleaseYear).GreaterThan(0);
        }
    }
}
