using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMC.SPaaS.CloudProvider;

namespace EMC.SPaaS.ProvisioningEngine
{
    public class ProvisionerFactory
    {
        public IProvisioner CreateProvisioner(string UserRole)
        {
            //TODO: IMPLEMENT STRATAGY PATTERN
            if (UserRole == "Azure")
            {
                return new Provisioner<AzureSubscription>(new Azure(new AzureSubscription()));
            }
            else
                return null;
        }


    }
}
