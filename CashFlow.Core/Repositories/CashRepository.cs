using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CashFlow.Core.Models;
using JetBrains.Annotations;
using NHibernate;
using NHibernate.Linq;

namespace CashFlow.Core.Repositories
{
    public interface ICashRepository : IRepository<Cash>
    { }

    public class CashRepository : AbstractRepository<Cash>, ICashRepository
    {
        public CashRepository([NotNull] ISession session)
            : base(session)
        { }

        public override Cash SingleBy(Expression<Func<Cash, bool>> query)
        {
            return Session.Query<Cash>().FirstOrDefault(query);
        }

        public override IList<Cash> Select()
        {
            return Session.Query<Cash>().ToList();
        }

        public override IList<Cash> SelectBy(Expression<Func<Cash, bool>> query)
        {
            return Session.Query<Cash>().Where(query).ToList();
        }
    }
}
