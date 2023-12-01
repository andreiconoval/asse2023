using System.Data.Entity;
using System.Linq.Expressions;
using Library.DAL.Interfaces;

namespace Library.DAL.DataMapper
{
    public abstract class BaseRepository<T> : IRepository<T>
        where T : class
    {
        public virtual IEnumerable<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "")
        {
            using (var ctx = new LibraryDbContext())
            {
                var dbSet = ctx.Set<T>();

                IQueryable<T> query = dbSet;

                foreach (var includeProperty in includeProperties.Split
                   (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
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

        public virtual void Insert(T entity)
        {
            using (var ctx = new LibraryDbContext())
            {
                var dbSet = ctx.Set<T>();
                dbSet.Add(entity);

                ctx.SaveChanges();
            }
        }

        public virtual void Update(T item)
        {
            using (var ctx = new LibraryDbContext())
            {
                var dbSet = ctx.Set<T>();
                dbSet.Attach(item);
                ctx.Entry(item).State = EntityState.Modified;

                ctx.SaveChanges();
            }
        }

        public virtual void Delete(object id)
        {
            Delete(GetByID(id));
        }

        public virtual void Delete(T entityToDelete)
        {
            using (var ctx = new LibraryDbContext())
            {
                var dbSet = ctx.Set<T>();

                if (ctx.Entry(entityToDelete).State == EntityState.Detached)
                {
                    dbSet.Attach(entityToDelete);
                }

                dbSet.Remove(entityToDelete);

                ctx.SaveChanges();
            }
        }

        public virtual T GetByID(object id)
        {
            using (var ctx = new LibraryDbContext())
            {
                return ctx.Set<T>().Find(id);
            }
        }
    }

}
