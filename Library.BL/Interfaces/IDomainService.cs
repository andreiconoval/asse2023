//------------------------------------------------------------------------------
// <copyright file="IDomainService.cs" company="Transilvania University of Brasov">
// Copyright (c) Conoval. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
//------------------------------------------------------------------------------

namespace Library.BL.Interfaces
{
    using Library.DAL.DomainModel;

    /// <summary>
    /// Represents the service interface for managing domains.
    /// </summary>
    public interface IDomainService : IService<Domain>
    {
        /// <summary>
        ///  Deletes an book based on the specified ID.
        /// </summary>
        /// <param name="domain">Domain entity</param>
        /// <param name="hardDelete">Flag to delete all relations</param>
        void Delete(Domain domain, bool hardDelete);
    }
}