//------------------------------------------------------------------------------
// <copyright file="IReaderLoanService.cs" company="Transilvania University of Brasov">
// Copyright (c) Conoval. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
//------------------------------------------------------------------------------

namespace Library.BL.Interfaces
{
    using Library.DAL.DomainModel;

    /// <summary>
    /// Represents the service interface for managing reader loans.
    /// </summary>
    public interface IReaderLoanService
    {
        /// <summary>
        /// Inserts a new reader loan.
        /// </summary>
        /// <param name="loan">The reader loan to insert.</param>
        /// <returns>The inserted reader loan.</returns>
        ReaderLoan Insert(ReaderLoan loan);

        /// <summary>
        /// Extend Book loan details period from Now.
        /// </summary>
        /// <param name="bookLoanDetailId"> Book loan detail id.</param>
        void SetExtensionsForLoan(int bookLoanDetailId);

        /// <summary>
        /// Mark reader loan as returned for all book.
        /// </summary>
        /// <param name="readerLoanId"> Reader loan id.</param>
        void ReturnBooks(int readerLoanId);

        /// <summary>
        /// Mark reader loan as returned for book.
        /// </summary>
        /// <param name="readerLoanId"> Reader loan id.</param>
        /// <param name="bookId"> Book id.</param>
        void ReturnBook(int readerLoanId, int bookId);
    }
}
