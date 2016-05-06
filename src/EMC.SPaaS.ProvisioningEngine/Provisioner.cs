using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMC.SPaaS.CloudProvider;
using EMC.SPaaS.DesignManager;

namespace EMC.SPaaS.ProvisioningEngine
{
    public class Provisioner<T> : IProvisioner where T : ISubscription
    {
        ICloudProvider<T> _cloudProvider;

        public Provisioner(ICloudProvider<T> CloudProvider)
        {
            _cloudProvider = CloudProvider;
        }

        public int CreateInstance(IDesign design)
        {
            foreach(var serverBP in design.ServerBluePrints)
            {
                _cloudProvider.CreateVM(serverBP.Server.Name);

                //TODO:INSTALL CHEF
                var configFile = serverBP.GetXmlConfigurationFile();

                //TODO:RUN CHEF WITH CONFIGURATIO?
            }



            //TODO:SAVE INSTANCE DETAILS FROM DB
            throw new NotImplementedException();
        }

        public bool TurnOnInstance(int instanceId)
        {
            //TODO:GET INSTANCE DETAILS FROM DB
            throw new NotImplementedException();
        }

        public bool TurnOffInstance(int instanceId)
        {
            //TODO:GET INSTANCE DETAILS FROM DB
            throw new NotImplementedException();
        }

        public bool DeleteInstance(int instanceId)
        {
            //TODO:GET INSTANCE DETAILS FROM DB
            throw new NotImplementedException();
        }
    }
}
