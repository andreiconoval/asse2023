using FluentValidation.Results;
using Library.BL.Interfaces;
using Library.BL.Validators;
using Library.DAL.DomainModel;
using Library.DAL.Interfaces;
using Microsoft.Extensions.Logging;

namespace Library.BL.Services
{
    public class BookService : BaseService<Book, IBookRepository>, IBookService
    {
        private readonly IBookDomainService _bookDomainService;
        private readonly IBookAuthorService _bookAuthorService;

        public BookService(
            IBookRepository repository, 
            ILogger logger, 
            IBookDomainService bookDomainService, 
            IBookAuthorService bookAuthorService) 
            : base(repository, new BookValidator(), logger)
        {
            _bookDomainService = bookDomainService;
            _bookAuthorService = bookAuthorService;
        }

        #region Public

        /// <summary>
        /// Add new book
        /// </summary>
        /// <param name="entity">Book entity</param>
        /// <returns>Validation result, Filled entity id with new one</returns>
        /// <exception cref="ArgumentException"></exception>
        public override ValidationResult Insert(Book entity)
        {
            try
            {
                var result = _validator.Validate(entity);

                if (!result.IsValid)
                {
                    _logger.LogInformation($"Cannot add new book, entity is invalid");
                    throw new ArgumentException("Cannot add new book, entity is invalid");
                }

                var bookExists = _repository.Get(i => i.Title.Trim().ToLower() == entity.Title.Trim().ToLower()).Any();

                if (bookExists)
                {
                    _logger.LogInformation($"Cannot add new book, entity already exists");
                    throw new ArgumentException("Cannot add new book, entity already exists");
                }

                _repository.Insert(entity);
                _logger.LogInformation($"Add new book");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Log on error add");
                throw;
            }
        }


        /// <summary>
        /// Update book based on id
        /// </summary>
        /// <param name="book">Book entity</param>
        /// <returns>Validation result for entity</returns>
        /// <exception cref="ArgumentException"></exception>
        public override ValidationResult Update(Book book)
        {
            try
            {
                var result = _validator.Validate(book);

                if (!result.IsValid)
                {
                    _logger.LogInformation("Cannot update book, invalid entity");
                    throw new ArgumentException("Cannot update book, invalid entity");
                }

                var databaseDomain = _repository.Get(i => i.Id == book.Id).FirstOrDefault();
                if (databaseDomain == null)
                {
                    _logger.LogInformation("Cannot update book, entity is missing");
                    throw new ArgumentException("Cannot update book, entity is missing");
                }

                databaseDomain.Title = book.Title;
                databaseDomain.YearPublication = book.YearPublication;
                _repository.Update(databaseDomain);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Log on error update");
                throw;
            }
        }

        /// <summary>
        /// Delete book from database based on Id.
        /// Hard delete will remove also all Book Domains and BookAuthor relations with hard delete flag,
        /// But will throw error if book have editions, instead of this mark book archived
        /// </summary>
        /// <param name="book">Book to delete by Id</param>
        /// <param name="hardDelete"> Flag that indicate to remove  book full</param>
        /// <exception cref="ArgumentException"></exception>
        public void Delete(Book book, bool hardDelete)
        {
            try
            {
                var fullDatabaseBook = _repository.Get(i => i.Id == book.Id, null, "BookDomains,BookAuthors,BookEditions").FirstOrDefault();
                if (fullDatabaseBook == null)
                {
                    _logger.LogInformation("Cannot delete book, entity is missing");
                    throw new ArgumentException("Cannot delete book, entity is missing");
                }

                if (!hardDelete && (fullDatabaseBook.BookDomains.Any() || fullDatabaseBook.BookDomains.Any()))
                {
                    _logger.LogInformation("Cannot delete book, entity has relations");
                    throw new ArgumentException("Cannot delete domain, entity has relations");
                }

                if(fullDatabaseBook.BookEditions.Any())
                {
                    _logger.LogInformation("Cannot delete book with editions, mark archived instead");
                    throw new ArgumentException("Cannot delete domain with editions, mark archived instead");
                }

                if (fullDatabaseBook.BookDomains.Any())
                {
                    foreach (var bookDomain in fullDatabaseBook.BookDomains)
                    {
                        _bookDomainService.Delete(bookDomain);
                    }
                }

                if (fullDatabaseBook.BookAuthors.Any())
                {
                    foreach (var bookAuthor in fullDatabaseBook.BookAuthors)
                    {
                        _bookAuthorService.Delete(bookAuthor);
                    }
                }

                base.Delete(book);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Log on error update");
                throw;
            }
        }

        #endregion
    }
}
