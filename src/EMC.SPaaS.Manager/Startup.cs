using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Data.Entity;
using EMC.SPaaS.Entities;
using EMC.SPaaS.AuthenticationProviders;
using EMC.SPaaS.JobScheduler;

namespace EMC.SPaaS.Manager
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build().ReloadOnChanged("appsettings.json");
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddOptions();

            services.Configure<WebAppConfigurations>(Configuration.GetSection("WebApp"));

            services.AddScoped<ProvisioningEngine.ProvisionerFactory>((IServiceProvider s) => {
                return new ProvisioningEngine.ProvisionerFactory(Configuration.GetSection("Authentication"));
            });

            services.AddScoped<AuthenticationStratagies>((IServiceProvider s) => {
                return new AuthenticationStratagies(Configuration.GetSection("Authentication"));
            });

            var dataConfigSection = Configuration.GetSection("Data");
            var defaultConnection = dataConfigSection.GetSection("DefaultConnection");
            var connectionString = defaultConnection["ConnectionString"];
            services.AddEntityFramework().AddNpgsql().AddDbContext<SPaaSDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });

            QuartzJobScheduler jobScheduler = new QuartzJobScheduler(connectionString);

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseIISPlatformHandler();

            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseDefaultFiles();

            app.UseStaticFiles();

            app.UseJsonWebTokenAuthorization();

            app.UseMvc();

            using (var context = (SPaaSDbContext)app.ApplicationServices.GetService<SPaaSDbContext>())
            {
                if (env.IsDevelopment())
                {
                    #region Job Status
                    if (context.JobStatuses.Count() == 0)
                    {
                        context.JobStatuses.Add(new JobStatusEntity
                        {
                            Id = (int)JobStatus.NotStarted,
                            Status = "Not Started"
                        });
                        context.JobStatuses.Add(new JobStatusEntity
                        {
                            Id = (int)JobStatus.InProgress,
                            Status = "In Progress"
                        });
                        context.JobStatuses.Add(new JobStatusEntity
                        {
                            Id = (int)JobStatus.Successful,
                            Status = "Successful"
                        });
                        context.JobStatuses.Add(new JobStatusEntity
                        {
                            Id = (int)JobStatus.Failed,
                            Status = "Failed"
                        });

                        context.SaveChanges();
                    }
                    #endregion

                    #region Instance Status
                    if (context.InstanceStatuses.Count() == 0)
                    {
                        context.InstanceStatuses.Add(new InstanceStatusEntity
                        {
                            Id = (int)InstanceStatus.NotProvisioned,
                            Status = "Not Provisioned"
                        });
                        context.InstanceStatuses.Add(new InstanceStatusEntity
                        {
                            Id = (int)InstanceStatus.Busy,
                            Status = "Busy"
                        });
                        context.InstanceStatuses.Add(new InstanceStatusEntity
                        {
                            Id = (int)InstanceStatus.TurnedOn,
                            Status = "Turned On"
                        });
                        context.InstanceStatuses.Add(new InstanceStatusEntity
                        {
                            Id = (int)InstanceStatus.TurnedOff,
                            Status = "Turned Off"
                        });

                        context.SaveChanges();
                    }
                    #endregion

                    #region Job Type
                    if (context.JobTypes.Count() == 0)
                    {
                        context.JobTypes.Add(new JobTypeEntity
                        {
                            Id = (int)JobType.Provision,
                            Type = "Provision"
                        });
                        context.JobTypes.Add(new JobTypeEntity
                        {
                            Id = (int)JobType.Release,
                            Type = "Release"
                        });
                        context.JobTypes.Add(new JobTypeEntity
                        {
                            Id = (int)JobType.TurnOn,
                            Type = "Turn On"
                        });
                        context.JobTypes.Add(new JobTypeEntity
                        {
                            Id = (int)JobType.TurnOff,
                            Type = "Turn Off"
                        });

                        context.SaveChanges();
                    }
                    #endregion
                }
            }
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
