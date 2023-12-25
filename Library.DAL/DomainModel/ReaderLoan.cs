using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Library.DAL.DomainModel
{
    public class ReaderLoan
    {
        [Key]
        public int Id { get; set; }

        public int ReaderId { get; set; }

        public int StaffId { get; set; }

        public DateTime LoanDate { get; set; }

        public int ExtensionsGranted { get; set; }

        public int BorrowedBooks { get; set; }

        [ForeignKey("ReaderId")]
        public virtual Reader Reader { get; set; }

        public virtual ICollection<BookLoanDetail> BookLoanDetails { get; set; }
    }
}
