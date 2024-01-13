//------------------------------------------------------------------------------
// <copyright file="ReaderLoanValidator.cs" company="Transilvania University of Brasov">
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
    /// ReaderLoan Validator.
    /// </summary>
    internal class ReaderLoanValidator : AbstractValidator<ReaderLoan>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReaderLoanValidator"/> class.
        /// </summary>
        public ReaderLoanValidator()
        {
            RuleFor(readerLoan => readerLoan.ReaderId).NotEmpty();
            RuleFor(readerLoan => readerLoan.StaffId).NotEmpty();
            RuleFor(readerLoan => readerLoan.LoanDate).NotEmpty().GreaterThanOrEqualTo(new DateTime(2000, 1, 15));
            RuleFor(readerLoan => readerLoan.BorrowedBooks).NotNull();
            RuleFor(readerLoan => readerLoan.BookLoanDetails).Must((readerLoan, readerLoanDetails) => (readerLoan.BorrowedBooks == 0 && readerLoanDetails == null) || (readerLoanDetails != null && readerLoanDetails.Count == readerLoan.BorrowedBooks));
        }
    }
}
