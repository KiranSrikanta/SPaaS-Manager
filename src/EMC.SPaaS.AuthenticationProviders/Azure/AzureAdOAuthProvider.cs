using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EMC.SPaaS.AuthenticationProviders
{
    public class AzureAdOAuthProvider : IAuthenticationProvider
    {
        #region properties
        public string TenantId { get; private set; }
        public string ClientId { get; private set; }
        internal string ClientSecret { get; set; }
        public string Resource { get; private set; }
        #endregion

        #region ctor
        public AzureAdOAuthProvider(string TenantId, string ClientId, string ClientSecret, string Resource)
        {
            this.TenantId = TenantId;
            this.ClientId = ClientId;
            this.ClientSecret = ClientSecret;
            this.Resource = Resource;
        }
        #endregion

        #region IAuthenticationProvider implementation
        public string Name
        {
            get
            {
                return AzureConstants.ProviderName;
            }
        }

        public string QueryStringKeyForCode
        {
            get
            {
                return "code";
            }
        }

        public string GetOAuthUrl(string redirectUrl)
        {
            return $"https://login.microsoftonline.com/{TenantId}/oauth2/authorize?client_id={ClientId}&response_type=code&redirect_uri={redirectUrl}";
        }

        public Token GetToken(string authCode, string redirectUrl)
        {
            string responseToken;
            using (var client = new WebClient())
            {
                var values = new NameValueCollection();
                values["grant_type"] = "authorization_code";
                values["client_id"] = ClientId;
                values["client_secret"] = ClientSecret;
                values["code"] = authCode;
                values["redirect_uri"] = redirectUrl;
                values["resource"] = Resource;

                var response = client.UploadValues($"https://login.microsoftonline.com/{TenantId}/oauth2/token", values);

                responseToken = Encoding.Default.GetString(response);
            }

            return new AzureToken(responseToken);
        }

        public string RefreshToken(string refreshCode)
        {
            throw new NotImplementedException();

            //string responseString;
            //using (var client = new WebClient())
            //{
            //    var values = new NameValueCollection();
            //    //values["grant_type"] = "authorization_code";
            //    values["client_id"] = ClientId;
            //    values["client_secret"] = ClientSecret;
            //    values["refresh_token"] = refreshCode;
            //    //values["redirect_uri"] = redirectUrl;
            //    values["resource"] = Resource;

            //    var response = client.UploadValues($"https://login.microsoftonline.com/{TenantId}/oauth2/token", values);

            //    responseString = Encoding.Default.GetString(response);
            //}

            //return responseString;
        }

        public IDictionary<string, string> ParseTokenContent(string token)
        {
            var azureToken = Newtonsoft.Json.JsonConvert.DeserializeObject<AzureAccessToken>(token, new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore
            });

            Dictionary<string, string> tokenContent = new Dictionary<string, string>();
            tokenContent.Add(nameof(azureToken.access_token), azureToken.access_token);
            tokenContent.Add(nameof(azureToken.expires_in), azureToken.expires_in);
            tokenContent.Add(nameof(azureToken.expires_on), azureToken.expires_on);
            tokenContent.Add(nameof(azureToken.id_token), azureToken.id_token);
            tokenContent.Add(nameof(azureToken.not_before), azureToken.not_before);
            tokenContent.Add(nameof(azureToken.refresh_token), azureToken.refresh_token);
            tokenContent.Add(nameof(azureToken.resource), azureToken.resource);
            tokenContent.Add(nameof(azureToken.scope), azureToken.scope);
            tokenContent.Add(nameof(azureToken.token_type), azureToken.token_type);

            return tokenContent;
        }

        #region GetToken method return classes
        class AzureUserInfo : OAuthUserInfo
        {
            public AzureUserInfo(UserInfoToken id_token) : base()
            {
                OtherProperties = new Dictionary<string, string>();
                OtherProperties.Add(nameof(id_token.aud), id_token.aud);
                OtherProperties.Add(nameof(id_token.email), id_token.email);
                OtherProperties.Add(nameof(id_token.exp), id_token.exp.ToString());
                OtherProperties.Add(nameof(id_token.family_name), id_token.family_name);
                OtherProperties.Add(nameof(id_token.given_name), id_token.given_name);
                OtherProperties.Add(nameof(id_token.iat), id_token.iat.ToString());
                OtherProperties.Add(nameof(id_token.idp), id_token.idp);
                OtherProperties.Add(nameof(id_token.ipaddr), id_token.ipaddr);
                OtherProperties.Add(nameof(id_token.iss), id_token.iss);
                OtherProperties.Add(nameof(id_token.name), id_token.name);
                OtherProperties.Add(nameof(id_token.nbf), id_token.nbf.ToString());
                OtherProperties.Add(nameof(id_token.oid), id_token.oid);
                OtherProperties.Add(nameof(id_token.sub), id_token.sub);
                OtherProperties.Add(nameof(id_token.tid), id_token.tid);
                OtherProperties.Add(nameof(id_token.unique_name), id_token.unique_name);
                OtherProperties.Add(nameof(id_token.ver), id_token.ver);

                base.Email = id_token.email;
                base.Name = id_token.name;
            }
        }

        class AzureToken : Token
        {
            public AzureToken(string content)
            {
                Provider = AzureConstants.ProviderName;

                RawContent = content;

                var token = Newtonsoft.Json.JsonConvert.DeserializeObject<AzureAccessToken>(RawContent, new JsonSerializerSettings
                {
                    MissingMemberHandling = MissingMemberHandling.Ignore
                });

                var userInfoString = JWT.JsonWebToken.Decode(token.id_token, string.Empty, false);

                var userInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<UserInfoToken>(userInfoString, new JsonSerializerSettings
                {
                    MissingMemberHandling = MissingMemberHandling.Ignore
                });

                UserInfo = new AzureUserInfo(userInfo);
            }
        } 
        #endregion
        #endregion

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
