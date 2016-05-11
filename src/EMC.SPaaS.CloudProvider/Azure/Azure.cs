using EMC.SPaaS.CloudProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMC.SPaaS.DesignManager;

namespace EMC.SPaaS.CloudProvider
{
    public class Azure : ICloudProvider<AzureSubscription>
    {
        public Azure(AzureSubscription Subscription)
        {
            _subscription = Subscription;
        }

        private AzureSubscription _subscription;

        public AzureSubscription Subscription
        {
            get
            {
                return _subscription;
            }
        }

        public string CreateVM(string Name)
        {
            throw new NotImplementedException();
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
