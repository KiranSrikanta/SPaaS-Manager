using EMC.SPaaS.DesignManager;
using EMC.SPaaS.Entities;

namespace EMC.SPaaS.ProvisioningEngine
{
    public interface IProvisioner
    {
        int CreateInstance(DesignEntity design);

        bool TurnOnInstance(int instanceId);

        bool TurnOffInstance(int instanceId);

        bool DeleteInstance(int instanceId);
    }
}