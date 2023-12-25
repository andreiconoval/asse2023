using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.DAL.DomainModel
{
    public class BookSample
    {
        [Key]
        public int Id { get; set; }

        public int BookEditionId { get; set; }

        public bool AvailableForLoan { get; set; }

        public bool AvailableForHall { get; set; }

        [ForeignKey("BookEditionId")]
        public virtual BookEdition BookEdition { get; set; }
    }
}
