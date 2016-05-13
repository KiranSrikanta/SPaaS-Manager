using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using EMC.SPaaS.AuthenticationProviders;
using Microsoft.Extensions.OptionsModel;
using EMC.SPaaS.Entities;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace EMC.SPaaS.Manager.Controllers
{
    public class AuthController : Controller
    {
        internal AuthenticationStratagies AllAuthenticationStratagies { get; set; }

        SPaaSDbContext DbContext { get; set; }

        //TODO:Make hostname dynamic
        const string redirectUri = "http://localhost:27934/api/auth/azure/callback";

        readonly string serverSecret;

        public AuthController(AuthenticationStratagies authStratagies, SPaaSDbContext dbContext, IOptions<WebAppConfigurations> appConfigs)
        {
            AllAuthenticationStratagies = authStratagies;

            serverSecret = appConfigs.Value.ServerSecret;

            DbContext = dbContext;
        }

        #region Routes
        // GET api/auth/{providerName}
        [Route("api/[controller]/{provider}")]
        [HttpGet("{provider}")]
        public void Login(string provider)
        {
            var authProvider = AllAuthenticationStratagies.GetAuthenticationStratagyForProvider(provider);

            Response.Redirect(authProvider.GetOAuthUrl(redirectUri));
        }

        // POST api/values
        [HttpGet("{provider}")]
        [Route("api/[controller]/{provider}/callback")]
        public void Callback(string provider)
        {
            var authProvider = AllAuthenticationStratagies.GetAuthenticationStratagyForProvider(provider);

            var code = Request.Query[authProvider.QueryStringKeyForCode][0];

            var token = authProvider.GetToken(code, redirectUri);

            var userToSave = DbContext.Users.FirstOrDefault(u => u.UserId == token.UserInfo.Email);
            bool newUser = false;
            if (userToSave == null)
            {
                userToSave = new UserEntity(); newUser = true;
            }

            userToSave.UserId = token.UserInfo.Email;
            userToSave.UserName = token.UserInfo.Name;
            userToSave.AccessToken = token.RawContent;
            userToSave.AuthenticationProvider = token.Provider;

            if (newUser)
                DbContext.Add(userToSave);
            else
                DbContext.Update(userToSave);

            DbContext.SaveChanges();

            var authData = new Dictionary<string, object>()
            {
                { Constants.AuthenticationSession.Properties.Provider, token.Provider },
                { Constants.AuthenticationSession.Properties.UserName, token.UserInfo.Name },
                { Constants.AuthenticationSession.Properties.Email, token.UserInfo.Email },
                { Constants.AuthenticationSession.Properties.UserId, userToSave.Id }
            };

            string sToken = JWT.JsonWebToken.Encode(authData, serverSecret, JWT.JwtHashAlgorithm.HS256);

            Response.Cookies.Append(Constants.AuthenticationSession.CookieKey, sToken, new Microsoft.AspNet.Http.CookieOptions
            {
                Expires = DateTime.UtcNow.AddMinutes(5)
            });
        }
        #endregion
    }
}
