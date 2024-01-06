//------------------------------------------------------------------------------
// <copyright file="LibrarySettings.cs" company="Transilvania University of Brasov">
// Copyright (c) Conoval. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
//------------------------------------------------------------------------------

namespace Library.DAL.DomainModel
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="LibrarySettings" />.
    /// </summary>
    public class LibrarySettings
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the MaxDomains
        /// DOMENII
        /// Number of maximum domains allowed for book.
        /// </summary>
        [Required]
        public int MaxDomains { get; set; }

        /// <summary>
        /// Gets or sets the MaxBookBorrowed
        /// NMC
        /// Max borrowed books for some period(PER).
        /// </summary>
        [Required]
        public int MaxBookBorrowed { get; set; }

        /// <summary>
        /// Gets or sets the BorrowedBooksPeriod
        /// PER
        /// Max period for borrowed books.
        /// </summary>
        [Required]
        public int BorrowedBooksPeriod { get; set; }

        /// <summary>
        /// Gets or sets the MaxBooksBorrowedPerTime
        /// C
        /// Max books allowed to be borrowed per time at once.
        /// </summary>
        [Required]
        public int MaxBooksBorrowedPerTime { get; set; }

        /// <summary>
        /// Gets or sets the MaxAllowedBooksPerDomain
        /// D
        /// Max Allowed books per domains for last L months.
        /// </summary>
        [Required]
        public int MaxAllowedBooksPerDomain { get; set; }

        /// <summary>
        /// Gets or sets the AllowedMonthsForSameDomain
        /// L
        /// Limit of months for same domain.
        /// </summary>
        [Required]
        public int AllowedMonthsForSameDomain { get; set; }

        /// <summary>
        /// Gets or sets the BorrowedBooksExtensionLimit
        /// LIM
        /// Max allowed extensions for borrowing books for last 3 month.
        /// </summary>
        [Required]
        public int BorrowedBooksExtensionLimit { get; set; }

        /// <summary>
        /// Gets or sets the SameBookRepeatBorrowingLimit
        /// DELTA
        /// Delta interval days for allowing to borrow same book.
        /// </summary>
        [Required]
        public int SameBookRepeatBorrowingLimit { get; set; }

        /// <summary>
        /// Gets or sets the MaxBorrowedBooksPerDay
        /// NCZ
        /// Max allowed borrowed books per day.
        /// </summary>
        [Required]
        public int MaxBorrowedBooksPerDay { get; set; }

        /// <summary>
        /// Gets or sets the LimitBookLend
        /// PERSIMP
        /// Max allowed book for lend by stuff per day.
        /// </summary>
        [Required]
        public int LimitBookLend { get; set; }
    }
}
