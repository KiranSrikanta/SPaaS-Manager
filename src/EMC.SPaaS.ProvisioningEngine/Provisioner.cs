using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMC.SPaaS.CloudProvider;
using EMC.SPaaS.DesignManager;
using EMC.SPaaS.Entities;
using EMC.SPaaS.Repository;

namespace EMC.SPaaS.ProvisioningEngine
{
    public class Provisioner : IProvisioner
    {
        ICloudProvider _cloudProvider;
        RepositoryManager _repositories;

        public Provisioner(ICloudProvider CloudProvider, RepositoryManager repositories)
        {
            _cloudProvider = CloudProvider;
            _repositories = repositories;
        }

        public void CreateInstanceVMs(InstanceEntity instance)
        {
            _cloudProvider.Initialize(instance);
            _cloudProvider.CreateVM(instance);
            foreach (var vm in instance.Design.VMs)
            {
                _repositories.Instances.AddVM(instance, new ProvisionedVmEntity {
                    Name = vm.Name,
                    StatusId = (int)ProvisionedVmStatus.Busy
                });
            }
            _repositories.Save();
        }

        public bool TurnOnInstanceVMs(int instanceId)
        {
            //TODO:GET INSTANCE DETAILS FROM DB
            throw new NotImplementedException();
        }

        public bool TurnOffInstanceVMs(int instanceId)
        {
            //TODO:GET INSTANCE DETAILS FROM DB
            throw new NotImplementedException();
        }

        public bool DeleteInstanceVMs(int instanceId)
        {
            //TODO:GET INSTANCE DETAILS FROM DB
            throw new NotImplementedException();
        }

        public void CreateInstance(InstanceEntity instance)
        {
            CreateInstanceVMs(instance);
        }

        public bool IsInstanceRunning(InstanceEntity instance)
        {
            return _cloudProvider.IsDeployedInstanceRunning(instance);
        }

        public bool IsInstanceOff(InstanceEntity instance)
        {
            return _cloudProvider.IsDeployedInstanceOff(instance);
        }

        public IEnumerable<ProvisionedVmEntity> GetProvisionedVMDetails(InstanceEntity instance)
        {
            return _cloudProvider.GetVMDetails(instance);
        }
    }
}
