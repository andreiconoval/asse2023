//------------------------------------------------------------------------------
// <copyright file="AuthorRepository.cs" company="Transilvania University of Brasov">
// Copyright (c) Conoval. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
//------------------------------------------------------------------------------

namespace Library.DAL.Repositories
{
    using Library.DAL.DataMapper;
    using Library.DAL.DomainModel;
    using Library.DAL.Interfaces;

    /// <summary>
    /// Defines the <see cref="AuthorRepository" />.
    /// </summary>
    public class AuthorRepository : BaseRepository<Author>, IAuthorRepository
    {
    }
}
