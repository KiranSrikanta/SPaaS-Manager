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

        string GetApiAccessToken(string token);

        string RefreshToken(string refreshCode);
    }

    public class OAuthUserInfo
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class Token
    {
        public string Provider { get; set; }
        public string RawContent { get; set; }
        public OAuthUserInfo UserInfo { get; set; }
    }
}
