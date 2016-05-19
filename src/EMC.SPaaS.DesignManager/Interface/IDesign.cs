using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMC.SPaaS.DesignManager
{
    public interface IDesign
    {
        IEnumerable<IServerBluePrint> ServerBluePrints { get; }
    }

    public class Server
    {
        public Server(string Name, string RAM, string Processors)
        {
            this.Name = Name;
            this.RAM = RAM;
            this.Processors = Processors;
        }
        public string Name { get; protected set; }

        public string RAM { get; protected set; }

        public string Processors { get; protected set; }
    }

    public interface IServerBluePrint
    {
        Server Server { get; }

        System.Xml.XmlDocument GetXmlConfigurationFile();
    }
}
