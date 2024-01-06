//------------------------------------------------------------------------------
// <copyright file="BookValidator.cs" company="Transilvania University of Brasov">
// Copyright (c) Conoval. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
//------------------------------------------------------------------------------

namespace Library.BL.Validators
{
    using FluentValidation;
    using Library.DAL.DomainModel;

    /// <summary>
    /// Defines the <see cref="BookValidator" />.
    /// </summary>
    internal class BookValidator : AbstractValidator<Book>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BookValidator"/> class.
        /// </summary>
        public BookValidator()
        {
            RuleFor(book => book.Title).NotEmpty().MaximumLength(255).MinimumLength(3);
            RuleFor(book => book.YearPublication).NotEmpty().GreaterThan(0);
        }
    }
}
