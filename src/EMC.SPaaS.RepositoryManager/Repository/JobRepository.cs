using EMC.SPaaS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMC.SPaaS.Repository
{
    public class JobRepository : IJobRepository
    {
        SPaaSDbContext Context { get; set; }

        public JobRepository(SPaaSDbContext context)
        {
            Context = context;
        }

        public IEnumerable<JobEntity> GetJobByStatus(JobStatus status)
        {
            return from j in Context.Jobs where j.StatusId == (int)status select j;
        }

        public void UpdateStatus(JobEntity job, JobStatus status)
        {
            job.StatusId = (int)status;
        }

        public void UpdateStatus(int jobId, JobStatus status)
        {
            var job = Context.Jobs.FirstOrDefault(j => j.Id == jobId);
            if (job == null)
                throw new ArgumentOutOfRangeException(nameof(jobId));

            job.StatusId = (int)status;
        }
    }
}
