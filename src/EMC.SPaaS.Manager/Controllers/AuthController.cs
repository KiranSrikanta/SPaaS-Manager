using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using EMC.SPaaS.AuthenticationProviders;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace EMC.SPaaS.Manager.Controllers
{
    public class AuthController : Controller
    {
        internal AuthenticationProviderFactory ProviderFactory { get; set; }

        string redirectUri = "http://localhost:27934/api/auth/azure/callback";

        public AuthController() : base()
        {
            ProviderFactory = new AuthenticationProviderFactory();
        }

        #region Routes
        // GET api/auth/{providerName}
        [Route("api/[controller]/{provider}")]
        [HttpGet("{provider}")]
        public void Login(string provider)
        {
            var authProvider = ProviderFactory.GetAuthenticationStratagy(provider);

            Response.Redirect(authProvider.GetOAuthUrl(redirectUri));
        }

        // POST api/values
        [HttpGet("{provider}")]
        [Route("api/[controller]/{provider}/callback")]
        public void Callback(string provider)
        {
            var authProvider = ProviderFactory.GetAuthenticationStratagy(provider);

            var code = Request.Query[authProvider.QueryStringKeyForCode][0];

            var token = authProvider.GetToken(code, redirectUri);

            //TODO:SAVE TO DB

            var authData = new Dictionary<string, object>()
            {
                { AuthenticationProperties.Provider, token.Provider },
                { AuthenticationProperties.UserName, token.UserInfo.Name },
                { AuthenticationProperties.UserId, token.UserInfo.Id }
            };

            //TODO:CONFIGURATION
            var secretKey = "SUPERSECRETKEY";
            string sToken = JWT.JsonWebToken.Encode(authData, secretKey, JWT.JwtHashAlgorithm.HS256);

            //TODO:CONFIGURATION
            Response.Cookies.Append("Authorization", sToken,new Microsoft.AspNet.Http.CookieOptions {
                Expires = DateTime.UtcNow.AddMinutes(5)
            });
        }
        #endregion
    }
}
