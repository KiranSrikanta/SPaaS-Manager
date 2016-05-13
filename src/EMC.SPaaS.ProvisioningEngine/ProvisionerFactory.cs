using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMC.SPaaS.CloudProvider;
using Microsoft.Extensions.Configuration;
using EMC.SPaaS.Utility;

namespace EMC.SPaaS.ProvisioningEngine
{
    public class ProvisionerFactory
    {
        IConfigurationSection AuthenticationConfiguration { get; set; }
        public ProvisionerFactory(IConfigurationSection authConfiguration)
        {
            AuthenticationConfiguration = authConfiguration;
        }
        public IProvisioner CreateProvisioner(System.Security.Claims.ClaimsPrincipal User)
        {
            var uIde = User.Identities.FirstOrDefault(i => i.AuthenticationType == GlobalConstants.CloudProviders.Azure.Name);
            //TODO: IMPLEMENT STRATAGY PATTERN
            if (User.Identity.AuthenticationType == GlobalConstants.CloudProviders.Azure.Name)
            {
                return new Provisioner(new Azure(AuthenticationConfiguration.GetSection(GlobalConstants.CloudProviders.Azure.Name), ""));
            }
            else
                return null;
        }


    }
}
