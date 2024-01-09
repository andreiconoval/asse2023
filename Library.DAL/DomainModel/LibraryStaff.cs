//------------------------------------------------------------------------------
// <copyright file="LibraryStaff.cs" company="Transilvania University of Brasov">
// Copyright (c) Conoval. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
//------------------------------------------------------------------------------

namespace Library.DAL.DomainModel
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Defines the <see cref="LibraryStaff" />.
    /// </summary>
    public class LibraryStaff
    {
        /// <summary>
        /// Gets or sets the UserId.
        /// </summary>
        [Key]
        [ForeignKey("User")]
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the User.
        /// </summary>
        public virtual User? User { get; set; }
    }
}
