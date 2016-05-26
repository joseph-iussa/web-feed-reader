using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

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
                    var orderedQuery = query as IOrderedQueryable<T>;

                    var firstOrderByDescr = orderBy.First();
                    orderedQuery = OrderByMember(
                        orderedQuery, firstOrderByDescr.OrderFieldSelector, firstOrderByDescr.OrderDirection);

                    for (int i = 1; i < orderBy.Length; i++)
                    {
                        var orderByDescr = orderBy[i];
                        orderedQuery = ThenOrderByMember(
                            orderedQuery, orderByDescr.OrderFieldSelector, orderByDescr.OrderDirection);
                    }

                    query = orderedQuery;
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

        // Use reflection to get around EF's problem dealing with Expression<Func<T, object>> when
        // the object is a struct type. See http://stackoverflow.com/questions/1145847/entity-framework-linq-to-entities-only-supports-casting-entity-data-model-primi
        private static IOrderedQueryable<T> OrderByMember(
            IOrderedQueryable<T> query,
            Expression<Func<T, object>> expression,
            ListSortDirection orderDirection)
        {
            return OrderByMemberImpl(query, expression, orderDirection,
                orderDirection == ListSortDirection.Ascending ? "OrderBy" : "OrderByDescending");
        }

        private static IOrderedQueryable<T> ThenOrderByMember(
            IOrderedQueryable<T> query,
            Expression<Func<T, object>> expression,
            ListSortDirection orderDirection)
        {
            return OrderByMemberImpl(query, expression, orderDirection,
                orderDirection == ListSortDirection.Ascending ? "ThenBy" : "ThenByDescending");
        }

        private static IOrderedQueryable<T> OrderByMemberImpl(
            IOrderedQueryable<T> query,
            Expression<Func<T, object>> expression,
            ListSortDirection orderDirection,
            string orderByMethodName)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                memberExpression = (expression.Body as UnaryExpression).Operand as MemberExpression;
            }

            return (IOrderedQueryable<T>)query.Provider.CreateQuery(
                Expression.Call(
                    typeof(Queryable),
                    orderByMethodName,
                    new[] { typeof(T), memberExpression.Type },
                    query.Expression,
                    Expression.Lambda(memberExpression, expression.Parameters)));
        }
    }
}