//------------------------------------------------------------------------------
// <copyright file="BookDomainValidator.cs" company="Transilvania University of Brasov">
// Copyright (c) Conoval. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
//------------------------------------------------------------------------------

namespace Library.BL.Validators
{
    using FluentValidation;
    using Library.DAL.DomainModel;

    /// <summary>
    /// Defines the <see cref="BookDomainValidator" />.
    /// </summary>
    internal class BookDomainValidator : AbstractValidator<BookDomain>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BookDomainValidator"/> class.
        /// </summary>
        public BookDomainValidator()
        {
            RuleFor(bookDomain => bookDomain.DomainId).GreaterThan(0);
            RuleFor(bookDomain => bookDomain.BookId).GreaterThan(0);
        }
    }
}
