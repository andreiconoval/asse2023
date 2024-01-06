//------------------------------------------------------------------------------
// <copyright file="ReaderService.cs" company="Transilvania University of Brasov">
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
    /// Defines the <see cref="ReaderService" />.
    /// </summary>
    public class ReaderService : BaseService<Reader, IReaderRepository>, IReaderService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReaderService"/> class.
        /// </summary>
        /// <param name="repository">The repository<see cref="IReaderRepository"/>.</param>
        /// <param name="logger">The logger<see cref="ILogger"/>.</param>
        public ReaderService(IReaderRepository repository, ILogger logger) : base(repository, new ReaderValidator(), logger)
        {
        }
    }
}
