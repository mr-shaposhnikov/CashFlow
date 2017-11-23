using System;

namespace CashFlow.Core.Models
{
    public class Cash
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }

        // TODO: прибыль повторяет расход, так же есть сумма, описание, дата, неплохо бы иметь категорию...но не хочется поддерживать два дерева классов
    }
}
