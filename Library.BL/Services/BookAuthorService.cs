using FluentValidation.Results;
using Library.BL.Interfaces;
using Library.BL.Validators;
using Library.DAL.DomainModel;
using Library.DAL.Interfaces;
using Microsoft.Extensions.Logging;

namespace Library.BL.Services
{
    public class BookAuthorService : BaseService<BookAuthor, IBookAuthorRepository>, IBookAuthorService
    {
        private readonly IAuthorRepository authorRepository;
        private readonly IBookRepository bookRepository;

        public BookAuthorService(IBookAuthorRepository repository, IAuthorRepository authorRepository, IBookRepository bookRepository, ILogger logger) : base(repository, new BookAuthorValidator(), logger)
        {
            this.authorRepository = authorRepository;
            this.bookRepository = bookRepository;
        }

        public override ValidationResult Insert(BookAuthor bookAuthor)
        {
            try
            {
                var result = _validator.Validate(bookAuthor);

                if (bookAuthor == null || !result.IsValid)
                {
                    throw new ArgumentException("Cannot add book author, invalid entity");
                }

                var bookAuthorExists = _repository.Get(i => i.BookId == bookAuthor.BookId && i.AuthorId == bookAuthor.AuthorId).Any();

                if (bookAuthorExists)
                {
                    throw new ArgumentException("Cannot add book author, book author already exists");
                }

                var bookExists = bookRepository.Get(i => i.Id == bookAuthor.BookId).Any();

                if (!bookExists)
                {
                    throw new ArgumentException("Cannot add book author, book does not exist");
                }

                var authorExists = authorRepository.Get(i => i.Id == bookAuthor.AuthorId).Any();

                if (!authorExists)
                {
                    throw new ArgumentException("Cannot add book author, author does not exist");
                }

                _repository.Insert(bookAuthor);

                return result;
            }
            catch (ArgumentException ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw;
            }
        }

        public override ValidationResult Update(BookAuthor entity)
        {
            throw new Exception("To delete and add new Book author is the best approach");
        }
    }
}
