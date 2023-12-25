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

                var bookDomainExists = _repository.Get(i => i.BookId == bookDomain.BookId && i.DomainId == bookDomain.DomainId).Any();

                if (bookDomainExists)
                {
                    _logger.LogInformation("Cannot add book domain, book domain already exists");
                    throw new ArgumentException("Cannot add book domain, book domain already exists");
                }

                var domains = _domainRepository.Get().ToList();
                var bookDomains = _repository.Get(i => i.BookId == bookDomain.BookId).ToList();

                if (IsDomainBookRelationValid(bookDomain, bookDomains, domains))
                {
                    _logger.LogInformation("Eroare: Relația stramos-descendent nu este validă!");
                    throw new ArgumentException("Eroare: Relația stramos-descendent nu este validă!");
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
                _logger.LogInformation("Eroare: Domeniul specificat nu există!");
                throw new ArgumentException("Eroare: Domeniul specificat nu există!");
            }

            var hasParentInRelations = true;

            foreach (var bookDomain in bookDomains)
            {
                var parentDomain = domains.Find(d => d.Id == bookDomain.DomainId);
                if (parentDomain == null)
                {
                    _logger.LogInformation("Eroare: Domeniul specificat in relatie nu există!");
                    throw new ArgumentException("Eroare: Domeniul specificat in relatie nu există!");
                }

                if (parentDomain.Id == domain.DomainId || domain.Id == parentDomain.DomainId)
                {
                    hasParentInRelations = false;
                }
            }

            return hasParentInRelations;
        }

    }
}
