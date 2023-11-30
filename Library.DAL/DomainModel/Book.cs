using System.ComponentModel.DataAnnotations;

namespace Library.DAL.DomainModel;

public class Book
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string Title { get; set; }

    public int YearPublication { get; set; }

    public virtual ICollection<BookDomain> BookDomains { get; set; }

    public virtual ICollection<BookAuthor> BookAuthors { get; set; }

    public virtual ICollection<BookEdition> BookEditions { get; set; }
}
