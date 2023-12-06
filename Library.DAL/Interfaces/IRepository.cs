using System.Linq.Expressions;

namespace Library.DAL.Interfaces
{
    public interface IRepository<T>
    {
        void Insert(T entity);

        void Update(T item);

        void Delete(T entity);

        T GetByID(object id);

        /// <summary>
        /// Search for entity
        /// </summary>
        /// <param name="filter">Filter function</param>
        /// <param name="orderBy">Order by function</param>
        /// <param name="includeProperties">Include entity properties, for multiple split with , </param>
        /// <returns></returns>
        IEnumerable<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "");
    }
}
