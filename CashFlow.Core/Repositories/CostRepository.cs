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
        private readonly ICategoryRepository _categoryRepository;

        public CostRepository([NotNull] ISession session, [NotNull] ICategoryRepository categoryRepository)
            : base(session)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        }

        public override void Insert([NotNull] Cost value)
        {
            var category = _categoryRepository.SingleBy(x => x.Id == value.CategoryId);
            category.Costs.Add(value);
            value.Category = category;

            base.Insert(value);
        }

        public override void Update([NotNull] Cost value)
        {
            var category = _categoryRepository.SingleBy(x => x.Id == value.CategoryId);
            if (!category.Costs.Contains(value)) throw new Exception("Попытка обновления несуществующей записи!");
            value.Category = category;

            base.Update(value);
        }

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
                .ToArray();
        }

        public override IList<Cost> SelectBy(Expression<Func<Cost, bool>> query)
        {
            return Session.Query<Cost>()
                .Where(query)
                .Fetch(x => x.Category)
                .ToArray();
        }
    }
}
