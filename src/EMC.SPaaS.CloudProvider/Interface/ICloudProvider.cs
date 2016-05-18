using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMC.SPaaS.DesignManager;
using EMC.SPaaS.Entities;

namespace EMC.SPaaS.CloudProvider
{
    public interface ICloudProvider
    {
        void Initialize(InstanceEntity instance);

        ProvisionedVmEntity CreateVM(VMDesignEntity vmDesign, InstanceEntity instance);

        bool DeleteVM(ProvisionedVmEntity vm);

        bool TurnOnVM(ProvisionedVmEntity vm);

        bool TurnOffVM(ProvisionedVmEntity vm);
    }
}
