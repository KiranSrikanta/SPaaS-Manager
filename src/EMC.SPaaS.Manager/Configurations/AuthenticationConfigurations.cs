using EMC.SPaaS.AuthenticationProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMC.SPaaS.Manager
{
    public class AuthenticationConfigurations
    {
        public string AzureClientID { get; set; }
        public string AzureClientSecret { get; set; }
        public string AzureClientTenantId { get; set; }
        public string AzureClientResource { get; set; }
        public string ServerSecret { get; set; }
    }

    public class OAuthProviderSettings
    {
        public dynamic Azure { get; set; }

        public OAuthProviderSettings(AuthenticationConfigurations config)
        {
            Azure = new
            {
                TenantId = config.AzureClientTenantId,
                ClientId = config.AzureClientID,
                ClientSecret = config.AzureClientSecret,
                Resource = config.AzureClientResource
            };
        }
    }
}
