using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace WebFeedReader.Persistence
{
    public struct OrderByDescription<T> where T : class
    {
        public readonly Expression<Func<T, object>> OrderFieldSelector;
        public readonly ListSortDirection OrderDirection;

        public OrderByDescription(
            Expression<Func<T, object>> OrderFieldSelector, ListSortDirection OrderDirection)
        {
            this.OrderFieldSelector = OrderFieldSelector;
            this.OrderDirection = OrderDirection;
        }

        // Convenience method.
        public static OrderByDescription<M> Ord<M>(
            Expression<Func<M, object>> OrderFieldSelector, ListSortDirection OrderDirection) where M : class
        {
            return new OrderByDescription<M>(OrderFieldSelector, OrderDirection);
        }
    }
}