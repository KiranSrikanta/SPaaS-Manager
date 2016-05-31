using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using EMC.SPaaS.ProvisioningEngine;
using EMC.SPaaS.Entities;
using EMC.SPaaS.Repository;
using Microsoft.AspNet.Authorization;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace EMC.SPaaS.Manager
{
    public class CloudProviderOptionsController : Controller
    {
        private ProvisionerFactory ProvisioningFactory
        {
            get; set;
        }

        private RepositoryManager Repositories { get; set; }

        public CloudProviderOptionsController(ProvisionerFactory provisioningFactory, SPaaSDbContext dbContext)
        {
            ProvisioningFactory = provisioningFactory;
            Repositories = new RepositoryManager(dbContext);
        }

        // GET: api/cloudProviderOptions/vmtype
        [Route("api/[controller]/vmtype")]
        [HttpGet]
        [Authorize]
        public JsonResult GetVmTypes()
        {
            var sUserId = User.FindAll(Constants.AuthenticationSession.Properties.UserId).FirstOrDefault().Value;
            int userId = int.Parse(sUserId);

            var user = Repositories.Users.GetUser(userId);

            var provisioner = ProvisioningFactory.CreateProvisioner(user);

            return new JsonResult(provisioner.VMOptions());
        }

        // GET: api/cloudProviderOptions/os
        [Route("api/[controller]/os")]
        [HttpGet]
        [Authorize]
        public JsonResult GetOSImages()
        {
            var sUserId = User.FindAll(Constants.AuthenticationSession.Properties.UserId).FirstOrDefault().Value;
            int userId = int.Parse(sUserId);

            var user = Repositories.Users.GetUser(userId);

            var provisioner = ProvisioningFactory.CreateProvisioner(user);

            return new JsonResult(provisioner.OSImageOptions());
        }
    }
}
