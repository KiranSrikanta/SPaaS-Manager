using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.OptionsModel;

namespace EMC.SPaaS.Manager
{
    // You may need to install the Microsoft.AspNet.Http.Abstractions package into your project
    public class JsonWebTokenAuthorization
    {
        private readonly RequestDelegate _next;

        private readonly string serverSecret;

        public JsonWebTokenAuthorization(RequestDelegate next, IOptions<AuthenticationConfigurations> authSettings)
        {
            _next = next;
            serverSecret = authSettings.Value.ServerSecret;
        }

        public Task Invoke(HttpContext context)
        {
            if (context.Request.Headers.ContainsKey(Constants.AuthenticationSession.HtmlHeader))
            {
                string authHeader = context.Request.Headers[Constants.AuthenticationSession.HtmlHeader];
                var authBits = authHeader.Split(' ');
                if (authBits.Length != 2)
                {
                    //Debug.WriteLine("[JsonWebTokenAuthorization] Ignoring Bad Authorization Header (count!=2)");
                    return _next(context);
                }
                if (!authBits[0].ToLowerInvariant().Equals(Constants.AuthenticationSession.HeaderStartsWith))
                {
                    //Debug.WriteLine("[JsonWebTokenAuthorization] Ignoring Bad Authorization Header (type!=bearer)");
                    return _next(context);
                }

                try
                {
                    try
                    {
                        var payload = JWT.JsonWebToken.DecodeToObject(authBits[1], serverSecret) as IDictionary<string, object>;

                        List<System.Security.Claims.Claim> claims = new List<System.Security.Claims.Claim>();
                        claims.Add(new System.Security.Claims.Claim(Constants.AuthenticationSession.Properties.UserId, payload[Constants.AuthenticationSession.Properties.UserId].ToString()));
                        claims.Add(new System.Security.Claims.Claim(Constants.AuthenticationSession.Properties.Email, payload[Constants.AuthenticationSession.Properties.Email].ToString()));
                        claims.Add(new System.Security.Claims.Claim(Constants.AuthenticationSession.Properties.UserName, payload[Constants.AuthenticationSession.Properties.UserName].ToString()));
                        context.User.AddIdentity(new System.Security.Claims.ClaimsIdentity(claims, payload[Constants.AuthenticationSession.Properties.Provider].ToString()));
                    }
                    catch (JWT.SignatureVerificationException)
                    {
                        Console.WriteLine("Invalid token!");
                    }
                    
                }
                catch (JWT.SignatureVerificationException)
                {
                    //Debug.WriteLine("[JsonWebTokenAuthorization] Ignoring Bad Authorization (JWT signature doesn't match)");
                    return _next(context);
                }
                catch (FormatException)
                {
                    //Debug.WriteLine("[JsonWebTokenAuthorization] Ignoring Bad Client Secret");
                    return _next(context);
                }

                //Debug.WriteLine(string.Format("[JsonWebTokenAuthorization] JWT Decoded as {0}", claims));
            }
            //Debug.WriteLine("In JsonWebTokenAuthorization.Invoke");
            return _next(context);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class JsonWebTokenAuthorizationExtensions
    {
        public static IApplicationBuilder UseJsonWebTokenAuthorization(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JsonWebTokenAuthorization>();
        }
    }
}
