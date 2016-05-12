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

        IDictionary<string, string> ParseTokenContent(string token);

        string RefreshToken(string refreshCode);
    }

    public class OAuthUserInfo
    {
        public string Name { get; protected set; }
        public string Email { get; protected set; }
        public IDictionary<string, string> OtherProperties { get; protected set; }
    }

    public class Token
    {
        public string Provider { get; protected set; }
        public string RawContent { get; protected set; }
        public OAuthUserInfo UserInfo { get; protected set; }
    }
}
