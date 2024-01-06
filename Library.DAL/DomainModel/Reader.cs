//------------------------------------------------------------------------------
// <copyright file="Reader.cs" company="Transilvania University of Brasov">
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
    /// Defines the <see cref="Reader" />.
    /// </summary>
    public class Reader
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

        /// <summary>
        /// Gets or sets the ReaderLoans.
        /// </summary>
        public virtual ICollection<ReaderLoan>? ReaderLoans { get; set; }
    }
}
