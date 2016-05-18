using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EMC.SPaaS.Entities
{
    public class VMDesignEntity
    {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int DId { get; set; }
        [ForeignKey("DId")]
        public DesignEntity Design { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public UserEntity User { get; set; }
        public string Name { get; set; }
        public string OS { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Type { get; set; }
        public string DesignXML { get; set; }
    }
}
