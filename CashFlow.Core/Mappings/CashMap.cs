using CashFlow.Core.Models;
using FluentNHibernate.Mapping;

namespace CashFlow.Core.Mappings
{
    public class CashMap : ClassMap<Cash>
    {
        public CashMap()
        {
            Id(x => x.Id);

            Map(x => x.Amount).Not.Nullable();
            Map(x => x.Name).Length(500);
            Map(x => x.Date).Not.Nullable().Default("getDate()");
        }
    }
}
