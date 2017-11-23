using System;

namespace CashFlow.Core.Models
{
    public class Cost
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }

        public int Amount { get; set; }
        public string Name { get; set; }
        public DateTime PayDate { get; set; }

        public Category Category { get; set; }

        // TODO: добавить планирование трат - дату запланированной траты, статус.
        // TODO: добавить отдельную сущность - циклическая плата, которая имеет цикличность, имеет постоянный статус, порождает конкретные платы, маячит в списке пока не оплатишь
        // TODO: сделать лист CostList (ссылаться на него их Cost). Список checkbox'ов, подтверждая которые, ты создаешь автоматом траты (только цену ввести остается)
    }
}
