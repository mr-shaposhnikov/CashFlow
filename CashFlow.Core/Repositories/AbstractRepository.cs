using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using JetBrains.Annotations;
using NHibernate;

namespace CashFlow.Core.Repositories
{
    public abstract class AbstractRepository<T> : IRepository<T>
        where T : class
    {
        protected readonly ISession Session;

        protected AbstractRepository([NotNull] ISession session)
        {
            Session = session ?? throw new ArgumentNullException(nameof(session));
        }

        public void Insert([NotNull] T value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            Session.Save(value);
        }

        public void Update([NotNull] T value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            Session.Update(value);
        }

        public void Delete([NotNull] T value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            Session.Delete(value);
        }

        public abstract T SingleBy(Expression<Func<T, bool>> query);

        public abstract IList<T> Select();

        public abstract IList<T> SelectBy(Expression<Func<T, bool>> query);
    }
}
