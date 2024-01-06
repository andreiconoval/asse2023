//------------------------------------------------------------------------------
// <copyright file="DomainHelper.cs" company="Transilvania University of Brasov">
// Copyright (c) Conoval. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
//------------------------------------------------------------------------------

namespace LibraryBLTests.Helpers
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Library.DAL.DomainModel;

    /// <summary>
    /// Defines the <see cref="DomainHelper" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class DomainHelper
    {
        /// <summary>
        /// The GenerateDomains.
        /// </summary>
        /// <returns>The <see cref="List{Domain}"/>.</returns>
        public static List<Domain> GenerateDomains()
        {
            var domains = new List<Domain>();

            var baseDomain = new Domain { Id = 0, DomainName = "BaseDomains NOT ASSIGN" };
            var stiinta = new Domain { Id = 1, DomainName = "Stiinta", DomainId = 0, ParentDomain = baseDomain };
            var informatica = new Domain { Id = 2, DomainName = "Informatica", DomainId = 1, ParentDomain = stiinta };
            var algoritmi = new Domain { Id = 3, DomainName = "Algoritmi", DomainId = 2, ParentDomain = informatica };
            var matematica = new Domain { Id = 4, DomainName = "Matematica", DomainId = 1, ParentDomain = stiinta };
            var fizica = new Domain { Id = 5, DomainName = "Fizica", DomainId = 1, ParentDomain = stiinta };
            var chimie = new Domain { Id = 6, DomainName = "Chimie", DomainId = 1, ParentDomain = stiinta };
            var programare = new Domain { Id = 7, DomainName = "Programare", DomainId = 2, ParentDomain = informatica };
            var bazeDeDate = new Domain { Id = 8, DomainName = "Baze de date", DomainId = 2, ParentDomain = informatica };
            var reteleDeCalculatoare = new Domain { Id = 9, DomainName = "Retele de calculatoare", DomainId = 2, ParentDomain = informatica };
            var algoritmicaGrafurilor = new Domain { Id = 10, DomainName = "Algoritmica grafurilor", DomainId = 3, ParentDomain = algoritmi };
            var algoritmiCuantici = new Domain { Id = 11, DomainName = "Algoritmi cuantici", DomainId = 3, ParentDomain = algoritmi };

            stiinta.Subdomains.Add(matematica);
            stiinta.Subdomains.Add(fizica);
            stiinta.Subdomains.Add(chimie);
            stiinta.Subdomains.Add(informatica);

            informatica.Subdomains.Add(algoritmi);
            informatica.Subdomains.Add(programare);
            informatica.Subdomains.Add(bazeDeDate);
            informatica.Subdomains.Add(reteleDeCalculatoare);

            algoritmi.Subdomains.Add(algoritmicaGrafurilor);
            algoritmi.Subdomains.Add(algoritmiCuantici);

            domains.Add(baseDomain);
            domains.Add(stiinta);
            domains.Add(informatica);
            domains.Add(algoritmi);
            domains.Add(matematica);
            domains.Add(fizica);
            domains.Add(chimie);
            domains.Add(programare);
            domains.Add(bazeDeDate);
            domains.Add(reteleDeCalculatoare);
            domains.Add(algoritmicaGrafurilor);
            domains.Add(algoritmiCuantici);

            return domains;
        }

        /// <summary>
        /// The IsDescendantOrAncestor.
        /// </summary>
        /// <param name="domain1">The domain1<see cref="Domain"/>.</param>
        /// <param name="domain2">The domain2<see cref="Domain"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool IsDescendantOrAncestor(Domain domain1, Domain domain2)
        {
            return domain1.Id == domain2.Id || domain1.Subdomains.Any(d => IsDescendantOrAncestor(d, domain2)) ||
                   domain2.Subdomains.Any(d => IsDescendantOrAncestor(domain1, d));
        }
    }
}
