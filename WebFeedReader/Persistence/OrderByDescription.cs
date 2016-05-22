using System;
using System.Linq.Expressions;

public struct OrderByDescription<T> where T : class
{
    public readonly Expression<Func<T, object>> OrderFieldSelector;
    public readonly bool OrderAscending;

    public OrderByDescription(Expression<Func<T, object>> OrderFieldSelector, bool OrderAscending)
    {
        this.OrderFieldSelector = OrderFieldSelector;
        this.OrderAscending = OrderAscending;
    }
}