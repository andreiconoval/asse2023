using FluentValidation.Results;
using Library.BL.Interfaces;
using Library.BL.Validators;
using Library.DAL.DomainModel;
using Library.DAL.Interfaces;
using Microsoft.Extensions.Logging;

namespace Library.BL.Services
{
    public class BookDomainService : BaseService<BookDomain, IBookDomainRepository>, IBookDomainService
    {
        private readonly IDomainRepository _domainRepository;


        public BookDomainService(IBookDomainRepository repository,
            IDomainRepository domainRepository,
            ILogger logger)
            : base(repository, new BookDomainValidator(), logger)
        {
            _domainRepository = domainRepository;
        }

        /// <summary>
        /// Insert new bookd domain
        /// </summary>
        /// <param name="bookDomain">Book domain relation</param>
        /// <returns>Validation result</returns>
        /// <exception cref="ArgumentException"></exception>
        public override ValidationResult Insert(BookDomain bookDomain)
        {
            try
            {
                var result = _validator.Validate(bookDomain);

                if (bookDomain == null || !result.IsValid)
                {
                    _logger.LogInformation("Cannot add book domain, invalid entity");
                    throw new ArgumentException("Cannot add book domain, invalid entity");
                }

                var bookDomains = _repository.Get(i => i.BookId == bookDomain.BookId).ToList();
                var bookDomainExists = bookDomains.Any(i => i.DomainId == bookDomain.DomainId);

                if (bookDomainExists)
                {
                    _logger.LogInformation("Cannot add book domain, book domain already exists");
                    throw new ArgumentException("Cannot add book domain, book domain already exists");
                }

                var domains = _domainRepository.Get().ToList();

                if (IsDomainBookRelationValid(bookDomain, bookDomains, domains))
                {
                    _logger.LogInformation("Cannot add book domain, the ancestor-descendant relationship is not valid!");
                    throw new ArgumentException("Cannot add book domain, the ancestor-descendant relationship is not valid!");
                }
                _repository.Insert(bookDomain);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Check if new book domain doesn't have parent or is not oarent for existing domain
        /// </summary>
        /// <param name="newRelation">New book domain relation</param>
        /// <param name="bookDomains">Current book domains</param>
        /// <param name="domains">All domains</param>
        /// <returns>True if meet parent child relation</returns>
        /// <exception cref="ArgumentException"></exception>
        private bool IsDomainBookRelationValid(BookDomain newRelation, List<BookDomain> bookDomains, List<Domain> domains)
        {
            if (newRelation is null) return false;

            var domain = domains.Find(d => d.Id == newRelation.DomainId);
            if (domain == null)
            {
                _logger.LogInformation("Cannot add book domain, the domain is not valid!");
                throw new ArgumentException("Cannot add book domain, the domain is not valid!");
            }
            var newDomainAncestor = GetDomainAncestor(domain.Id, domains);

            foreach (var bookDomain in bookDomains)
            {
                if (bookDomain == null || bookDomain.Domain == null || newDomainAncestor == GetDomainAncestor(bookDomain.Domain.Id, domains))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Method to find domains ancestors
        /// </summary>
        /// <param name="id">Domains id</param>
        /// <param name="domains">List of domains to search</param>
        /// <returns>Ancestor domain id</returns>
        private int? GetDomainAncestor(int id, List<Domain> domains)
        {
            var ancestor = domains.FirstOrDefault(i => i.Id == id);
            if (ancestor != null && ancestor.DomainId != null)
            {
                return GetDomainAncestor(ancestor.DomainId.Value, domains);
            }

            return ancestor?.Id;
        }
    }
}
