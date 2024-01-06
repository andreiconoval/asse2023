﻿//------------------------------------------------------------------------------
// <copyright file="IBookAuthorRepository.cs" company="Transilvania University of Brasov">
// Copyright (c) Conoval. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
//------------------------------------------------------------------------------

namespace Library.DAL.Interfaces
{
    using Library.DAL.DomainModel;

    /// <summary>
    /// Defines the <see cref="IBookAuthorRepository" />.
    /// </summary>
    public interface IBookAuthorRepository : IRepository<BookAuthor>
    {
    }
}
