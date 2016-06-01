using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EMC.SPaaS.Entities
{
    public class SolnEnvironmentEntity
    {
        [Key]
        public int EnvID { get; set; }
         public string Name { get; set; }
         public string Versions { get; set; }
    }
}
