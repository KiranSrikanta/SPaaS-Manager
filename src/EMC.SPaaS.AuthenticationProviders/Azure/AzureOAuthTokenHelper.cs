using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EMC.SPaaS.AuthenticationProviders
{
    public static class AzureOAuthTokenHelper
    {
        public static Token ParseToken(string accessToken)
        {
            var token = Newtonsoft.Json.JsonConvert.DeserializeObject<AzureAccessToken>(accessToken, new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore
            });

            var userInfoString = JWT.JsonWebToken.Decode(token.id_token, string.Empty, false);

            var userInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<UserInfoToken>(userInfoString, new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore
            });

            return new Token
            {
                Provider = AzureConstants.ProviderName,
                RawContent = accessToken,
                UserInfo = new OAuthUserInfo
                {
                    Email = userInfo.email,
                    Name = userInfo.name
                }
            };
        }

        public static string GetApiToken(string accessToken)
        {
            var token = Newtonsoft.Json.JsonConvert.DeserializeObject<AzureAccessToken>(accessToken, new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore
            });

            return token.access_token;
        }
        #region response token classes
        class AzureAccessToken
        {
            public string token_type { get; set; }
            public string scope { get; set; }
            public string expires_in { get; set; }
            public string expires_on { get; set; }
            public string not_before { get; set; }
            public string resource { get; set; }
            public string access_token { get; set; }
            public string refresh_token { get; set; }
            public string id_token { get; set; }
        }

        class UserInfoToken
        {
            public string aud { get; set; }
            public string iss { get; set; }
            public long iat { get; set; }
            public long nbf { get; set; }
            public long exp { get; set; }
            public string email { get; set; }
            public string family_name { get; set; }
            public string given_name { get; set; }
            public string idp { get; set; }
            public string ipaddr { get; set; }
            public string name { get; set; }
            public string oid { get; set; }
            public string sub { get; set; }
            public string tid { get; set; }
            public string unique_name { get; set; }
            public string ver { get; set; }
        }
        #endregion
    }
}
