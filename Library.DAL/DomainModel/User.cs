using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Library.DAL.DomainModel
{
    [ExcludeFromCodeCoverage]
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(255)]
        public string LastName { get; set; }

        [MaxLength(255)]
        public string Address { get; set; }

        [MaxLength(15)]
        public string Phone { get; set; }

        [MaxLength(255)]
        public string Email { get; set; }

        public virtual Reader Reader { get; set; }

        public virtual LibraryStaff LibraryStaff { get; set; }
    }
}
