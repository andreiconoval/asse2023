//------------------------------------------------------------------------------
// <copyright file="Domain.cs" company="Transilvania University of Brasov">
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
    /// Defines the <see cref="Domain" />.
    /// </summary>
    public class Domain
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the DomainName.
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string? DomainName { get; set; }

        /// <summary>
        /// Gets or sets the DomainId.
        /// </summary>
        public int? DomainId { get; set; }

        /// <summary>
        /// Gets or sets the ParentDomain.
        /// </summary>
        [ForeignKey("DomainId")]
        public virtual Domain? ParentDomain { get; set; }

        /// <summary>
        /// Gets or sets the Subdomains.
        /// </summary>
        public virtual ICollection<Domain>? Subdomains { get; set; }

        /// <summary>
        /// Gets or sets the BookDomains.
        /// </summary>
        public virtual ICollection<BookDomain>? BookDomains { get; set; }
    }
}
