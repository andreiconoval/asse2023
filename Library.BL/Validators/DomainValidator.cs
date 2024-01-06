//------------------------------------------------------------------------------
// <copyright file="DomainValidator.cs" company="Transilvania University of Brasov">
// Copyright (c) Conoval. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
//------------------------------------------------------------------------------

namespace Library.BL.Validators
{
    using FluentValidation;
    using Library.DAL.DomainModel;

    /// <summary>
    /// Defines the <see cref="DomainValidator" />.
    /// </summary>
    internal class DomainValidator : AbstractValidator<Domain>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainValidator"/> class.
        /// </summary>
        public DomainValidator()
        {
            RuleFor(book => book.DomainName).NotEmpty().MaximumLength(255).MinimumLength(3);
            RuleFor(book => book.DomainId).NotEmpty().NotEqual(r => r.Id);
        }
    }
}
