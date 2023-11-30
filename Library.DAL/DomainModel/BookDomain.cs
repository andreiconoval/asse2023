using System.ComponentModel.DataAnnotations.Schema;

namespace Library.DAL.DomainModel
{
    public class BookDomain
    {
        public int DomainId { get; set; }
        public int BookId { get; set; }

        [ForeignKey("DomainId")]
        public virtual Domain Domain { get; set; }

        [ForeignKey("BookId")]
        public virtual Book Book { get; set; }
    }

}
