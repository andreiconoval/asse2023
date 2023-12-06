using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.DAL.DomainModel
{

    public class Domain
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string DomainName { get; set; }

        public int? DomainId { get; set; }

        [ForeignKey("DomainId")]
        public virtual Domain ParentDomain { get; set; }

        public virtual ICollection<Domain> Subdomains { get; set; }
        public virtual ICollection<BookDomain> BookDomains { get; set; }
    }
}
