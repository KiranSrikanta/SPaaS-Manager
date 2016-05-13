using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using EMC.SPaaS.Utility;

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
        public AzureAdOAuthProvider(IConfigurationSection settings)
        {
            this.TenantId = settings[GlobalConstants.CloudProviders.Azure.ConfigurationKeys.TenantId];
            this.ClientId = settings[GlobalConstants.CloudProviders.Azure.ConfigurationKeys.ClientId];
            this.ClientSecret = settings[GlobalConstants.CloudProviders.Azure.ConfigurationKeys.ClientSecret];
            this.Resource = settings[GlobalConstants.CloudProviders.Azure.ConfigurationKeys.Resource];
        }
        #endregion

        #region IAuthenticationProvider implementation
        public string Name
        {
            get
            {
                return GlobalConstants.CloudProviders.Azure.Name;
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

            return AzureOAuthTokenHelper.ParseToken(responseToken);
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

        public string GetApiAccessToken(string token)
        {
            return AzureOAuthTokenHelper.GetApiToken(token);
        }
        #endregion
    }
}
