//------------------------------------------------------------------------------
// <copyright file="ReaderLoan.cs" company="Transilvania University of Brasov">
// Copyright (c) Conoval. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
//------------------------------------------------------------------------------

namespace Library.DAL.DomainModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Defines the <see cref="ReaderLoan" />.
    /// </summary>
    public class ReaderLoan
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the ReaderId.
        /// </summary>
        public int ReaderId { get; set; }

        /// <summary>
        /// Gets or sets the StaffId.
        /// </summary>
        public int StaffId { get; set; }

        /// <summary>
        /// Gets or sets the LoanDate.
        /// </summary>
        public DateTime LoanDate { get; set; }

        /// <summary>
        /// Gets or sets the ExtensionsGranted.
        /// </summary>
        public int ExtensionsGranted { get; set; }

        /// <summary>
        /// Gets or sets the BorrowedBooks.
        /// </summary>
        public int BorrowedBooks { get; set; }

        /// <summary>
        /// Gets or sets the Reader.
        /// </summary>
        [ForeignKey("ReaderId")]
        public virtual Reader? Reader { get; set; }

        /// <summary>
        /// Gets or sets the BookLoanDetails.
        /// </summary>
        public virtual ICollection<BookLoanDetail>? BookLoanDetails { get; set; }
    }
}
