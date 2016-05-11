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

    public class OAuthSettingsProvider
    {
        public OAuthSettings Settings { get; private set; }

        public OAuthSettingsProvider(AuthenticationConfigurations config)
        {
            var azureSettings = new AzureOAuthSettings(config.AzureClientTenantId, config.AzureClientID, config.AzureClientSecret, config.AzureClientResource);
            Settings = new OAuthSettings();
            Settings.Azure = azureSettings;
        }
    }
}
