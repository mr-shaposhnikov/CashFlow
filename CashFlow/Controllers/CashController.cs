using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CashFlow.Core.Models;
using CashFlow.Core.Repositories;

namespace CashFlow.Controllers
{
    [Route("api/[controller]")]
    public class CashController : Controller
    {
        private readonly ICashRepository _repository = Startup.IoContainer.Resolve<ICashRepository>();
        
        // GET: api/values
        [HttpGet]
        public IEnumerable<Cash> Get()
        {
            return _repository.Select();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Cash Get(int id)
        {
            return _repository.SingleBy(x => x.Id == id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]Cash value)
        {
            _repository.Insert(value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Cash value)
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
