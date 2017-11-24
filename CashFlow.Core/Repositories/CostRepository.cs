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
    public interface ICostRepository : IRepository<Cost>
    { }

    public class CostRepository : AbstractRepository<Cost>, ICostRepository
    {
        public CostRepository([NotNull] ISession session)
            : base(session)
        { }
        
        public override Cost SingleBy(Expression<Func<Cost, bool>> query)
        {
            return Session.Query<Cost>()
                .Fetch(x => x.Category)
                .FirstOrDefault(query);
        }

        public override IList<Cost> Select()
        {
            return Session.Query<Cost>()
                .Fetch(x => x.Category)
                .ToList();
        }

        public override IList<Cost> SelectBy(Expression<Func<Cost, bool>> query)
        {
            return Session.Query<Cost>()
                .Where(query)
                .Fetch(x => x.Category)
                .ToList();
        }
    }
}
