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

    public interface IServer
    {
        string Name { get; set; }
        
        string RAM { get; }

        string Processors { get; }
    }

    public interface IServerBluePrint
    {
        IServer Server { get; }

        System.Xml.XmlDocument GetXmlConfigurationFile();
    }
}
