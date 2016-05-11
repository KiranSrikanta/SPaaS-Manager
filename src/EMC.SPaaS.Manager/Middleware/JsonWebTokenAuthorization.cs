using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using EMC.SPaaS.AuthenticationProviders;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace EMC.SPaaS.Manager
{
    // You may need to install the Microsoft.AspNet.Http.Abstractions package into your project
    public class JsonWebTokenAuthorization
    {
        private readonly RequestDelegate _next;

        public JsonWebTokenAuthorization(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            //TODO:CONFIGURATION
            if (context.Request.Headers.ContainsKey("Authorization"))
            {
                //TODO:CONFIGURATION
                string authHeader = context.Request.Headers["Authorization"];
                var authBits = authHeader.Split(' ');
                if (authBits.Length != 2)
                {
                    //Debug.WriteLine("[JsonWebTokenAuthorization] Ignoring Bad Authorization Header (count!=2)");
                    return _next(context);
                }
                if (!authBits[0].ToLowerInvariant().Equals("bearer"))
                {
                    //Debug.WriteLine("[JsonWebTokenAuthorization] Ignoring Bad Authorization Header (type!=bearer)");
                    return _next(context);
                }

                try
                {
                    //TODO:CONFIGURATION
                    var secretKey = "SUPERSECRETKEY";
                    try
                    {
                        var payload = JWT.JsonWebToken.DecodeToObject(authBits[1], secretKey) as IDictionary<string, object>;

                        List<System.Security.Claims.Claim> claims = new List<System.Security.Claims.Claim>();
                        claims.Add(new System.Security.Claims.Claim(AuthenticationProperties.UserId, payload[AuthenticationProperties.UserId].ToString()));
                        claims.Add(new System.Security.Claims.Claim(AuthenticationProperties.UserName, payload[AuthenticationProperties.UserName].ToString()));
                        context.User.AddIdentity(new System.Security.Claims.ClaimsIdentity(claims, payload[AuthenticationProperties.Provider].ToString()));
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

    public static class AuthenticationProperties
    {
        public const string UserName = "UserName";
        public const string UserId = "UserId";
        public const string Provider = "Provider";
    }
}
