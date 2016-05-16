using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using EMC.SPaaS.Entities;
using EMC.SPaaS.Repository;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace EMC.SPaaS.Manager.Controllers
{
    [Route("api/[controller]")]
    public class SharepointDesignController : Controller
    {
        private RepositoryManager Repositories { get; set; }

        public SharepointDesignController(SPaaSDbContext dbContext)
        {
            Repositories = new RepositoryManager(dbContext);
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<DesignEntity> GetAll()
        {

            return Repositories.Designs.GetAll(1);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return Repositories.Designs.Find(id).DesignName;
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
