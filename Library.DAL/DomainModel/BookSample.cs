//------------------------------------------------------------------------------
// <copyright file="BookSample.cs" company="Transilvania University of Brasov">
// Copyright (c) Conoval. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
//------------------------------------------------------------------------------

namespace Library.DAL.DomainModel
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Defines the <see cref="BookSample" />.
    /// </summary>
    public class BookSample
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the BookEditionId.
        /// </summary>
        public int BookEditionId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether AvailableForLoan.
        /// </summary>
        public bool AvailableForLoan { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether AvailableForHall.
        /// </summary>
        public bool AvailableForHall { get; set; }

        /// <summary>
        /// Gets or sets the BookEdition.
        /// </summary>
        [ForeignKey("BookEditionId")]
        public virtual BookEdition? BookEdition { get; set; }
    }
}
