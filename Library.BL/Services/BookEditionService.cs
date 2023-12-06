using FluentValidation;
using FluentValidation.Results;
using Library.BL.Interfaces;
using Library.BL.Validators;
using Library.DAL.DomainModel;
using Library.DAL.Interfaces;
using Microsoft.Extensions.Logging;

namespace Library.BL.Services
{
    public class BookEditionService : BaseService<BookEdition, IBookEditionRepository>, IBookEditionService
    {
        private readonly IBookRepository _bookRepository;

        public BookEditionService(IBookEditionRepository repository, ILogger logger, IBookRepository bookRepository) : base(repository, new BookEditionValidator(), logger)
        {
            _bookRepository = bookRepository;
        }

        /// <summary>
        /// Add new book edition
        /// </summary>
        /// <param name="entity">Book edition</param>
        /// <returns>Validation result and fill entity model with  id</returns>
        /// <exception cref="ArgumentException"></exception>
        public override ValidationResult Insert(BookEdition entity)
        {
            try
            {
                var result = _validator.Validate(entity);

                if (!result.IsValid)
                {
                    _logger.LogInformation($"Cannot add new book edition, entity is invalid");
                    throw new ArgumentException("Cannot add new book edition, entity is invalid");
                }

                var editionExists = _repository.Get(i => i.Edition.Trim().ToLower() == entity.Edition.Trim().ToLower()).Any();

                if (editionExists)
                {
                    _logger.LogInformation($"Cannot add new book edition, entity already exists");
                    throw new ArgumentException("Cannot add new book edition, entity already exists");
                }

                var bookExists = _bookRepository.Get(i => i.Id == entity.BookId).Any();

                if (!bookExists)
                {
                    _logger.LogInformation($"Cannot add new book edition, book is missing");
                    throw new ArgumentException("Cannot add new book edition, book is missing");
                }

                _repository.Insert(entity);
                _logger.LogInformation($"Add new book edition");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Log on error add");
                throw;
            }
        }


        /// <summary>
        /// Update book edition based on id
        /// </summary>
        /// <param name="bookEdition">Book edition entity</param>
        /// <returns>Validation result for entity</returns>
        /// <exception cref="ArgumentException"></exception>
        public override ValidationResult Update(BookEdition bookEdition)
        {
            try
            {
                var result = _validator.Validate(bookEdition);

                if (!result.IsValid)
                {
                    _logger.LogInformation("Cannot update book edition, invalid entity");
                    throw new ArgumentException("Cannot update book edition, invalid entity");
                }

                var databaseBookEdition = _repository.Get(i => i.Id == bookEdition.Id).FirstOrDefault();
                if (databaseBookEdition == null)
                {
                    _logger.LogInformation("Cannot update book edition, entity is missing");
                    throw new ArgumentException("Cannot update book edition, entity is missing");
                }

                if(databaseBookEdition.BookId != bookEdition.BookId)
                {
                    var bookExists = _bookRepository.Get(i => i.Id == bookEdition.BookId).Any();
                    if (!bookExists)
                    {
                        _logger.LogInformation($"Cannot add update book edition, book is missing");
                        throw new ArgumentException("Cannot add update book edition, book is missing");
                    }
                }

                databaseBookEdition.Edition = bookEdition.Edition;
                databaseBookEdition.PageNumber = bookEdition.PageNumber;
                databaseBookEdition.BookId = bookEdition.BookId;
                databaseBookEdition.BookType = bookEdition.BookType;
                databaseBookEdition.ReleaseYear= bookEdition.ReleaseYear;
                _repository.Update(databaseBookEdition);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Log on error update");
                throw;
            }
        }


        /// <summary>
        /// Delete book edition from database based on Id.
        /// But will throw error if book edition have BookSamples
        /// </summary>
        /// <param name="bookEdition">Book edition to delete by Id</param>
        /// <exception cref="ArgumentException"></exception>
        public override void Delete(BookEdition bookEdition)
        {
            try
            {
                var fullDatabaseBook = _repository.Get(i => i.Id == bookEdition.Id, null, "BookSamples").FirstOrDefault();
                if (fullDatabaseBook == null)
                {
                    _logger.LogInformation("Cannot delete book edition, entity is missing");
                    throw new ArgumentException("Cannot delete book edition, entity is missing");
                }

                if (fullDatabaseBook.BookSamples.Any())
                {
                    _logger.LogInformation("Cannot delete book edition, entity has relations");
                    throw new ArgumentException("Cannot delete book edition, entity has relations");
                }

                base.Delete(bookEdition);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Log on error update");
                throw;
            }
        }
    }
}
