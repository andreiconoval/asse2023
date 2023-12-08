using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Library.DAL.DomainModel
{
    [ExcludeFromCodeCoverage]
    public class Reader
    {
        [Key]
        [ForeignKey("User")]
        public int UserId { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<ReaderLoan> ReaderLoans { get; set; }
    }

}
