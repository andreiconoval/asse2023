//------------------------------------------------------------------------------
// <copyright file="BookEdition.cs" company="Transilvania University of Brasov">
// Copyright (c) Conoval. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
//------------------------------------------------------------------------------

namespace Library.DAL.DomainModel
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Defines the <see cref="BookEdition" />.
    /// </summary>
    public class BookEdition
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the BookId.
        /// </summary>
        public int BookId { get; set; }

        /// <summary>
        /// Gets or sets the PageNumber.
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Gets or sets the BookType.
        /// </summary>
        [MaxLength(255)]
        public string? BookType { get; set; }

        /// <summary>
        /// Gets or sets the Edition.
        /// </summary>
        [MaxLength(255)]
        public string? Edition { get; set; }

        /// <summary>
        /// Gets or sets the ReleaseYear.
        /// </summary>
        public int ReleaseYear { get; set; }

        /// <summary>
        /// Gets or sets the Book.
        /// </summary>
        [ForeignKey("BookId")]
        public virtual Book? Book { get; set; }

        /// <summary>
        /// Gets or sets the BookSamples.
        /// </summary>
        public virtual ICollection<BookSample>? BookSamples { get; set; }
    }
}
