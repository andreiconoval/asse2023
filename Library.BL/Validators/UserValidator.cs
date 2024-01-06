//------------------------------------------------------------------------------
// <copyright file="UserValidator.cs" company="Transilvania University of Brasov">
// Copyright (c) Conoval. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
//------------------------------------------------------------------------------

namespace Library.BL.Validators
{
    using FluentValidation;
    using Library.DAL.DomainModel;

    /// <summary>
    /// Defines the <see cref="UserValidator" />.
    /// </summary>
    internal class UserValidator : AbstractValidator<User>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserValidator"/> class.
        /// </summary>
        public UserValidator()
        {
            RuleFor(user => user.FirstName).NotEmpty().MaximumLength(255);
            RuleFor(user => user.LastName).NotEmpty().MaximumLength(255);
            RuleFor(user => user.Address).MaximumLength(255);
            RuleFor(user => user.Phone).MaximumLength(15);
            RuleFor(user => user.Email).NotEmpty().MaximumLength(255);
        }
    }
}
