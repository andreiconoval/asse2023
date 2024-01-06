//------------------------------------------------------------------------------
// <copyright file="Author.cs" company="Transilvania University of Brasov">
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
    /// Defines the <see cref="Author" />.
    /// </summary>
    public class Author
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the FirstName.
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string? FirstName { get; set; }

        /// <summary>
        /// Gets or sets the LastName.
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string? LastName { get; set; }

        /// <summary>
        /// Gets or sets the BookAuthors.
        /// </summary>
        public virtual ICollection<BookAuthor>? BookAuthors { get; set; }
    }
}
