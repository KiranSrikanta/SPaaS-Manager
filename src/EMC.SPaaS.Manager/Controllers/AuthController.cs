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
        internal AuthenticationProviderFactory ProviderFactory { get; set; }

        SPaaSDbContext DbContext { get; set; }

        string redirectUri = "http://localhost:27934/api/auth/azure/callback";

        readonly string serverSecret;

        public AuthController(IOptions<AuthenticationConfigurations> authSettings, SPaaSDbContext dbContext)
        {
            OAuthSettingsProvider settingsProvider = new OAuthSettingsProvider(authSettings.Value);
            ProviderFactory = new AuthenticationProviderFactory(settingsProvider.Settings);

            serverSecret = authSettings.Value.ServerSecret;

            DbContext = dbContext;
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
            //var user = new UserEntity();
            //user.Id = 1;
            //user.UserId = "AAAAAAaa";
            //user.UserName = "BBB";
            //DbContext.Add(user);
            //DbContext.SaveChanges();

            var authData = new Dictionary<string, object>()
            {
                { Constants.AuthenticationSession.Properties.Provider, token.Provider },
                { Constants.AuthenticationSession.Properties.UserName, token.UserInfo.Name },
                { Constants.AuthenticationSession.Properties.UserId, token.UserInfo.Id }
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
