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

        public AuthenticationProviderFactory(OAuthSettings Settings)
        {
            AuthenticationProviders = new List<IAuthenticationProvider>();

            AuthenticationProviders.Add(new AzureAdOAuthProvider(Settings.Azure));
        }

        public IAuthenticationProvider GetAuthenticationStratagy(string Provider)
        {
            return AuthenticationProviders.Find(a => a.Name.Equals(Provider, StringComparison.CurrentCultureIgnoreCase));
        }
    }

    public class OAuthSettings
    {
        public AzureOAuthSettings Azure { get; set; }
    }
}
