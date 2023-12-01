using FluentValidation;
using Library.DAL.DomainModel;

namespace Library.BL.Validators
{
    class ReaderValidator : AbstractValidator<Reader>
    {
        public ReaderValidator()
        {
            RuleFor(reader => reader.UserId).NotEmpty();
        }
    }
}
