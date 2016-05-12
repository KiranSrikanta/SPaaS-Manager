using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMC.SPaaS.AuthenticationProviders
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class AuthenticationStratagies
    {
        List<IAuthenticationProvider> AuthenticationProviders { get; set; }

        public AuthenticationStratagies(dynamic Settings)
        {
            AuthenticationProviders = new List<IAuthenticationProvider>();

            AuthenticationProviders.Add(new AzureAdOAuthProvider(Settings.Azure));
        }

        public IAuthenticationProvider GetAuthenticationStratagyForProvider(string Provider)
        {
            return AuthenticationProviders.Find(a => a.Name.Equals(Provider, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
