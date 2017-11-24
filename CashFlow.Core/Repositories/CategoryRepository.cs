using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using CashFlow.Core.Models;
using JetBrains.Annotations;
using NHibernate;
using NHibernate.Linq;

namespace CashFlow.Core.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    { }

    public class CategoryRepository : AbstractRepository<Category>, ICategoryRepository
    {
        public CategoryRepository([NotNull] ISession session)
            : base(session)
        { }
        
        public override Category SingleBy(Expression<Func<Category, bool>> query)
        {
            return Session.Query<Category>()
                .FetchMany(c => c.Costs)
                .FirstOrDefault(query);
        }

        public override IList<Category> Select()
        {
            return Session.Query<Category>()
                .FetchMany(c => c.Costs)
                .ToList();
        }

        public override IList<Category> SelectBy(Expression<Func<Category, bool>> query)
        {
            return Session.Query<Category>()
                .Where(query)
                .FetchMany(c => c.Costs)
                .ToList();
        }
    }
}
