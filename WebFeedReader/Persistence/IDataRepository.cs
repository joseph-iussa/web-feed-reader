using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;

namespace WebFeedReader.Persistence
{
    public interface IDataRepository<T> where T : class
    {
        IList<T> GetList(Expression<Func<T, bool>> filterBy = null,
                         OrderByDescription<T>[] orderBy = null,
                         Expression<Func<T, object>>[] navigationProperties = null);

        T GetSingle(Expression<Func<T, bool>> filterBy,
                    params Expression<Func<T, object>>[] navigationProperties);

        void Add(params T[] entities);

        void Update(params T[] entities);

        void Remove(params T[] entities);
    }
}