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

        public Provisioner(ICloudProvider CloudProvider)
        {
            _cloudProvider = CloudProvider;
        }

        public void CreateInstanceVMs(InstanceEntity instance)
        {
            _cloudProvider.Initialize(instance);
            _cloudProvider.CreateVM(instance);
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

        public bool UpdateDetailsIfInstanceRunning(InstanceEntity instance)
        {
            return _cloudProvider.UpdateVMDetailsIfInstanceRunning(instance);
        }

        public bool IsInstanceOff(InstanceEntity instance)
        {
            return _cloudProvider.IsDeployedInstanceOff(instance);
        }

        public void UpdateProvisionedVMDetails(InstanceEntity instance)
        {
            _cloudProvider.UpdateVMDetailsIfInstanceRunning(instance);
        }
    }
}
