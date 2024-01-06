//------------------------------------------------------------------------------
// <copyright file="BookDomain.cs" company="Transilvania University of Brasov">
// Copyright (c) Conoval. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
//------------------------------------------------------------------------------

namespace Library.DAL.DomainModel
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Defines the <see cref="BookDomain" />.
    /// </summary>
    public class BookDomain
    {
        /// <summary>
        /// Gets or sets the DomainId.
        /// </summary>
        [Key, Column(Order = 0)]
        public int DomainId { get; set; }

        /// <summary>
        /// Gets or sets the BookId.
        /// </summary>
        [Key, Column(Order = 1)]
        public int BookId { get; set; }

        /// <summary>
        /// Gets or sets the Domain.
        /// </summary>
        [ForeignKey("DomainId")]
        public virtual Domain? Domain { get; set; }

        /// <summary>
        /// Gets or sets the Book.
        /// </summary>
        [ForeignKey("BookId")]
        public virtual Book? Book { get; set; }
    }
}
