//------------------------------------------------------------------------------
// <copyright file="BookAuthorValidator.cs" company="Transilvania University of Brasov">
// Copyright (c) Conoval. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
//------------------------------------------------------------------------------

namespace Library.BL.Validators
{
    using FluentValidation;
    using Library.DAL.DomainModel;

    /// <summary>
    /// Defines the <see cref="BookAuthorValidator" />.
    /// </summary>
    internal class BookAuthorValidator : AbstractValidator<BookAuthor>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BookAuthorValidator"/> class.
        /// </summary>
        public BookAuthorValidator()
        {
            RuleFor(author => author.AuthorId).GreaterThan(0);
            RuleFor(author => author.BookId).GreaterThan(0);
        }
    }
}
