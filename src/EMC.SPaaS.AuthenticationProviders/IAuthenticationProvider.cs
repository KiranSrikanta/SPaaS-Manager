using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace EMC.SPaaS.AuthenticationProviders
{
    public interface IAuthenticationProvider
    {
        string Name { get; }

        string QueryStringKeyForCode { get; }

        string GetOAuthUrl(string redirectUrl);

        Token GetToken(string authCode, string redirectUrl);

        string RefreshToken(string refreshCode);
    }

    public class OAuthUserInfo
    {
        public string Name { get; protected set; }
        public string Id { get; protected set; }
        public IDictionary<string, string> Claims { get; protected set; }
    }

    public class Token
    {
        public string Provider { get; protected set; }
        public string Content { get; protected set; }
        public OAuthUserInfo UserInfo { get; protected set; }
    }
}
