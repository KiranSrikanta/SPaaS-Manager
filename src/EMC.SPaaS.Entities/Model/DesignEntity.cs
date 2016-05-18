using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMC.SPaaS.Entities
{
    public class DesignEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string DesignXML { get; set; }
        public string DesignOwner { get; set; }
        public string DesignName { get; set; }
    }
}
