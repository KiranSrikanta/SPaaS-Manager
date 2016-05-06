using EMC.SPaaS.DesignManager;

namespace EMC.SPaaS.ProvisioningEngine
{
    public interface IProvisioner
    {
        int CreateInstance(IDesign design);

        bool TurnOnInstance(int instanceId);

        bool TurnOffInstance(int instanceId);

        bool DeleteInstance(int instanceId);
    }
}