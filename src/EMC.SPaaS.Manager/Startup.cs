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
            services.AddEntityFramework().AddNpgsql().AddDbContext<SPaaSDbContext>(options => {
                options.UseNpgsql(connectionString);
            });
            
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
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
