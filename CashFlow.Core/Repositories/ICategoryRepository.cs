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

    public class CategoryRepository : ICategoryRepository
    {
        private readonly ISession _session;

        public CategoryRepository([NotNull] ISession session)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
        }

        public void Insert([NotNull] Category value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            _session.Save(value);
        }

        public void Update([NotNull] Category value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            _session.Update(value);
        }

        public void Delete([NotNull] Category value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            _session.Delete(value);
        }
        
        public Category SingleBy(Expression<Func<Category, bool>> query)
        {
            return _session.Query<Category>()
                .FetchMany(c => c.Costs)
                .FirstOrDefault(query);
        }

        public IList<Category> Select()
        {
            return _session.Query<Category>()
                .FetchMany(c => c.Costs)
                .ToList();
        }

        public IList<Category> SelectBy(Expression<Func<Category, bool>> query)
        {
            return _session.Query<Category>()
                .Where(query)
                .FetchMany(c => c.Costs)
                .ToList();
        }
    }
}
