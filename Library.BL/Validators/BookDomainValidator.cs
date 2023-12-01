using FluentValidation;
using Library.DAL.DomainModel;

namespace Library.BL.Validators
{
    class BookDomainValidator : AbstractValidator<BookDomain>
    {
        public BookDomainValidator()
        {
            RuleFor(bookDomain => bookDomain.DomainId).GreaterThan(0);
            RuleFor(bookDomain => bookDomain.BookId).GreaterThan(0);
        }
    }
}
