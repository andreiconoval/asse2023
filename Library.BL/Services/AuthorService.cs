using Library.BL.Interfaces;
using Library.BL.Validators;
using Library.DAL.DomainModel;
using Library.DAL.Interfaces;
using Microsoft.Extensions.Logging;

namespace Library.BL.Services
{
    public class AuthorService : BaseService<Author, IAuthorRepository>, IAuthorService
    {
        private readonly IBookAuthorService _bookAuthorservice;

        public AuthorService(IAuthorRepository authorRepository, IBookAuthorService bookAuthorService, ILogger<IAuthorService> logger) : base(authorRepository, new AuthorValidator(), logger)
        {
            _bookAuthorservice = bookAuthorService;
        }

        #region Public

        public void DeleteAuthor(int id, bool hardDelete)
        {
            try
            {
                var author = GetByID(id);
                if (author == null)
                {
                    _logger.LogInformation($"Cannot delete author with id: {id}, id is invalid");
                    throw new ArgumentException($"Cannot delete author with id: {id}, id is invalid");
                }

                var bookAuthors = GetAuthorBooks(id);

                if ((!bookAuthors?.Any() ?? true) || hardDelete)
                {
                    if (hardDelete && bookAuthors != null)
                    {
                        foreach (var bookAuthor in bookAuthors)
                        {
                            _bookAuthorservice.Delete(bookAuthor);
                        }

                        _logger.LogInformation($"All links to books was deleted");
                    }
                    base.Delete(author);
                }
                else
                {
                    _logger.LogInformation($"Cannot delete author with id: {id}, there are books liked to it");
                    throw new ArgumentException($"Cannot delete author with id: {id}, there are books liked to it");
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw;
            }
        }

        public IEnumerable<BookAuthor> GetAuthorBooks(int id)
        {
            var author = _repository.Get(i => i.Id == id, null, "BookAuthors").FirstOrDefault();
            if (author == null)
            {
                _logger.LogInformation("Author doesn't exist");
                throw new ArgumentException("Author doesn't exist");
            }

            var books = author.BookAuthors;
            return books;
        }


        public int AddAuthor(Author author)
        {
            try
            {
                var result = _validator.Validate(author);

                if (author == null || !result.IsValid)
                {
                    _logger.LogInformation("Cannot add author, invalid entity");
                    throw new ArgumentException("Cannot add author, invalid entity");
                }

                var authorExists = _repository.Get(i => i.FirstName.Trim() == author.FirstName.Trim() && i.LastName.Trim() == author.LastName.Trim()).Any();

                if (authorExists)
                {
                    _logger.LogInformation("Cannot add author, author already exists");
                    throw new ArgumentException("Cannot add author, author already exists");
                }

                _repository.Insert(author);

                return author.Id;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw;
            }
        }

        public void UpdateAuthor(Author author)
        {
            var result = _validator.Validate(author);

            if (author == null || author.Id == 0 || !result.IsValid)
            {
                _logger.LogInformation("Cannot update author, invalid entity");
                throw new ArgumentException("Cannot update author, invalid entity");
            }

            var databaseAuthor = _repository.Get(i => i.Id == author.Id).FirstOrDefault();

            if (databaseAuthor == null)
            {
                _logger.LogInformation("Cannot update author, entity is missing");
                throw new ArgumentException("Cannot update author, entity is missing");
            }

            databaseAuthor.FirstName = author.FirstName;
            databaseAuthor.LastName = author.LastName;

            _repository.Update(databaseAuthor);
        }

        #endregion
    }
}
