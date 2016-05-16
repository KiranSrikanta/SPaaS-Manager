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
using Microsoft.WindowsAzure.Management.Compute.Models;
using Microsoft.WindowsAzure.Management.Storage;
using Microsoft.WindowsAzure.Management.Storage.Models;
using Microsoft.WindowsAzure.Management.Models;

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
            string storageAccountName = "spaasstorage";
            

            string hostedService = "spaashost";
            string hostedServiceLabel = "spaas";

            string vmName = "vm1";
            string vmUserName = "SPaaSAdmin";
            string vmPassword = "Welcome@123";

            string deploymentName = "testdeploy";


            try
            {
                //DONE Works!
                //storage account
                //using (var storageClient = new StorageManagementClient(Credentials))
                //{
                //    storageClient.StorageAccounts.Create(new StorageAccountCreateParameters
                //    {
                //        Label = "SPaaS Storage Account",
                //        Location = LocationNames.WestUS,
                //        Name = storageAccountName,
                //        AccountType = StorageAccountTypes.StandardLRS
                //    });
                //}


                //DONE Works!
                //create cloud service
                //using (var computeClient = new ComputeManagementClient(Credentials))
                //{
                //    computeClient.HostedServices.Create(new HostedServiceCreateParameters
                //    {
                //        Label = hostedServiceLabel,
                //        Location = LocationNames.WestUS,
                //        ServiceName = hostedService
                //    });
                //}


                //create vm
                using (var computeClient = new ComputeManagementClient(Credentials))
                {
                    var operatingSystemImageListResult = computeClient.VirtualMachineOSImages.List().Images;
                    var imageName = operatingSystemImageListResult.FirstOrDefault().Name;

                    //OS config
                    var windowsConfigSet = new ConfigurationSet
                    {
                        ConfigurationSetType = ConfigurationSetTypes.WindowsProvisioningConfiguration,
                        AdminPassword = vmPassword,
                        AdminUserName = vmUserName,
                        ComputerName = vmName,
                        HostName = string.Format("{0}.cloudapp.net", hostedService)
                    };

                    //remote powershell and rdp
                    var networkConfigSet = new ConfigurationSet
                    {
                        ConfigurationSetType = "NetworkConfiguration",
                        InputEndpoints = new List<InputEndpoint>
                          {
                            new InputEndpoint
                            {
                              Name = "PowerShell",
                              LocalPort = 5986,
                              Protocol = "tcp",
                              Port = 5986,
                            },
                            new InputEndpoint
                            {
                              Name = "Remote Desktop",
                              LocalPort = 3389,
                              Protocol = "tcp",
                              Port = 3389,
                            }
                          }
                    };

                    //virtual harddisk
                    var vhd = new OSVirtualHardDisk
                    {
                        SourceImageName = imageName,
                        HostCaching = VirtualHardDiskHostCaching.ReadWrite,
                        MediaLink = new Uri(string.Format("https://{0}.blob.core.windows.net/vhds/{1}.vhd", storageAccountName, imageName))
                    };

                    //vm configuration
                    var vmAttributes = new Role
                    {
                        RoleName = vmName,

                        //Make configurable
                        RoleSize = VirtualMachineRoleSize.Small,
                        RoleType = VirtualMachineRoleType.PersistentVMRole.ToString(),
                        OSVirtualHardDisk = vhd,
                        ConfigurationSets = new List<ConfigurationSet> { windowsConfigSet, networkConfigSet },

                        //Optional?
                        ProvisionGuestAgent = true
                    };

                    //deployment config
                    var deploymentParameters = new VirtualMachineCreateDeploymentParameters
                    {
                        Name = deploymentName,
                        Label = deploymentName,
                        DeploymentSlot = DeploymentSlot.Production,
                        Roles = new List<Role> { vmAttributes }
                    };

                    //create the vm
                    var deploymentResult = computeClient.VirtualMachines.CreateDeployment(hostedService, deploymentParameters);
                    
                    computeClient.VirtualMachines.Get("", "", "");
                    computeClient.Deployments.GetByName("", "");

                    return deploymentResult.RequestId;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

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
