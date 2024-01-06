//------------------------------------------------------------------------------
// <copyright file="BookLoanDetail.cs" company="Transilvania University of Brasov">
// Copyright (c) Conoval. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
//------------------------------------------------------------------------------

namespace Library.DAL.DomainModel
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Defines the <see cref="BookLoanDetail" />.
    /// </summary>
    public class BookLoanDetail
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the BookSampleId.
        /// </summary>
        public int BookSampleId { get; set; }

        /// <summary>
        /// Gets or sets the BookEditionId.
        /// </summary>
        public int? BookEditionId { get; set; }

        /// <summary>
        /// Gets or sets the BookId.
        /// </summary>
        public int? BookId { get; set; }

        /// <summary>
        /// Gets or sets the ReaderLoanId.
        /// </summary>
        public int ReaderLoanId { get; set; }

        /// <summary>
        /// Gets or sets the LoanDate.
        /// </summary>
        public DateTime LoanDate { get; set; }

        /// <summary>
        /// Gets or sets the ExpectedReturnDate.
        /// </summary>
        public DateTime ExpectedReturnDate { get; set; }

        /// <summary>
        /// Gets or sets the EffectiveReturnDate.
        /// </summary>
        public DateTime? EffectiveReturnDate { get; set; }

        /// <summary>
        /// Gets or sets the BookSample.
        /// </summary>
        [ForeignKey("BookSampleId")]
        public virtual BookSample? BookSample { get; set; }

        /// <summary>
        /// Gets or sets the ReaderLoan.
        /// </summary>
        [ForeignKey("ReaderLoanId")]
        public virtual ReaderLoan? ReaderLoan { get; set; }
    }
}
