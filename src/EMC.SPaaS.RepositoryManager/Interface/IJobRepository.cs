using System.Collections.Generic;
using EMC.SPaaS.Entities;

namespace EMC.SPaaS.Repository
{
    public interface IJobRepository
    {
        IEnumerable<JobEntity> GetJobByStatus(JobStatus status);
        void UpdateStatus(JobEntity job, JobStatus status);
        void UpdateStatus(int jobId, JobStatus status);
    }
}