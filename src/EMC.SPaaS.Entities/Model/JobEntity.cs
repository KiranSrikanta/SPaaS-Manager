using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EMC.SPaaS.Entities
{
    public class JobEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public UserEntity User { get; set; }

        public int StatusId { get; set; }

        [ForeignKey("StatusId")]
        public JobStatus Status { get; set; }

        public int TypeId { get; set; }

        [ForeignKey("TypeId")]
        public JobTypeEntity Type { get; set; }

        [ForeignKey("InstanceId")]
        public InstanceEntity Instance { get; set; }

        public int InstanceId { get; set; }
    }

    public enum JobStatus
    {
        NotStarted = 1,
        InProgress,
        Successful,
        Failed
    }

    public class JobStatusEntity
    {
        [Key]
        public int Id { get; set; }

        public string Status { get; set; }

        public ICollection<JobEntity> Jobs { get; set; }
    }

    public enum JobType
    {
        Provision = 1,
        Release,
        TurnOn,
        TurnOff
    }

    public class JobTypeEntity
    {
        [Key]
        public int Id { get; set; }

        public string Type { get; set; }

        public ICollection<JobEntity> Jobs { get; set; }
    }
}
