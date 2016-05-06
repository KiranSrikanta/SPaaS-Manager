using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace EMC.SPaaS.DesignManager
{
    public interface IDesigner<T> where T : IDesign
    {
        T Design { get; }
    }
}
