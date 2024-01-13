//------------------------------------------------------------------------------
// <copyright file="BookDomainService.cs" company="Transilvania University of Brasov">
// Copyright (c) Conoval. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
//------------------------------------------------------------------------------

namespace Library.BL.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentValidation.Results;
    using Library.BL.Interfaces;
    using Library.BL.Validators;
    using Library.DAL.DomainModel;
    using Library.DAL.Interfaces;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Defines the <see cref="BookDomainService" />.
    /// </summary>
    public class BookDomainService : BaseService<BookDomain, IBookDomainRepository>, IBookDomainService
    {
        /// <summary>
        /// Defines the _domainRepository.
        /// </summary>
        private readonly IDomainRepository domainRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookDomainService"/> class.
        /// </summary>
        /// <param name="repository">The repository<see cref="IBookDomainRepository"/>.</param>
        /// <param name="domainRepository">The domainRepository<see cref="IDomainRepository"/>.</param>
        /// <param name="logger">The logger<see cref="ILogger"/>.</param>
        public BookDomainService(
            IBookDomainRepository repository,
            IDomainRepository domainRepository,
            ILogger logger)
            : base(repository, new BookDomainValidator(), logger)
        {
            this.domainRepository = domainRepository;
        }

        /// <summary>
        /// Insert new book domain.
        /// </summary>
        /// <param name="bookDomain">Book domain relation.</param>
        /// <returns>Validation result.</returns>
        public override ValidationResult Insert(BookDomain bookDomain)
        {
            try
            {
                var result = Validator.Validate(bookDomain);

                if (!result.IsValid)
                {
                    Logger.LogInformation("Cannot add book domain, invalid entity");
                    throw new ArgumentException("Cannot add book domain, invalid entity");
                }

                var bookDomains = Repository.Get(i => i.BookId == bookDomain.BookId).ToList();
                var bookDomainExists = bookDomains.Any(i => i.DomainId == bookDomain.DomainId);

                if (bookDomainExists)
                {
                    Logger.LogInformation("Cannot add book domain, book domain already exists");
                    throw new ArgumentException("Cannot add book domain, book domain already exists");
                }

                var domains = this.domainRepository.Get().ToList();

                if (this.IsDomainBookRelationValid(bookDomain, bookDomains, domains))
                {
                    Logger.LogInformation("Cannot add book domain, the ancestor-descendant relationship is not valid!");
                    throw new ArgumentException("Cannot add book domain, the ancestor-descendant relationship is not valid!");
                }

                Repository.Insert(bookDomain);

                return result;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Check if new book domain doesn't have parent or is not aren't for existing domain.
        /// </summary>
        /// <param name="newRelation">New book domain relation.</param>
        /// <param name="bookDomains">Current book domains.</param>
        /// <param name="domains">All domains.</param>
        /// <returns>True if meet parent child relation.</returns>
        private bool IsDomainBookRelationValid(BookDomain newRelation, List<BookDomain> bookDomains, List<Domain> domains)
        {
            var domain = domains.Find(d => d.Id == newRelation.DomainId);
            if (domain == null)
            {
                Logger.LogInformation("Cannot add book domain, the domain is not valid!");
                throw new ArgumentException("Cannot add book domain, the domain is not valid!");
            }

            var newDomainAncestor = this.GetDomainAncestor(domain.Id, domains);

            foreach (var bookDomain in bookDomains)
            {
                if (bookDomain.Domain == null || newDomainAncestor == this.GetDomainAncestor(bookDomain.Domain.Id, domains))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Method to find domains ancestors.
        /// </summary>
        /// <param name="id">Domains id.</param>
        /// <param name="domains">List of domains to search.</param>
        /// <returns>Ancestor domain id.</returns>
        private int? GetDomainAncestor(int id, List<Domain> domains)
        {
            var ancestor = domains.FirstOrDefault(i => i.Id == id);
            if (ancestor != null && ancestor.DomainId != null)
            {
                return this.GetDomainAncestor(ancestor.DomainId.Value, domains);
            }

            return ancestor?.Id;
        }
    }
}
