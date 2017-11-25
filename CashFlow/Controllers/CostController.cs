using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CashFlow.Core.Models;
using CashFlow.Core.Repositories;

namespace CashFlow.Controllers
{
    [Route("api/[controller]")]
    public class CostController : Controller
    {
        private readonly ICostRepository _repository = Startup.IoContainer.Resolve<ICostRepository>();
        private readonly ICategoryRepository _categoryRepository = Startup.IoContainer.Resolve<ICategoryRepository>();

        // GET: api/values
        [HttpGet]
        public IEnumerable<Cost> Get()
        {
            return _repository.Select();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Cost Get(int id)
        {
            return _repository.SingleBy(x => x.Id == id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]Cost value)
        {
            if (value?.CategoryId > 0)
            {
                value.Category = _categoryRepository.SingleBy(x => x.Id == value.CategoryId);
            }
            _repository.Insert(value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Cost value)
        {
            value.Id = id;
            _repository.Update(value);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var value = _repository.SingleBy(x => x.Id == id);
            if (value != null) _repository.Delete(value);
        }
    }
}
