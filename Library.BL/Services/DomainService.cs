//------------------------------------------------------------------------------
// <copyright file="DomainService.cs" company="Transilvania University of Brasov">
// Copyright (c) Conoval. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
//------------------------------------------------------------------------------

namespace Library.BL.Services
{
    using System;
    using System.Linq;
    using FluentValidation.Results;
    using Library.BL.Interfaces;
    using Library.BL.Validators;
    using Library.DAL.DomainModel;
    using Library.DAL.Interfaces;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Domain Service.
    /// </summary>
    public class DomainService : BaseService<Domain, IDomainRepository>, IDomainService
    {
        /// <summary>
        /// book domain service.
        /// </summary>
        private readonly IBookDomainService bookDomainService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainService"/> class.
        /// </summary>
        /// <param name="repository">Domain repository.</param>
        /// <param name="logger">Logger object.</param>
        /// <param name="bookDomainService">Book domain service.</param>
        public DomainService(IDomainRepository repository, ILogger logger, IBookDomainService bookDomainService) : base(repository, new DomainValidator(), logger)
        {
            this.bookDomainService = bookDomainService;
        }

        /// <summary>
        /// Add new domain.
        /// </summary>
        /// <param name="entity">Domain entity.</param>
        /// <returns>Validation Result.</returns>
        public override ValidationResult Insert(Domain entity)
        {
            try
            {
                var result = Validator.Validate(entity);

                if (!result.IsValid)
                {
                    Logger.LogInformation($"Cannot add new domain, entity is invalid");
                    throw new ArgumentException("Cannot add new domain, entity is invalid");
                }

                var domainExists = Repository.Get(i => i.DomainName != null && entity.DomainName != null && i.DomainName.Trim() == entity.DomainName.Trim()).Any();

                if (domainExists)
                {
                    Logger.LogInformation($"Cannot add new domain, entity already exists");
                    throw new ArgumentException("Cannot add new domain, entity already exists");
                }

                Repository.Insert(entity);
                Logger.LogInformation($"Add new domain");
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "Log on error add");
                throw;
            }
        }

        /// <summary>
        /// Update domain name based on id.
        /// </summary>
        /// <param name="domain">Domain entity.</param>
        /// <returns>Validation Result.</returns>
        public override ValidationResult Update(Domain domain)
        {
            try
            {
                var result = Validator.Validate(domain);

                if (!result.IsValid)
                {
                    Logger.LogInformation("Cannot update domain, invalid entity");
                    throw new ArgumentException("Cannot update domain, invalid entity");
                }

                var databaseDomain = Repository.Get(i => i.Id == domain.Id).FirstOrDefault();

                if (databaseDomain == null)
                {
                    Logger.LogInformation("Cannot update domain, entity is missing");
                    throw new ArgumentException("Cannot update domain, entity is missing");
                }

                databaseDomain.DomainName = domain.DomainName;
                databaseDomain.DomainId = domain.DomainId;
                Repository.Update(databaseDomain);
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "Log on error update");
                throw;
            }
        }

        /// <summary>
        /// Delete domain from database based on Id.
        /// Hard delete will remove also all subdomains and book domains relations.
        /// </summary>
        /// <param name="domain">Domain to delete by Id.</param>
        /// <param name="hardDelete"> Flag that indicate to remove domain definitive.</param>
        public void Delete(Domain domain, bool hardDelete)
        {
            try
            {
                if (domain == null)
                {
                    Logger.LogInformation("Cannot delete domain, domain is missing");
                    throw new ArgumentException("Cannot delete domain, domain is missing");
                }

                var fullDatabaseDomain = Repository.Get(i => i.Id == domain.Id, includeProperties: "BookDomains,Subdomains").FirstOrDefault();
                if (fullDatabaseDomain == null)
                {
                    Logger.LogInformation("Cannot delete domain, entity is missing");
                    throw new ArgumentException("Cannot delete domain, entity is missing");
                }

                if (!hardDelete && ((fullDatabaseDomain?.Subdomains?.Any() ?? false) || (fullDatabaseDomain?.BookDomains?.Any() ?? false)))
                {
                    Logger.LogInformation("Cannot delete domain, entity has relations");
                    throw new ArgumentException("Cannot delete domain, entity has relations");
                }

                if (fullDatabaseDomain?.Subdomains?.Any() ?? false)
                {
                    foreach (var subdomain in fullDatabaseDomain.Subdomains)
                    {
                        this.Delete(subdomain, hardDelete);
                    }
                }

                if (fullDatabaseDomain?.BookDomains?.Any() ?? false)
                {
                    foreach (var bookDomain in fullDatabaseDomain.BookDomains)
                    {
                        this.bookDomainService.Delete(bookDomain);
                    }
                }

                base.Delete(domain);
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "Log on error update");
                throw;
            }
        }
    }
}
