using Library.DAL.DomainModel;
using System.Diagnostics.CodeAnalysis;

namespace LibraryBLTests.Helpers
{
    [ExcludeFromCodeCoverage]
    public static class DomainHelper
    {
        public static List<Domain> GenerateDomains()
        {
            var domains = new List<Domain>();

            var baseDomain = new Domain { Id = 0, DomainName = "BaseDomains NOT ASSIGN" };
            // Define the domain hierarchy
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


            // Build the hierarchy
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


            // Add domains to the list
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

        public static bool IsDescendantOrAncestor(Domain domain1, Domain domain2)
        {
            // Check if domain1 is a descendant or ancestor of domain2
            return domain1.Id == domain2.Id || domain1.Subdomains.Any(d => IsDescendantOrAncestor(d, domain2)) ||
                   domain2.Subdomains.Any(d => IsDescendantOrAncestor(domain1, d));
        }

    }
}
