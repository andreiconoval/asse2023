//------------------------------------------------------------------------------
// <copyright file="IRepository.cs" company="Transilvania University of Brasov">
// Copyright (c) Conoval. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
//------------------------------------------------------------------------------

namespace Library.DAL.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Defines the <see cref="IRepository{T}" />.
    /// </summary>
    /// <typeparam name="T">Entity type.</typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// The Insert.
        /// </summary>
        /// <param name="entity">The entity<see cref="T"/>.</param>
        void Insert(T entity);

        /// <summary>
        /// The Update.
        /// </summary>
        /// <param name="item">The item<see cref="T"/>.</param>
        void Update(T item);

        /// <summary>
        /// The Delete.
        /// </summary>
        /// <param name="entity">The entity<see cref="T"/>.</param>
        void Delete(T entity);

        /// <summary>
        /// The GetByID.
        /// </summary>
        /// <param name="id">The id<see cref="object"/>.</param>
        /// <returns>The <see cref="T"/>.</returns>
        T GetByID(object id);

        /// <summary>
        /// Search for entity.
        /// </summary>
        /// <param name="filter">Filter function.</param>
        /// <param name="orderBy">Order by function.</param>
        /// <param name="includeProperties">Include entity properties, for multiple split with , .</param>
        /// <returns>List of entities.</returns>
        IEnumerable<T> Get(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string includeProperties = "");
    }
}
