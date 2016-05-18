using EMC.SPaaS.DesignManager;
using EMC.SPaaS.Entities;

namespace EMC.SPaaS.ProvisioningEngine
{
    public interface IProvisioner
    {
        void CreateInstance(InstanceEntity instance);

        bool TurnOnInstance(int instanceId);

        bool TurnOffInstance(int instanceId);

        bool DeleteInstance(int instanceId);
    }
}