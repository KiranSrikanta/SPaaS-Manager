using EMC.SPaaS.CloudProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMC.SPaaS.DesignManager;
using Microsoft.WindowsAzure.Management.Compute;
using Microsoft.WindowsAzure;
using EMC.SPaaS.AuthenticationProviders;
using Microsoft.Extensions.Configuration;
using EMC.SPaaS.Utility;

namespace EMC.SPaaS.CloudProvider
{
    public class Azure : ICloudProvider
    {
        SubscriptionCloudCredentials Credentials { get; set; }

        IAuthenticationProvider OAuthProvider { get; set; }

        public Azure(IConfigurationSection configuration, string token)
        {
            OAuthProvider = new AzureAdOAuthProvider(configuration);

            Credentials = new TokenCloudCredentials(
                configuration[GlobalConstants.CloudProviders.Azure.ConfigurationKeys.SubscriptionId],
                OAuthProvider.GetApiAccessToken(token)
            );
        }

        public string CreateVM(string Name)
        {
            ComputeManagementClient client = new ComputeManagementClient(Credentials);

            var result = client.VirtualMachines.BeginCreating("aaa", "bbb", new Microsoft.WindowsAzure.Management.Compute.Models.VirtualMachineCreateParameters
            {
                RoleName = "ccc"
            });

            return result.RequestId;
        }

        public bool DeleteVM(string id)
        {
            throw new NotImplementedException();
        }

        public bool TurnOffVM(string id)
        {
            throw new NotImplementedException();
        }

        public bool TurnOnVM(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IServer> GetAvailableVMOptions()
        {
            List<IServer> serverOptionsList = new List<IServer>(3);

            serverOptionsList.Add(new AzureServer() { Name = "A1", Processors = 4, RAM = 7 });
            serverOptionsList.Add(new AzureServer() { Name = "A2", Processors = 2, RAM = 3 });

            return serverOptionsList.ToArray();
        }

        class AzureServer : IServer
        {
            public string Name
            {
                get; set;
            }

            public int Processors
            {
                get; set;
            }

            public int RAM
            {
                get; set;
            }
        }
    }
}
