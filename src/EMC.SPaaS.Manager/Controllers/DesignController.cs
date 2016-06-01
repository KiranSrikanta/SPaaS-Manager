using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using EMC.SPaaS.Entities;
using EMC.SPaaS.Repository;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace EMC.SPaaS.Manager.Controllers
{
    
    public class DesignController : Controller
    {
        private RepositoryManager Repositories { get; set; }

        public DesignController(SPaaSDbContext dbContext)
        {
            Repositories = new RepositoryManager(dbContext);
        }
        
        [Route("api/[controller]/Sharepoint")]
        [HttpGet]
        public IEnumerable<DesignEntity> GetAll()
        {

            return Repositories.Designs.GetAll(1);
        }
        [Route("api/[controller]/Sharepoint")]
        // GET api/values/5
        [HttpGet("{id}")]
        public DesignEntity Get(int id)
        {
            return Repositories.Designs.Find(id);
        }

        // POST api/values
        [Route("api/[controller]/Sharepoint")]
        [HttpPost]
        public void Post([FromBody] JObject itemDesign)
        {
            if(itemDesign != null)
            {
                dynamic jsonData = itemDesign;
                var objDesign = jsonData.Designs["compDesign"][0].ToObject<DesignEntity>();
                var objVMDesign = jsonData.Designs["compVMDesign"][0].ToObject<VMDesignEntity>();

                Repositories.Designs.Add(objDesign);
                Repositories.VMDesigns.Add(objVMDesign);

                Repositories.Save();
            }
        }

        // PUT api/values/5
        [Route("api/[controller]/Sharepoint")]
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [Route("api/[controller]/Sharepoint")]
        [HttpDelete("{id}/{userID}")]
        public void Delete(int id,int userID)
        {
            Repositories.Designs.Remove(id, userID);
            Repositories.VMDesigns.Remove(id, userID);
            Repositories.Save();

        }
    }
}
