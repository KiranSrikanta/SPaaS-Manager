using EMC.SPaaS.Entities;
using Microsoft.Data.Entity;
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
            var jobs = Context.Jobs.Where(j => j.StatusId == (int)status)
                .Include(j => j.User)
                .Include(j => j.Instance)
                .Include(j => j.Instance.Design)
                .Include(j => j.Instance.VMs)
                .Include(j => j.Instance.Design.VMs);
            return jobs;
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