using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml;
using System.Collections.Generic;
namespace EMC.SPaaS.Entities
{
    public class DesignEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DId { get; set; }
        public string DesignXML { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public UserEntity User { get; set; }
        public string DesignName { get; set; }

        
    }
}
