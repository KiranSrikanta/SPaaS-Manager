using System;
using Quartz;
using Microsoft.Data.Entity.Infrastructure;
using EMC.SPaaS.Entities;
using Microsoft.Data.Entity;
using EMC.SPaaS.ProvisioningEngine;

namespace EMC.SPaaS.JobScheduler
{
    internal class CloudProvisionerJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.MergedJobDataMap;

            var Repositories = CreateRepositoryManagerFromConnectionString(
                dataMap.GetString(Constants.PropertyKeys.ConnectionString)
            );

            var user = Repositories.Users.GetUser(1);
            
            throw new NotImplementedException();
        }

        Repository.RepositoryManager CreateRepositoryManagerFromConnectionString(string connectionString)
        {
            var dbOptions = new DbContextOptionsBuilder<SPaaSDbContext>();

            dbOptions.UseNpgsql(connectionString);

            SPaaSDbContext dbContext = new SPaaSDbContext(dbOptions.Options);

            return new Repository.RepositoryManager(dbContext);
        }
    }
}