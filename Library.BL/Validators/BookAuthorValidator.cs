using FluentValidation;
using Library.DAL.DomainModel;

namespace Library.BL.Validators
{
    class BookAuthorValidator : AbstractValidator<BookAuthor>
    {
        public BookAuthorValidator()
        {
            RuleFor(author => author.AuthorId).GreaterThan(0);
            RuleFor(author => author.BookId).GreaterThan(0);
        }
    }
}
