using CashFlow.Core.Models;
using FluentNHibernate.Mapping;

namespace CashFlow.Core.Mappings
{
    public class CategoryMap : ClassMap<Category>
    {
        public CategoryMap()
        {
            Id(x => x.Id);

            Map(x => x.Name).Length(500).Not.Nullable();
            Map(x => x.HexColor).Length(7).Not.Nullable().Default($"'{Category.DEFAULT_COLOR}'");
            Map(x => x.Glyphicon).Length(200).Not.Nullable().Default($"'{Category.DEFAULT_GLYHPICON}'");

            HasMany(x => x.Costs)
                .KeyColumn(nameof(Cost.CategoryId))
                .Inverse()
                .Cascade.All();
        }
    }

    //public class CategoryMap : ClassMapping<Category>
    //{
    //    public CategoryMap()
    //    {
    //        Id(x => x.Id, m => m.Generator(Generators.Native));

    //        Property(x => x.Name, m => m.Column(c =>
    //        {
    //            c.Length(500);
    //            c.NotNullable(true);
    //        }));

    //        Property(x => x.HexColor, m => m.Column(c =>
    //        {
    //            c.Length(7);
    //            c.NotNullable(true);
    //            c.Default($"'{Category.DEFAULT_COLOR}'");
    //        }));

    //        Property(x => x.Glyphicon, m => m.Column(c =>
    //        {
    //            c.Length(200);
    //            c.NotNullable(true);
    //            c.Default($"'{Category.DEFAULT_GLYHPICON}'");
    //        }));

    //        Set(x => x.Costs, c =>
    //        {
    //            c.Fetch(CollectionFetchMode.Join);
    //            c.Cascade(Cascade.All);
    //            c.Inverse(true);

    //            c.Key(k =>
    //            {
    //                k.Column(nameof(Cost.CategoryId));
    //                k.NotNullable(true);
    //            });
    //        });
    //    }
    //}
}
