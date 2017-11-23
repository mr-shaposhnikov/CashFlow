using System.Collections.Generic;
using System.Drawing;

namespace CashFlow.Core.Models
{
    public class Category
    {
        public const string DEFAULT_COLOR = "#AAAAAA";
        public const string DEFAULT_GLYHPICON = "glyphicon-asterisk";

        public int Id { get; set; }
        public string Name { get; set; }

        public string HexColor { get; set; } = DEFAULT_COLOR;
        public Color Color
        {
            get => ColorTranslator.FromHtml(HexColor);
            set => HexColor = ColorTranslator.ToHtml(value);
        }

        public string Glyphicon { get; set; } = DEFAULT_GLYHPICON;

        public IList<Cost> Costs { get; set; }
    }
}
