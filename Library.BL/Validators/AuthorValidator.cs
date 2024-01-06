//------------------------------------------------------------------------------
// <copyright file="AuthorValidator.cs" company="Transilvania University of Brasov">
// Copyright (c) Conoval. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
//------------------------------------------------------------------------------

namespace Library.BL.Validators
{
    using FluentValidation;
    using Library.DAL.DomainModel;

    /// <summary>
    /// Defines the <see cref="AuthorValidator" />.
    /// </summary>
    internal class AuthorValidator : AbstractValidator<Author>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorValidator"/> class.
        /// </summary>
        public AuthorValidator()
        {
            RuleFor(author => author.FirstName).NotEmpty();
            RuleFor(author => author.LastName).NotEmpty();
        }
    }
}
