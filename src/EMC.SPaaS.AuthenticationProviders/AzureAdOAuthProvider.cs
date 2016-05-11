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
        public AzureOAuthSettings Settings { get; private set; }

        public AzureAdOAuthProvider(AzureOAuthSettings settings)
        {
            Settings = settings;
        }

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
            return $"https://login.microsoftonline.com/{Settings.TenantId}/oauth2/authorize?client_id={Settings.ClientId}&response_type=code&redirect_uri={redirectUrl}";
        }

        public Token GetToken(string authCode, string redirectUrl)
        {
            string responseToken;
            using (var client = new WebClient())
            {
                var values = new NameValueCollection();
                values["grant_type"] = "authorization_code";
                values["client_id"] = Settings.ClientId;
                values["client_secret"] = Settings.ClientSecret;
                values["code"] = authCode;
                values["redirect_uri"] = redirectUrl;
                values["resource"] = Settings.Resource;

                var response = client.UploadValues($"https://login.microsoftonline.com/{Settings.TenantId}/oauth2/token", values);

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

        class AzureAccessToken
        {
            public string id_token { get; set; }
        }

        class IdToken
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

        class AzureUserInfo : OAuthUserInfo
        {
            public AzureUserInfo(IdToken id_token) : base()
            {
                Claims = new Dictionary<string, string>();
                Claims.Add(nameof(id_token.aud), id_token.aud);
                Claims.Add(nameof(id_token.email), id_token.email);
                Claims.Add(nameof(id_token.exp), id_token.exp.ToString());
                Claims.Add(nameof(id_token.family_name), id_token.family_name);
                Claims.Add(nameof(id_token.given_name), id_token.given_name);
                Claims.Add(nameof(id_token.iat), id_token.iat.ToString());
                Claims.Add(nameof(id_token.idp), id_token.idp);
                Claims.Add(nameof(id_token.ipaddr), id_token.ipaddr);
                Claims.Add(nameof(id_token.iss), id_token.iss);
                Claims.Add(nameof(id_token.name), id_token.name);
                Claims.Add(nameof(id_token.nbf), id_token.nbf.ToString());
                Claims.Add(nameof(id_token.oid), id_token.oid);
                Claims.Add(nameof(id_token.sub), id_token.sub);
                Claims.Add(nameof(id_token.tid), id_token.tid);
                Claims.Add(nameof(id_token.unique_name), id_token.unique_name);
                Claims.Add(nameof(id_token.ver), id_token.ver);

                base.Id = id_token.email;
                base.Name = id_token.name;
            }
        }

        class AzureToken : Token
        {
            public AzureToken(string content)
            {
                Provider = AzureConstants.ProviderName;

                Content = content;

                var token = Newtonsoft.Json.JsonConvert.DeserializeObject<AzureAccessToken>(Content, new JsonSerializerSettings
                {
                    MissingMemberHandling = MissingMemberHandling.Ignore
                });

                var userInfoString = JWT.JsonWebToken.Decode(token.id_token, string.Empty, false);

                var userInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<IdToken>(userInfoString, new JsonSerializerSettings
                {
                    MissingMemberHandling = MissingMemberHandling.Ignore
                });

                UserInfo = new AzureUserInfo(userInfo);
            }
        }
    }

    public class AzureOAuthSettings
    {
        public AzureOAuthSettings(string tenantId, string clientId, string clientSecret, string resource)
        {
            TenantId = tenantId;
            ClientId = clientId;
            ClientSecret = clientSecret;
            Resource = resource;
        }

        public string TenantId { get; private set; }
        public string ClientId { get; private set; }
        internal string ClientSecret { get; set; }
        public string Resource { get; private set; }
    }
}
