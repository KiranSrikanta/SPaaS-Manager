using System.Collections.Generic;
using EMC.SPaaS.DesignManager;
using EMC.SPaaS.Entities;

namespace EMC.SPaaS.ProvisioningEngine
{
    public interface IProvisioner
    {
        void CreateInstance(InstanceEntity instance);

        void CreateInstanceVMs(InstanceEntity instance);

        bool TurnOnInstanceVMs(int instanceId);

        bool TurnOffInstanceVMs(int instanceId);

        bool DeleteInstanceVMs(int instanceId);

        bool UpdateDetailsIfInstanceRunning(InstanceEntity instance);

        bool IsInstanceOff(InstanceEntity instance);

        IEnumerable<Server> VMOptions();

        IEnumerable<string> OSImageOptions();
    }
}