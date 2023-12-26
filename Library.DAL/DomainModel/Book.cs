using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Library.DAL.DomainModel;

public class Book
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string Title { get; set; }

    public int YearPublication { get; set; }

    public virtual ICollection<BookDomain> BookDomains { get; set; }

    public virtual ICollection<BookAuthor> BookAuthors { get; set; }

    public virtual ICollection<BookEdition> BookEditions { get; set; }
}
