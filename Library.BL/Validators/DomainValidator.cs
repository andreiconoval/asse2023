using FluentValidation;
using Library.DAL.DomainModel;

namespace Library.BL.Validators
{
    class DomainValidator : AbstractValidator<Domain>
    {
        public DomainValidator()
        {
            RuleFor(book => book.DomainName).NotEmpty().MaximumLength(255).MinimumLength(3);
            RuleFor(book => book.DomainId).NotEmpty().NotEqual(r => r.Id);
        }
    }
}
