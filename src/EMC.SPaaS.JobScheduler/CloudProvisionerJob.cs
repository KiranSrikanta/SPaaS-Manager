﻿using System;
using System.Collections.Generic;
using Quartz;
using Microsoft.Data.Entity.Infrastructure;
using EMC.SPaaS.Entities;
using Microsoft.Data.Entity;
using EMC.SPaaS.ProvisioningEngine;
using Microsoft.Extensions.Configuration;
using System.Threading;

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

            var rootConfig = GetConfiguration();
            var provisionerFactory = new ProvisionerFactory(rootConfig.GetSection("Authentication"));

            #region New Jobs
            var newJobs = Repositories.Jobs.GetJobByStatus(JobStatus.NotStarted);

            foreach (var job in newJobs)
            {
                var provisioner = provisionerFactory.CreateProvisioner(job.User, Repositories);

                Repositories.Jobs.UpdateStatus(job, JobStatus.InProgress);
                Repositories.Save();

                switch ((JobType)job.TypeId)
                {
                    case JobType.Provision:
                        provisioner.CreateInstanceVMs(job.Instance);
                        job.Instance.StatusId = (int)InstanceStatus.Provisioning;
                        Repositories.Save();
                        break;
                    case JobType.Release:
                        provisioner.DeleteInstanceVMs(job.InstanceId);
                        job.Instance.StatusId = (int)InstanceStatus.Busy;
                        Repositories.Save();
                        break;
                    case JobType.TurnOff:
                        provisioner.TurnOffInstanceVMs(job.InstanceId);
                        job.Instance.StatusId = (int)InstanceStatus.Busy;
                        Repositories.Save();
                        break;
                    case JobType.TurnOn:
                        provisioner.TurnOnInstanceVMs(job.InstanceId);
                        job.Instance.StatusId = (int)InstanceStatus.Busy;
                        Repositories.Save();
                        break;
                }
            }
            #endregion

            #region In Progress
            var jobsInProgress = Repositories.Jobs.GetJobByStatus(JobStatus.InProgress);
            foreach (var job in jobsInProgress)
            {
                try
                {
                    #region Provisioning
                    if (job.TypeId == (int)JobType.Provision)
                    {
                        switch ((InstanceStatus)job.Instance.StatusId)
                        {
                            case InstanceStatus.Provisioning:
                                var provisioner = provisionerFactory.CreateProvisioner(job.User, Repositories);
                                if (provisioner.IsInstanceRunning(job.Instance))
                                {
                                    IEnumerable<ProvisionedVmEntity> VMs = provisioner.GetProvisionedVMDetails(job.Instance);
                                    Repositories.Instances.AddVM(job.Instance, VMs);
                                    Repositories.Save();
                                    //TODO:INSTALL CHEF NODE
                                    job.Instance.StatusId = (int)InstanceStatus.ChefNodeInstallation;
                                    Repositories.Save();
                                }
                                break;
                            case InstanceStatus.ChefNodeInstallation:
                                //TODO:CHECK CHEF NODE INSTALLATION
                                if (false)
                                {
                                    //TODO:START INSTALLATION
                                    job.Instance.StatusId = (int)InstanceStatus.SolutionInstallation;
                                    Repositories.Save();
                                }
                                break;
                            case InstanceStatus.SolutionInstallation:
                                //TODO:CHECK IF INSTALLATION COMPLETE
                                if (false)
                                {
                                    job.Instance.StatusId = (int)InstanceStatus.TurnedOn;
                                    job.StatusId = (int)JobStatus.Successful;
                                    Repositories.Save();
                                }
                                break;
                        }
                    }
                    #endregion

                    #region TurnOn
                    if (job.TypeId == (int)JobType.TurnOn)
                    {
                        var provisioner = provisionerFactory.CreateProvisioner(job.User, Repositories);
                        if (provisioner.IsInstanceRunning(job.Instance))
                        {
                            job.Instance.StatusId = (int)InstanceStatus.TurnedOn;
                            job.StatusId = (int)JobStatus.Successful;
                            Repositories.Save();
                        }
                    }
                    #endregion

                    #region TurnOff
                    if (job.TypeId == (int)JobType.TurnOff)
                    {
                        var provisioner = provisionerFactory.CreateProvisioner(job.User, Repositories);
                        if (provisioner.IsInstanceOff(job.Instance))
                        {
                            job.Instance.StatusId = (int)InstanceStatus.TurnedOff;
                            job.StatusId = (int)JobStatus.Successful;
                            Repositories.Save();
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    job.ErrorDetails = string.Format("Error: {0}" + Environment.NewLine + "Stack Trace: {1}", ex.Message, ex.StackTrace);
                    job.StatusId = (int)JobStatus.Failed;
                    Repositories.Save();
                }
            }
            #endregion
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

            return builder.Build();
        }
    }
}