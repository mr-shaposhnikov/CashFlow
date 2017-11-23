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
            Map(x => x.HexColor).Length(7).Not.Nullable().Default(Category.DEFAULT_COLOR);
            Map(x => x.Glyphicon).Length(200).Not.Nullable().Default(Category.DEFAULT_GLYHPICON);
            
            HasMany(x => x.Costs).Inverse().Cascade.All().KeyColumn(nameof(Cost.CategoryId));
        }
    }
}
