using System.ComponentModel.DataAnnotations;

namespace Library.DAL.DomainModel
{
    public class LibrarySettings
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// DOMENII
        /// Number of maximum domains allowed for book
        /// </summary>
        [Required]
        public int MaxDomains { get; set; }

        /// <summary>
        /// NMC
        /// Max borrowed books for certains period(PER)
        /// </summary>
        [Required]
        public int MaxBookBorrowed { get; set; }

        /// <summary>
        /// PER
        /// Max period for borrowed books
        /// </summary>
        [Required]
        public int BorrowedBooksPeriod { get; set; }

        /// <summary>
        /// C
        /// Max books allowed to be borrowed per time at once
        /// </summary>
        [Required]
        public int MaxBooksBorrowedPerTime { get; set; }

        /// <summary>
        /// D
        /// Max Allowd books per domains for last L months
        /// </summary>
        [Required]
        public int MaxAllowedBooksPerDomain { get; set; }

        /// <summary>
        /// L
        /// Limit of months for same domain
        /// </summary>
        [Required]
        public int AllowedMonthsForSameDomain { get; set; }

        /// <summary>
        /// LIM
        /// Max allowed extensions for borrowing books for last 3 month
        /// </summary>
        [Required]
        public int BorrowedBooksExtensionLimit { get; set; }

        /// <summary>
        /// DELTA
        /// Delta interval days for allowing to borrow same book 
        /// </summary>
        [Required]
        public int SameBookRepeatBorrowingLimt { get; set; }

        /// <summary>
        /// NCZ
        /// Max allowed borrowed books per day
        /// </summary>
        [Required]
        public int MaxBorrowedBooksPerDay { get; set; }

        /// <summary>
        /// PERSIMP
        /// Max allowed book for lend by stuff per day
        /// </summary>
        [Required]
        public int LimitBookLend { get; set; }
    }
}
