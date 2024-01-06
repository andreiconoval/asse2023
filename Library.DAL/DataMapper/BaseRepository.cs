//------------------------------------------------------------------------------
// <copyright file="BaseRepository.cs" company="Transilvania University of Brasov">
// Copyright (c) Conoval. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
//------------------------------------------------------------------------------

namespace Library.DAL.DataMapper
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Expressions;
    using Library.DAL.Interfaces;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Defines the <see cref="BaseRepository{T}" />.
    /// </summary>
    /// <typeparam name="T">Entity type.</typeparam>
    [ExcludeFromCodeCoverage]
    public abstract class BaseRepository<T> : IRepository<T>
        where T : class
    {
        /// <summary>
        /// The Get.
        /// </summary>
        /// <param name="filter">The filter<see cref="Expression{Func{T, bool}}?"/>.</param>
        /// <param name="orderBy">The orderBy<see cref="Func{IQueryable{T}, IOrderedQueryable{T}}?"/>.</param>
        /// <param name="includeProperties">The includeProperties<see cref="string"/>.</param>
        /// <returns>The <see cref="IEnumerable{T}"/>.</returns>
        public virtual IEnumerable<T> Get(
            Expression<Func<T, bool>>? filter,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy,
            string includeProperties = "")
        {
            using (var ctx = new LibraryDbContext())
            {
                var databaseSet = ctx.Set<T>();

                IQueryable<T> query = databaseSet;

                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                if (orderBy != null)
                {
                    return orderBy(query).ToList();
                }
                else
                {
                    return query.ToList();
                }
            }
        }

        /// <summary>
        /// The Insert.
        /// </summary>
        /// <param name="entity">The entity<see cref="T"/>.</param>
        public virtual void Insert(T entity)
        {
            using (var ctx = new LibraryDbContext())
            {
                var databaseSet = ctx.Set<T>();
                databaseSet.Add(entity);

                ctx.SaveChanges();
            }
        }

        /// <summary>
        /// The Update.
        /// </summary>
        /// <param name="item">The item<see cref="T"/>.</param>
        public virtual void Update(T item)
        {
            using (var ctx = new LibraryDbContext())
            {
                var databaseSet = ctx.Set<T>();
                databaseSet.Attach(item);
                ctx.Entry(item).State = EntityState.Modified;

                ctx.SaveChanges();
            }
        }

        /// <summary>
        /// The Delete.
        /// </summary>
        /// <param name="id">The id<see cref="object"/>.</param>
        public virtual void Delete(object id)
        {
            this.Delete(this.GetByID(id));
        }

        /// <summary>
        /// The Delete.
        /// </summary>
        /// <param name="entityToDelete">The entityToDelete<see cref="T"/>.</param>
        public virtual void Delete(T entityToDelete)
        {
            using (var ctx = new LibraryDbContext())
            {
                var databaseSet = ctx.Set<T>();

                if (ctx.Entry(entityToDelete).State == EntityState.Detached)
                {
                    databaseSet.Attach(entityToDelete);
                }

                databaseSet.Remove(entityToDelete);

                ctx.SaveChanges();
            }
        }

        /// <summary>
        /// The GetByID.
        /// </summary>
        /// <param name="id">The id<see cref="object"/>.</param>
        /// <returns>The <see cref="T"/>.</returns>
        public virtual T GetByID(object id)
        {
            using (var ctx = new LibraryDbContext())
            {
                return ctx.Set<T>().Find(id);
            }
        }
    }
}
