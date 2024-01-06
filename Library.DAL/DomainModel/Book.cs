//------------------------------------------------------------------------------
// <copyright file="Book.cs" company="Transilvania University of Brasov">
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
    /// Defines the <see cref="Book" />.
    /// </summary>
    public class Book
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Title.
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the YearPublication.
        /// </summary>
        public int YearPublication { get; set; }

        /// <summary>
        /// Gets or sets the BookDomains.
        /// </summary>
        public virtual ICollection<BookDomain>? BookDomains { get; set; }

        /// <summary>
        /// Gets or sets the BookAuthors.
        /// </summary>
        public virtual ICollection<BookAuthor>? BookAuthors { get; set; }

        /// <summary>
        /// Gets or sets the BookEditions.
        /// </summary>
        public virtual ICollection<BookEdition>? BookEditions { get; set; }
    }
}
