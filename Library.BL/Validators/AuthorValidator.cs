using FluentValidation;
using Library.DAL.DomainModel;

namespace Library.BL.Validators
{
    class AuthorValidator : AbstractValidator<Author>
    {
        public AuthorValidator()
        {
            RuleFor(author => author.FirstName).NotEmpty();
            RuleFor(author => author.LastName).NotEmpty();
        }
    }
}
