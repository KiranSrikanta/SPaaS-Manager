using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMC.SPaaS.Entities
{
    public class XMLDesignData
    {
        public string SPVersion { get; set; }
        public string PassPhrase { get; set; }
        public string FarmAccountUsername { get; set; }
        public string FarmAccountPassword { get; set; }
        public string FarmDBServerName { get; set; }
        public string Service { get; set; }
    }
}
