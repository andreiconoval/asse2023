//------------------------------------------------------------------------------
// <copyright file="LibraryStaffValidator.cs" company="Transilvania University of Brasov">
// Copyright (c) Conoval. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
//------------------------------------------------------------------------------

namespace Library.BL.Validators
{
    using FluentValidation;
    using Library.DAL.DomainModel;

    /// <summary>
    /// Defines the <see cref="LibraryStaffValidator" />.
    /// </summary>
    internal class LibraryStaffValidator : AbstractValidator<LibraryStaff>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryStaffValidator"/> class.
        /// </summary>
        public LibraryStaffValidator()
        {
            RuleFor(staff => staff.UserId).NotEmpty();
        }
    }
}
