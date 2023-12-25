using FluentValidation.Results;
using Library.BL.Interfaces;
using Library.BL.Validators;
using Library.DAL.DomainModel;
using Library.DAL.Interfaces;
using Microsoft.Extensions.Logging;

namespace Library.BL.Services
{
    public class BookSampleService : BaseService<BookSample, IBookSampleRepository>, IBookSampleService
    {
        private readonly IBookEditionRepository _bookEditionRepository;
        private readonly IBookLoanDetailRepository _bookLoanDetailRepository;

        public BookSampleService(IBookSampleRepository repository,
            IBookEditionRepository bookEditionRepository,
            IBookLoanDetailRepository bookLoanDetailRepository,
            ILogger logger)
            : base(repository, new BookSampleValidator(), logger)
        {
            _bookEditionRepository = bookEditionRepository;
            _bookLoanDetailRepository = bookLoanDetailRepository;
        }

        public override ValidationResult Insert(BookSample bookSample)
        {
            try
            {
                var result = _validator.Validate(bookSample);

                if (bookSample == null || !result.IsValid)
                {
                    _logger.LogInformation("Cannot add book sample, invalid entity");
                    throw new ArgumentException("Cannot add book sample, invalid entity");
                }

                var bookEditionExists = _bookEditionRepository.Get(i => i.Id == bookSample.BookEditionId).Any();

                if (bookEditionExists)
                {
                    _logger.LogInformation("Cannot add book sample, book edition do not exists");
                    throw new ArgumentException("Cannot add book sample, book edition do not exists");
                }

                _repository.Insert(bookSample);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw;
            }
        }


        public override ValidationResult Update(BookSample bookSample)
        {
            try
            {
                var result = _validator.Validate(bookSample);

                if (bookSample == null || !result.IsValid)
                {
                    _logger.LogInformation("Cannot update book sample, invalid entity");
                    throw new ArgumentException("Cannot update book sample, invalid entity");
                }

                var bookSampleExists = _repository.Get(i => i.Id == bookSample.Id).Any();

                if (bookSampleExists)
                {
                    _logger.LogInformation("Cannot update book sample, book sample do not exists");
                    throw new ArgumentException("Cannot update book sample, book sample do not exists");
                }

                var bookEditionExists = _bookEditionRepository.Get(i => i.Id == bookSample.BookEditionId).Any();

                if (bookEditionExists)
                {
                    _logger.LogInformation("Cannot add book sample, book edition do not exists");
                    throw new ArgumentException("Cannot add book sample, book edition do not exists");
                }

                _repository.Update(bookSample);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw;
            }
        }


        public override void Delete(BookSample bookSample)
        {
            if (bookSample is null || bookSample.Id == 0)
            {
                _logger.LogInformation("Cannot delete book sample, invalid entity");
                throw new ArgumentException("Cannot delete book sample, invalid entity");
            }


            var hasActiveBookLoan = _bookLoanDetailRepository.Get(bld => bld.BookSampleId == bookSample.Id && bld.EffectiveReturnDate != null).Any();
            if (hasActiveBookLoan)
            {
                _logger.LogInformation("Cannot delete book sample, book is in use");
                throw new ArgumentException("Cannot delete book sample, book is in use");
            }

            base.Delete(bookSample);
        }

    }
}
