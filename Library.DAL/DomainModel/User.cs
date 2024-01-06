//------------------------------------------------------------------------------
// <copyright file="User.cs" company="Transilvania University of Brasov">
// Copyright (c) Conoval. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
//------------------------------------------------------------------------------

namespace Library.DAL.DomainModel
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="User" />.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        [Key]
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
        /// Gets or sets the Address.
        /// </summary>
        [MaxLength(255)]
        public string? Address { get; set; }

        /// <summary>
        /// Gets or sets the Phone.
        /// </summary>
        [MaxLength(15)]
        public string? Phone { get; set; }

        /// <summary>
        /// Gets or sets the Email.
        /// </summary>
        [MaxLength(255)]
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the Reader.
        /// </summary>
        public virtual Reader? Reader { get; set; }

        /// <summary>
        /// Gets or sets the LibraryStaff.
        /// </summary>
        public virtual LibraryStaff? LibraryStaff { get; set; }
    }
}
