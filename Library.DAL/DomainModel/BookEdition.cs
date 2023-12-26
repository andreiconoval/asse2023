using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Library.DAL.DomainModel
{
    public class BookEdition
    {
        [Key]
        public int Id { get; set; }

        public int BookId { get; set; }

        public int PageNumber { get; set; }

        [MaxLength(255)]
        public string BookType { get; set; }

        [MaxLength(255)]
        public string Edition { get; set; }

        public int ReleaseYear { get; set; }

        [ForeignKey("BookId")]
        public virtual Book Book { get; set; }

        public virtual ICollection<BookSample> BookSamples { get; set; }
    }

}
