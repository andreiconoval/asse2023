//------------------------------------------------------------------------------
// <copyright file="ReaderValidator.cs" company="Transilvania University of Brasov">
// Copyright (c) Conoval. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
//------------------------------------------------------------------------------

namespace Library.BL.Validators
{
    using FluentValidation;
    using Library.DAL.DomainModel;

    /// <summary>
    /// Defines the <see cref="ReaderValidator" />.
    /// </summary>
    internal class ReaderValidator : AbstractValidator<Reader>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReaderValidator"/> class.
        /// </summary>
        public ReaderValidator()
        {
            RuleFor(reader => reader.UserId).NotEmpty();
        }
    }
}
