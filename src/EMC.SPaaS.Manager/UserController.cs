using EMC.SPaaS.Entities;
using EMC.SPaaS.Repository;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace EMC.SPaaS.Manager
{
    public class UserController : Controller
    {
        private RepositoryManager Repositories { get; set; }

        public UserController(SPaaSDbContext dbContext)
        {
            Repositories = new RepositoryManager(dbContext);
        }

        // GET: api/me
        [Route("api/me")]
        [HttpGet]
        [Authorize]
        public JsonResult Get()
        {
            var sUserId = User.FindAll(Constants.AuthenticationSession.Properties.UserId).FirstOrDefault().Value;
            int userId = int.Parse(sUserId);

            var user = Repositories.Users.GetUser(userId);

            return new JsonResult(new {
                UserId = user.UserId,
                UserName = user.UserName,
                Provider = user.AuthenticationProvider
            });
        }
    }
}
