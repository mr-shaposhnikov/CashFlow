using System;

namespace CashFlow.Core.Models
{
    public class Cost
    {
        public virtual int Id { get; set; }
        public virtual int CategoryId { get; set; }

        public virtual int Amount { get; set; }
        public virtual string Name { get; set; }
        public virtual DateTime PayDate { get; set; }
        
        public virtual Category Category { get; set; }

        // TODO: добавить планирование трат - дату запланированной траты, статус.
        // TODO: добавить отдельную сущность - циклическая плата, которая имеет цикличность, имеет постоянный статус, порождает конкретные платы, маячит в списке пока не оплатишь
        // TODO: сделать лист CostList (ссылаться на него их Cost). Список checkbox'ов, подтверждая которые, ты создаешь автоматом траты (только цену ввести остается)
    }
}
