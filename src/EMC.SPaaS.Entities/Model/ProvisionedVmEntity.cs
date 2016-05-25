using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EMC.SPaaS.Entities
{
    public class ProvisionedVmEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string VmId { get; set; }

        [ForeignKey("InstanceId")]
        public InstanceEntity Instance { get; set; }

        public int InstanceId { get; set; }

        public string IP { get; set; }

        public string Name { get; set; }

        public int StatusId { get; set; }

        [ForeignKey("StatusId")]
        public ProvisionedVmStatusEntity Status { get; set; }
    }

    public class ProvisionedVmStatusEntity
    {
        [Key]
        public int Id { get; set; }

        public string Status { get; set; }

        public ICollection<ProvisionedVmEntity> VMs { get; set; }
    }

    public enum ProvisionedVmStatus
    {
        NotProvisioned = 1,
        TurnedOn,
        TurnedOff,
        Busy
    }
}
