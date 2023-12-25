using Library.BL.Interfaces;
using Library.BL.Validators;
using Library.DAL.DomainModel;
using Library.DAL.Interfaces;
using Library.DAL.Repositories;
using Microsoft.Extensions.Logging;

namespace Library.BL.Services
{
    public class BookAuthorService : BaseService<BookAuthor, IBookAuthorRepository>, IBookAuthorService
    {
        public BookAuthorService(IBookAuthorRepository repository, ILogger logger) : base(repository, new BookAuthorValidator(), logger)
        {
        }

        public void AddBookAuthor(BookAuthor bookAuthor)
        {
            try
            {
                var result = _validator.Validate(bookAuthor);

                if (bookAuthor == null || !result.IsValid)
                {
                    _logger.LogInformation("Cannot add book author, invalid entity");
                    throw new ArgumentException("Cannot add book author, invalid entity");
                }

                var bookAuthorExists = _repository.Get(i => i.BookId == bookAuthor.BookId && i.AuthorId == bookAuthor.AuthorId).Any();

                if (bookAuthorExists)
                {
                    _logger.LogInformation("Cannot add book author, book author already exists");
                    throw new ArgumentException("Cannot add book author, book author already exists");
                }

                _repository.Insert(bookAuthor);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw;
            }
        }
    }
}
