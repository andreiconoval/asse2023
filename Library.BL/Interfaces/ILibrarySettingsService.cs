//------------------------------------------------------------------------------
// <copyright file="ILibrarySettingsService.cs" company="Transilvania University of Brasov">
// Copyright (c) Conoval. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
//------------------------------------------------------------------------------

namespace Library.BL.Interfaces
{
    using System.Collections.Generic;
    using Library.DAL.DomainModel;

    /// <summary>
    /// Represents the service interface for managing library settings.
    /// </summary>
    public interface ILibrarySettingsService
    {
        /// <summary>
        /// Gets the value of C.
        /// </summary>
        int C { get; }

        /// <summary>
        /// Gets the value of D.
        /// </summary>
        int D { get; }

        /// <summary>
        /// Gets the value of DELTA.
        /// </summary>
        int DELTA { get; }

        /// <summary>
        /// Gets the value of DOMENII.
        /// </summary>
        int DOMENII { get; }

        /// <summary>
        /// Gets the value of L.
        /// </summary>
        int L { get; }

        /// <summary>
        /// Gets or sets the library settings.
        /// </summary>
        LibrarySettings LibrarySettings { get; set; }

        /// <summary>
        /// Gets the value of LIM.
        /// </summary>
        int LIM { get; }

        /// <summary>
        /// Gets the value of NCZ.
        /// </summary>
        int NCZ { get; }

        /// <summary>
        /// Gets the value of NMC.
        /// </summary>
        int NMC { get; }

        /// <summary>
        /// Gets the value of PER.
        /// </summary>
        int PER { get; }

        /// <summary>
        /// Gets the value of PERSIMP.
        /// </summary>
        int PERSIMP { get; }

        /// <summary>
        /// Gets or sets the USER_IND.
        /// </summary>
        int USER_IND { get; set; }

        /// <summary>
        /// Checks if a user can borrow books based on specified parameters.
        /// </summary>
        /// <param name="user">The user to check.</param>
        /// <param name="newLoan">The new loan to be considered.</param>
        /// <param name="previousLoans">A list of previous loans for the user.</param>
        /// <param name="staffLendCount">The staff lend count.</param>
        void CheckIfUserCanBorrowBooks(User user, ReaderLoan newLoan, List<ReaderLoan> previousLoans, int staffLendCount);

        /// <summary>
        /// Checks if a user can extend time for loan.
        /// </summary>
        /// <param name="user">The user to check.</param>
        /// <param name="totalExtensionsInLastThreeMonths">The total extensions in last three months.</param>
        bool CheckIfUserCanExtendForLoan(User user, int totalExtensionsInLastThreeMonths);
    }
}