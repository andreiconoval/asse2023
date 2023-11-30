using System.ComponentModel.DataAnnotations;

namespace Library.DAL.DomainModel
{
    public class Author
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(255)]
        public string LastName { get; set; }

        public virtual ICollection<BookAuthor> BookAuthors { get; set; }
    }
}
