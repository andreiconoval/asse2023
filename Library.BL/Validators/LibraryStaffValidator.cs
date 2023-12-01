using FluentValidation;
using Library.DAL.DomainModel;

namespace Library.BL.Validators
{
    class LibraryStaffValidator : AbstractValidator<LibraryStaff>
    {
        public LibraryStaffValidator()
        {
            RuleFor(staff => staff.UserId).NotEmpty();
        }
    }
}
