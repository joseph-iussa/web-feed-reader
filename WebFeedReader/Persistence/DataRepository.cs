using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebFeedReader.Persistence
{
    public class DataRepository<T> : IDataRepository<T> where T : class
    {
        public IList<T> GetList(Expression<Func<T, bool>> filterBy = null,
                                OrderByDescription<T>[] orderBy = null,
                                Expression<Func<T, object>>[] navigationProperties = null)
        {
            using (var context = GetDbContext())
            {
                IQueryable<T> query = context.Set<T>();

                if (filterBy != null)
                {
                    query = query.Where(filterBy);
                }

                if (orderBy != null && orderBy.Any())
                {
                    var firstOrderByDesc = orderBy.First();
                    if (firstOrderByDesc.OrderAscending)
                    {
                        query = query.OrderBy(firstOrderByDesc.OrderFieldSelector);
                    }
                    else
                    {
                        query = query.OrderByDescending(firstOrderByDesc.OrderFieldSelector);
                    }

                    for (int i = 1; i < orderBy.Length; i++)
                    {
                        var orderByDesc = orderBy[i];
                        if (orderByDesc.OrderAscending)
                        {
                            query = (query as IOrderedQueryable<T>).ThenBy(orderByDesc.OrderFieldSelector);
                        }
                        else
                        {
                            query = (query as IOrderedQueryable<T>).ThenByDescending(orderByDesc.OrderFieldSelector);
                        }
                    }
                }

                if (navigationProperties != null && navigationProperties.Any())
                {
                    foreach (var navProp in navigationProperties)
                    {
                        query = query.Include(navProp);
                    }
                }

                return query.ToList();
            }
        }

        public T GetSingle(Expression<Func<T, bool>> filterBy,
                           params Expression<Func<T, object>>[] navigationProperties)
        {
            using (var context = GetDbContext())
            {
                IQueryable<T> query = context.Set<T>();

                query = query.Where(filterBy);

                foreach (var navProp in navigationProperties)
                {
                    query = query.Include(navProp);
                }

                try
                {
                    return query.SingleOrDefault();
                }
                catch (InvalidOperationException ex)
                {
                    throw new DataRepositoryException("Filter expression returned more than one entity", ex);
                }
            }
        }

        public void Add(params T[] entities)
        {
            using (var context = GetDbContext())
            {
                DbSet<T> entitySet = context.Set<T>();
                entitySet.AddRange(entities);
                context.SaveChanges();
            }
        }

        public void Update(params T[] entities)
        {
            using (var context = GetDbContext())
            {
                foreach (T entity in entities)
                {
                    context.Entry(entity).State = EntityState.Modified;
                }

                context.SaveChanges();
            }
        }

        public void Remove(params T[] entities)
        {
            using (var context = GetDbContext())
            {
                foreach (T entity in entities)
                {
                    context.Entry(entity).State = EntityState.Deleted;
                }

                context.SaveChanges();
            }
        }

        protected virtual DbContext GetDbContext()
        {
            return new PersistenceContext();
        }
    }
}