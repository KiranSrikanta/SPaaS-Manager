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
using EMC.SPaaS.Entities;

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

        public ProvisionedVmEntity CreateVM(VMDesignEntity vmDesign, InstanceEntity instance)
        {
            //Configurable
            string storageAccountName = "spaasstorage";

            string hostedService = instance.Name;//"spaashost";
            string hostedServiceLabel = instance.Name;//"spaas";

            string vmName = vmDesign.Name;//"vm1";
            string vmUserName = vmDesign.UserName;//"SPaaSAdmin";
            string vmPassword = vmDesign.Password;//"Welcome@123";
            string vmSize = vmDesign.Type;

            string deploymentName = instance.Name + vmDesign.Name;// "testdeploy";

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
                    //Configurable??
                    var imageName = operatingSystemImageListResult.FirstOrDefault(os => os.Label.Contains(vmDesign.Name)).Name;

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
                        RoleSize = vmSize,//VirtualMachineRoleSize.Small,
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

                    return new ProvisionedVmEntity {
                        Instance = instance,
                        Name = deploymentName,
                        StatusId = (int)ProvisionedVmStatus.Busy,
                        VmId = deploymentResult.RequestId
                    };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            throw new NotImplementedException();
        }

        public bool DeleteVM(ProvisionedVmEntity vm)
        {
            throw new NotImplementedException();
        }

        public bool TurnOnVM(ProvisionedVmEntity vm)
        {
            throw new NotImplementedException();
        }

        public bool TurnOffVM(ProvisionedVmEntity vm)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IServer> GetAvailableVMOptions()
        {
            List<IServer> serverOptionsList = new List<IServer>(3);

            //Not Real
            serverOptionsList.Add(new AzureServer() { Name = VirtualMachineRoleSize.ExtraSmall, Processors = "shared core", RAM = "768 MB" });
            serverOptionsList.Add(new AzureServer() { Name = VirtualMachineRoleSize.Small, Processors = "1 core", RAM = "1.75 GB" });
            serverOptionsList.Add(new AzureServer() { Name = VirtualMachineRoleSize.Medium, Processors = "2 cores", RAM = "3.5 GB" });
            serverOptionsList.Add(new AzureServer() { Name = VirtualMachineRoleSize.Large, Processors = "4 cores", RAM = "7 GB" });
            serverOptionsList.Add(new AzureServer() { Name = VirtualMachineRoleSize.ExtraLarge, Processors = "8 cores", RAM = "14 GB" });
            serverOptionsList.Add(new AzureServer() { Name = VirtualMachineRoleSize.A5, Processors = "2 cores", RAM = "14 GB" });
            serverOptionsList.Add(new AzureServer() { Name = VirtualMachineRoleSize.A6, Processors = "4 cores", RAM = "28 GB" });
            serverOptionsList.Add(new AzureServer() { Name = VirtualMachineRoleSize.A7, Processors = "8 cores", RAM = "56 GB" });
            //serverOptionsList.Add(new AzureServer() { Name = VirtualMachineRoleSize.A8, Processors = 2, RAM = 3 });
            //serverOptionsList.Add(new AzureServer() { Name = VirtualMachineRoleSize.A9, Processors = 2, RAM = 3 });

            return serverOptionsList.ToArray();
        }

        public void Initialize(InstanceEntity instance)
        {
            string hostedService = instance.Name;//"spaashost";
            string hostedServiceLabel = instance.Name;//"spaas";

            //create cloud service
            using (var computeClient = new ComputeManagementClient(Credentials))
            {
                computeClient.HostedServices.Create(new HostedServiceCreateParameters
                {
                    Label = hostedServiceLabel,
                    Location = LocationNames.WestUS,
                    ServiceName = hostedService
                });
            }
        }

        class AzureServer : IServer
        {
            public string Name
            {
                get; set;
            }

            public string Processors
            {
                get; set;
            }

            public string RAM
            {
                get; set;
            }
        }
    }
}
