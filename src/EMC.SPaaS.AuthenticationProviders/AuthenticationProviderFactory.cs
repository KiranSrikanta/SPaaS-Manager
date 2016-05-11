using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMC.SPaaS.AuthenticationProviders
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class AuthenticationProviderFactory
    {
        List<IAuthenticationProvider> AuthenticationProviders { get; set; }

        public AuthenticationProviderFactory()
        {
            AuthenticationProviders = new List<IAuthenticationProvider>();

            //TODO:CONFIGURABLE
            string tenantId = "e5245dea-1dd2-440c-95cc-4eea226576ce";
            string clientId = "2552a77d-0333-47f6-a3df-8109c519d666";
            string clientSecret = "U4Kq/DLp13L4dAd+/lvfclV+w4jiirvcDnfzZQb8d5A=";
            string resource = "https://management.azure.com/";

            AuthenticationProviders.Add(new AzureAdOAuthProvider(tenantId, clientId, clientSecret, resource));
        }

        public IAuthenticationProvider GetAuthenticationStratagy(string Provider)
        {
            return AuthenticationProviders.Find(a => a.Name.Equals(Provider, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
