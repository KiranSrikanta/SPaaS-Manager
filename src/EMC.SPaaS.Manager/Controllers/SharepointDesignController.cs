using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using EMC.SPaaS.RepositoryManager;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace EMC.SPaaS.Manager.Controllers
{
    [Route("api/[controller]")]
    public class SharepointDesignController : Controller
    {
        private IRepository<DesignItem, string> _repository;

        public SharepointDesignController(IRepository<DesignItem, string> repo)
        {
            _repository = repo;
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<DesignItem> GetAll()
        {

            return _repository.GetAll();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
