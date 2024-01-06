//------------------------------------------------------------------------------
// <copyright file="BookEditionValidator.cs" company="Transilvania University of Brasov">
// Copyright (c) Conoval. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
//------------------------------------------------------------------------------

namespace Library.BL.Validators
{
    using FluentValidation;
    using Library.DAL.DomainModel;

    /// <summary>
    /// Defines the <see cref="BookEditionValidator" />.
    /// </summary>
    internal class BookEditionValidator : AbstractValidator<BookEdition>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BookEditionValidator"/> class.
        /// </summary>
        public BookEditionValidator()
        {
            RuleFor(bookEdition => bookEdition.BookId).GreaterThan(0);
            RuleFor(bookEdition => bookEdition.PageNumber).GreaterThan(0);
            RuleFor(bookEdition => bookEdition.BookType).NotEmpty().MaximumLength(255);
            RuleFor(bookEdition => bookEdition.Edition).NotEmpty().MaximumLength(255);
            RuleFor(bookEdition => bookEdition.ReleaseYear).GreaterThan(0);
        }
    }
}
