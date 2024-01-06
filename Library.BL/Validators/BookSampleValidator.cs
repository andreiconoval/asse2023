//------------------------------------------------------------------------------
// <copyright file="BookSampleValidator.cs" company="Transilvania University of Brasov">
// Copyright (c) Conoval. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
//------------------------------------------------------------------------------

namespace Library.BL.Validators
{
    using FluentValidation;
    using Library.DAL.DomainModel;

    /// <summary>
    /// Defines the <see cref="BookSampleValidator" />.
    /// </summary>
    public class BookSampleValidator : AbstractValidator<BookSample>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BookSampleValidator"/> class.
        /// </summary>
        public BookSampleValidator()
        {
            RuleFor(book => book.BookEditionId).NotEmpty();
            RuleFor(book => book)
             .Must(book => book.AvailableForLoan || book.AvailableForHall)
             .WithMessage("At least one of AvailableForLoan or AvailableForHall should be true.");
        }
    }
}
