using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CashFlow.Core.Repositories
{
    public interface IRepository<T>
        where T : class
    {
        void Insert(T value);
        void Update(T value);
        void Delete(T value);

        T SingleBy(Expression<Func<T, bool>> query);
        IList<T> Select();
        IList<T> SelectBy(Expression<Func<T, bool>> query);
    }
}
