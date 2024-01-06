//------------------------------------------------------------------------------
// <copyright file="LibraryStaffService.cs" company="Transilvania University of Brasov">
// Copyright (c) Conoval. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
//------------------------------------------------------------------------------

namespace Library.BL.Services
{
    using Library.BL.Interfaces;
    using Library.BL.Validators;
    using Library.DAL.DomainModel;
    using Library.DAL.Interfaces;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Library settings service.
    /// </summary>
    public class LibraryStaffService : BaseService<LibraryStaff, ILibraryStaffRepository>, ILibraryStaffService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryStaffService"/> class.
        /// </summary>
        /// <param name="repository">LibraryStaff repository.</param>
        /// <param name="logger">Logger object.</param>
        public LibraryStaffService(ILibraryStaffRepository repository, ILogger logger) : base(repository, new LibraryStaffValidator(), logger)
        {
        }
    }
}
