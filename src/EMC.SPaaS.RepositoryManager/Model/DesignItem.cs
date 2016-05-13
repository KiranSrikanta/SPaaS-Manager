using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EMC.SPaaS.RepositoryManager
{
    public class DesignItem
    {
        [Key]
        public string DesignID { get; set; }
        public string DesignXML { get; set; }
        public string DesignOwner { get; set; }
    }
}
