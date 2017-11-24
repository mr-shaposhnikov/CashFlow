using System;

namespace CashFlow.Core.Models
{
    public class Cash
    {
        public virtual int Id { get; set; }
        public virtual int Amount { get; set; }
        public virtual string Name { get; set; }
        public virtual DateTime Date { get; set; }

        // TODO: прибыль повторяет расход, так же есть сумма, описание, дата, неплохо бы иметь категорию...но не хочется поддерживать два дерева классов
    }
}
