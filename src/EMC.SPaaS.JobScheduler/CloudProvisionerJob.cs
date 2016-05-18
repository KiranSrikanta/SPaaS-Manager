using System;
using Quartz;
using Microsoft.Data.Entity.Infrastructure;
using EMC.SPaaS.Entities;
using Microsoft.Data.Entity;
using EMC.SPaaS.ProvisioningEngine;
using Microsoft.Extensions.Configuration;

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

            var jobs = Repositories.Jobs.GetJobByStatus(JobStatus.NotStarted);

            var rootConfig = GetConfiguration();
            var provisionerFactory = new ProvisionerFactory(rootConfig.GetSection("Authentication"));
            foreach (var job in jobs)
            {
                var provisioner = provisionerFactory.CreateProvisioner(job.User);

                Repositories.Jobs.UpdateStatus(job, JobStatus.InProgress);
                Repositories.Save();

                switch ((JobType)job.TypeId)
                {
                    case JobType.Provision:
                        provisioner.CreateInstance(job.Instance.Design);
                        break;
                    case JobType.Release:
                        provisioner.DeleteInstance(job.InstanceId);
                        break;
                    case JobType.TurnOff:
                        provisioner.TurnOffInstance(job.InstanceId);
                        break;
                    case JobType.TurnOn:
                        provisioner.TurnOnInstance(job.InstanceId);
                        break;
                }
            }
        }

        Repository.RepositoryManager CreateRepositoryManagerFromConnectionString(string connectionString)
        {
            var dbOptions = new DbContextOptionsBuilder<SPaaSDbContext>();

            dbOptions.UseNpgsql(connectionString);

            SPaaSDbContext dbContext = new SPaaSDbContext(dbOptions.Options);

            return new Repository.RepositoryManager(dbContext);
        }

        IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            return builder.Build().ReloadOnChanged("appsettings.json");
        }
    }
}