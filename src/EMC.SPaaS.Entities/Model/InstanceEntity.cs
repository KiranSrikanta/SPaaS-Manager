﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EMC.SPaaS.Entities
{
    public class InstanceEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int DesignId { get; set; }

        public string Name { get; set; }

        public int StatusId { get; set; }

        [ForeignKey("StatusId")]
        public InstanceStatus Status { get; set; }

        public ICollection<ProvisionedVmEntity> VMs { get; set; }

        public ICollection<JobEntity> Jobs { get; set; }
    }

    public class InstanceStatus
    {
        [Key]
        public int Id { get; set; }

        public string Status { get; set; }

        public ICollection<InstanceEntity> Instances { get; set; }
    }
}