using System.Collections.Generic;

namespace CashFlow.Core.Models
{
    public class Category
    {
        public const string DEFAULT_COLOR = "#AAAAAA";
        public const string DEFAULT_GLYHPICON = "glyphicon-asterisk";

        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string HexColor { get; set; } = DEFAULT_COLOR;
        public virtual string Glyphicon { get; set; } = DEFAULT_GLYHPICON;

        public virtual IList<Cost> Costs { get; set; }
    }
}
