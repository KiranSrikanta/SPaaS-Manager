using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMC.SPaaS.DesignManager;

namespace EMC.SPaaS.CloudProvider
{
    public interface ICloudProvider<T> where T : ISubscription
    {
        T Subscription { get; }

        string CreateVM(string Name);

        bool DeleteVM(string id);

        bool TurnOnVM(string id);

        bool TurnOffVM(string id);
    }
}
