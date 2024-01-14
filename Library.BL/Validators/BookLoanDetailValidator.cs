//------------------------------------------------------------------------------
// <copyright file="BookLoanDetailValidator.cs" company="Transilvania University of Brasov">
// Copyright (c) Conoval. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
//------------------------------------------------------------------------------

namespace Library.BL.Validators
{
    using System;
    using FluentValidation;
    using Library.DAL.DomainModel;

    /// <summary>
    /// Defines the <see cref="BookLoanDetailValidator" />.
    /// </summary>
    internal class BookLoanDetailValidator : AbstractValidator<BookLoanDetail>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BookLoanDetailValidator"/> class.
        /// </summary>
        public BookLoanDetailValidator()
        {
            RuleFor(bookLoanDetail => bookLoanDetail.BookSampleId).GreaterThan(0);
            RuleFor(bookLoanDetail => bookLoanDetail.ReaderLoanId).GreaterThan(0);
            RuleFor(bookLoanDetail => bookLoanDetail.LoanDate).NotNull().GreaterThanOrEqualTo(new DateTime(2000, 1, 15));
            RuleFor(bookLoanDetail => bookLoanDetail.ExpectedReturnDate).NotEmpty().GreaterThan(r => r.LoanDate);
            RuleFor(bookLoanDetail => bookLoanDetail.EffectiveReturnDate).GreaterThan(r => r.LoanDate);
        }
    }
}
