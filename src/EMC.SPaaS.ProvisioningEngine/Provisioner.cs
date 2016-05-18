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

        public int CreateInstance(InstanceEntity instance)
        {
            _cloudProvider.Initialize(instance);

            var vm = new VMDesignEntity();
            var provisionedVM = _cloudProvider.CreateVM(vm, instance);
            _repositories.Instances.AddVM(instance, provisionedVM);
            _repositories.Save();

            //foreach(var serverBP in design.ServerBluePrints)
            //{
            //    _cloudProvider.CreateVM(serverBP.Server.Name);

            //    //TODO:INSTALL CHEF
            //    var configFile = serverBP.GetXmlConfigurationFile();

            //    //TODO:RUN CHEF WITH CONFIGURATIO?
            //}



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
