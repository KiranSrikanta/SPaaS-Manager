using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMC.SPaaS.CloudProvider;
using Microsoft.Extensions.Configuration;
using EMC.SPaaS.Utility;
using EMC.SPaaS.Entities;

namespace EMC.SPaaS.ProvisioningEngine
{
    public class ProvisionerFactory
    {
        IConfigurationSection AuthenticationConfiguration { get; set; }
        public ProvisionerFactory(IConfigurationSection authConfiguration)
        {
            AuthenticationConfiguration = authConfiguration;
        }
        public IProvisioner CreateProvisioner(UserEntity User)
        {
            //TODO: IMPLEMENT STRATAGY PATTERN
            if (User.AuthenticationProvider == GlobalConstants.CloudProviders.Azure.Name)
            {
                var azureCloudProvider = new Azure(AuthenticationConfiguration.GetSection(GlobalConstants.CloudProviders.Azure.Name), User.AccessToken);

                azureCloudProvider.CreateVM("AAA");

                return new Provisioner(azureCloudProvider);
            }
            else
                return null;
        }


    }
}
