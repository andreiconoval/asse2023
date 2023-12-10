using FluentValidation.Results;
using Library.BL.Interfaces;
using Library.BL.Validators;
using Library.DAL.DomainModel;
using Library.DAL.Interfaces;
using Microsoft.Extensions.Logging;

namespace Library.BL.Services
{
    public class DomainService : BaseService<Domain, IDomainRepository>, IDomainService
    {
        private readonly IBookDomainService _bookDomainService;
        public DomainService(IDomainRepository repository, ILogger logger, IBookDomainService bookDomainService) : base(repository, new DomainValidator(), logger)
        {
            _bookDomainService = bookDomainService;
        }

        /// <summary>
        /// Add new domain
        /// </summary>
        /// <param name="entity">Domain</param>
        /// <returns>Validation Result</returns>
        /// <exception cref="ArgumentException"></exception>
        public override ValidationResult Insert(Domain entity)
        {
            try
            {
                var result = _validator.Validate(entity);

                if (!result.IsValid)
                {
                    _logger.LogInformation($"Cannot add new domain, entity is invalid");
                    throw new ArgumentException("Cannot add new domain, entity is invalid");
                }

                var domainExists = _repository.Get(i => i.DomainName.Trim() == entity.DomainName.Trim()).Any();

                if (domainExists)
                {
                    _logger.LogInformation($"Cannot add new domain, entity already exists");
                    throw new ArgumentException("Cannot add new domain, entity already exists");
                }

                _repository.Insert(entity);
                _logger.LogInformation($"Add new domain");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Log on error add");
                throw;
            }
        }

        /// <summary>
        /// Update domain name based on id
        /// </summary>
        /// <param name="domain">Domain entity</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public override ValidationResult Update(Domain domain)
        {
            try
            {
                var result = _validator.Validate(domain);

                if (!result.IsValid)
                {
                    _logger.LogInformation("Cannot update domain, invalid entity");
                    throw new ArgumentException("Cannot update domain, invalid entity");
                }

                var databaseDomain = _repository.Get(i => i.Id == domain.Id).FirstOrDefault();

                if (databaseDomain == null)
                {
                    _logger.LogInformation("Cannot update domain, entity is missing");
                    throw new ArgumentException("Cannot update domain, entity is missing");
                }

                databaseDomain.DomainName = domain.DomainName;
                databaseDomain.DomainId = domain.DomainId;
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
        /// Delete domain from database based on Id.
        /// Hard delete will remove also all subdomains and book domains relations
        /// </summary>
        /// <param name="domain">Domain to delete by Id</param>
        /// <param name="hardDelete"> Flag that indicate to remove domain definetive</param>
        /// <exception cref="ArgumentException"></exception>
        public void Delete(Domain domain, bool hardDelete)
        {
            try
            {
                var fullDatabaseDomain = _repository.Get(i => i.Id == domain.Id, null, "BookDomains,Subdomains").FirstOrDefault();
                if (fullDatabaseDomain == null)
                {
                    _logger.LogInformation("Cannot delete domain, entity is missing");
                    throw new ArgumentException("Cannot delete domain, entity is missing");
                }

                if (!hardDelete && (fullDatabaseDomain.Subdomains.Any() || fullDatabaseDomain.BookDomains.Any()))
                {
                    _logger.LogInformation("Cannot delete domain, entity has relations");
                    throw new ArgumentException("Cannot delete domain, entity has relations");
                }

                if (fullDatabaseDomain.Subdomains.Any())
                {
                    foreach (var subdomain in fullDatabaseDomain.Subdomains)
                    {
                        Delete(subdomain, hardDelete);
                    }
                }

                if (fullDatabaseDomain.BookDomains.Any())
                {
                    foreach (var bookDomain in fullDatabaseDomain.BookDomains)
                    {
                        _bookDomainService.Delete(bookDomain);
                    }
                }

                base.Delete(domain);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Log on error update");
                throw;
            }
        }

    }
}
