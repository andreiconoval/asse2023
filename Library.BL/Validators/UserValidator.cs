using FluentValidation;
using Library.DAL.DomainModel;
using System.Diagnostics.CodeAnalysis;

namespace Library.BL.Validators
{
    class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(user => user.FirstName).NotEmpty().MaximumLength(255);
            RuleFor(user => user.LastName).NotEmpty().MaximumLength(255);
            RuleFor(user => user.Address).MaximumLength(255);
            RuleFor(user => user.Phone).MaximumLength(15);
            RuleFor(user => user.Email).NotEmpty().MaximumLength(255);
        }
    }
}
