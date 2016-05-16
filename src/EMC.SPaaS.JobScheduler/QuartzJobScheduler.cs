using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMC.SPaaS.JobScheduler
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class QuartzJobScheduler
    {
        public QuartzJobScheduler(string PostgressConnectionString)
        {
            ISchedulerFactory schedFact = new StdSchedulerFactory();

            // get a scheduler
            IScheduler sched = schedFact.GetScheduler();
            sched.Start();

            // define the job and tie it to our HelloJob class
            IJobDetail job = JobBuilder.Create<CloudProvisionerJob>()
                //.WithIdentity("myJob", "group1")
                .UsingJobData(Constants.PropertyKeys.ConnectionString, PostgressConnectionString)
                //.UsingJobData("myFloatValue", 3.141f)
                .Build();

            // Trigger the job to run now, and then every 40 seconds
            ITrigger trigger = TriggerBuilder.Create()
              //.WithIdentity("myTrigger", "group1")
              .StartNow()
              .WithSimpleSchedule(x => x
                  .WithIntervalInMinutes(5)
                  .RepeatForever())
              .Build();

            sched.ScheduleJob(job, trigger);
        }
    }
}
