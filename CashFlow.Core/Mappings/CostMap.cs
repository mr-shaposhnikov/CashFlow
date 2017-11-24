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

            References(x => x.Category).Not.Nullable().Column(nameof(Cost.CategoryId)).Cascade.SaveUpdate();
        }
    }

    //public class CostMap : ClassMapping<Cost>
    //{
    //    public CostMap()
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

    //        Property(x => x.PayDate, m => m.Column(c =>
    //        {
    //            c.NotNullable(true);
    //            c.Default("getDate()");
    //        }));

    //        ManyToOne(x => x.Category,
    //            c =>
    //            {
    //                c.Cascade(Cascade.Persist);
    //                c.Column(nameof(Cost.CategoryId));
    //            }
    //        );
    //    }
    //}
}
