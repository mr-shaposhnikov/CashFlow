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
            Map(x => x.Name).Nullable().Length(500);
            Map(x => x.Date).Not.Nullable().Default("GETUTCDATE()");
        }
    }

    //public class CashMap : ClassMapping<Cash>
    //{
    //    public CashMap()
    //    {
    //        Id(x => x.Id, m => m.Generator(Generators.Native));

    //        Property(x => x.Amount, m => m.Column(c =>
    //        {
    //            c.NotNullable(true);
    //        }));

    //        Property(x => x.Name, m => m.Column(c =>
    //        {
    //            c.Length(500);
    //        }));

    //        Property(x => x.Date, m => m.Column(c =>
    //        {
    //            c.NotNullable(true);
    //            c.Default("getDate()");
    //        }));
    //    }
    //}
}
