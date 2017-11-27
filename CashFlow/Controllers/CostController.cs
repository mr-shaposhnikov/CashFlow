using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CashFlow.Core.Models;
using CashFlow.Core.Repositories;
using JetBrains.Annotations;

namespace CashFlow.Controllers
{
    [Route("api/[controller]")]
    public class CostController : Controller
    {
        private readonly ICostRepository _repository;
        private readonly ICategoryRepository _categoryRepository;

        public CostController([NotNull] ICostRepository repository, [NotNull] ICategoryRepository categoryRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository)); ;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<Cost> Get()
        {
            var results = _repository.Select();
            return results;
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
