using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.DAL.DomainModel
{
    public class BookLoanDetail
    {
        [Key]
        public int Id { get; set; }

        public int BookSampleId { get; set; }

        public int? BookEditionId { get; set; }

        public int? BookId { get; set; }

        public int ReaderLoanId { get; set; }

        public DateTime LoanDate { get; set; }

        public DateTime ExpectedReturnDate { get; set; }

        public DateTime? EffectiveReturnDate { get; set; }

        [ForeignKey("BookSampleId")]
        public virtual BookSample BookSample { get; set; }

        [ForeignKey("ReaderLoanId")]
        public virtual ReaderLoan ReaderLoan { get; set; }
    }
}
