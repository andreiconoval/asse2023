using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Library.DAL.DomainModel
{
    [ExcludeFromCodeCoverage]
    public class ReaderLoan
    {
        [Key]
        public int Id { get; set; }

        public int ReaderId { get; set; }

        public DateTime LoanDate { get; set; }

        public DateTime ExpectedReturnDate { get; set; }

        public DateTime? EffectiveReturnDate { get; set; }

        public int ExtensionsGranted { get; set; }

        public int BorrowedBooks { get; set; }

        [ForeignKey("ReaderId")]
        public virtual Reader Reader { get; set; }

        public virtual ICollection<BookLoanDetail> BookLoanDetails { get; set; }
    }
}
