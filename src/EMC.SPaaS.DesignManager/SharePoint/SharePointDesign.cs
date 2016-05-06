using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace EMC.SPaaS.DesignManager
{
    public class SharePointDesign : IDesign
    {
        IEnumerable<SharePointService> sharePointServices { get; set; }

        private List<IServerBluePrint> sharePointServerBluePrints;

        public IEnumerable<IServerBluePrint> ServerBluePrints
        {
            get
            {
                return sharePointServerBluePrints.AsEnumerable<IServerBluePrint>();
            }
        }

        public SharePointDesign()
        {
            sharePointServerBluePrints = new List<IServerBluePrint>();

            var spServices = new List<SharePointService>();
            spServices.Add(new SharePointService() { Name = "WFE" });
            spServices.Add(new SharePointService() { Name = "DB" });
            sharePointServices = spServices.AsEnumerable();

        }

        public IEnumerable<string> GetAllSharePointServices()
        {
            return sharePointServices.OrderBy(s => s.Name).Select(s => s.Name);
        }
    }

    public class SharePointServerBluePrint : IServerBluePrint
    {
        public IServer Server
        {
            get; private set;
        }

        internal List<SharePointService> Services { get; private set; }

        public SharePointServerBluePrint()
        {
            Services = new List<SharePointService>();
        }

        public XmlDocument GetXmlConfigurationFile()
        {
            throw new NotImplementedException();
        }
    }

    class SharePointService
    {
        public string Name { get; internal set; }
    }
}
