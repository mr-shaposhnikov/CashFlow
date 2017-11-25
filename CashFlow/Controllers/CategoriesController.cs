using CashFlow.Core.Models;
using CashFlow.Core.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CashFlow.Controllers
{
    [Route("api/[controller]")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository _repository = Startup.IoContainer.Resolve<ICategoryRepository>();

        // GET: api/values
        [HttpGet]
        public IEnumerable<Category> Get()
        {
            return _repository.Select();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Category Get(int id)
        {
            return _repository.SingleBy(x => x.Id == id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]Category value)
        {
            _repository.Insert(value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Category value)
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
