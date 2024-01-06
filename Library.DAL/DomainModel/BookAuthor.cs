//------------------------------------------------------------------------------
// <copyright file="BookAuthor.cs" company="Transilvania University of Brasov">
// Copyright (c) Conoval. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
//------------------------------------------------------------------------------

namespace Library.DAL.DomainModel
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Defines the <see cref="BookAuthor" />.
    /// </summary>
    public class BookAuthor
    {
        /// <summary>
        /// Gets or sets the BookId.
        /// </summary>
        [Key, Column(Order = 0)]
        public int BookId { get; set; }

        /// <summary>
        /// Gets or sets the AuthorId.
        /// </summary>
        [Key, Column(Order = 1)]
        public int AuthorId { get; set; }

        /// <summary>
        /// Gets or sets the Book TODO delete.
        /// </summary>
        [ForeignKey("BookId")]
        public virtual Book? Book { get; set; }

        /// <summary>
        /// Gets or sets the Author.
        /// </summary>
        [ForeignKey("AuthorId")]
        public virtual Author? Author { get; set; }
    }
}
