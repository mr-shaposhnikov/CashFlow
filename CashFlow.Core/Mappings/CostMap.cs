using CashFlow.Core.Models;
using FluentNHibernate.Mapping;

namespace CashFlow.Core.Mappings
{
    public class CostMap : ClassMap<Cost>
    {
        public CostMap()
        {
            Id(x => x.Id);
            
            Map(x => x.Amount).Not.Nullable();
            Map(x => x.Name).Length(500);
            Map(x => x.PayDate).Not.Nullable().Default("getDate()");

            References(x => x.Category).Column(nameof(Cost.CategoryId)).Not.Nullable();
        }
    }
}
