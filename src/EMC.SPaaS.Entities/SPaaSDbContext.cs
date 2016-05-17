using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMC.SPaaS.Entities
{
    public class SPaaSDbContext : DbContext
    {
        public SPaaSDbContext() : base()
        {
            Database.EnsureCreated();
            
            Database.Migrate();
        }

        public SPaaSDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();

            Database.Migrate();
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>().Property(u => u.UserId).IsRequired();
            modelBuilder.Entity<UserEntity>().Property(u => u.UserName).IsRequired();
        }

        public DbSet<UserEntity> Users { get; set; }

        public DbSet<DesignEntity> Designs { get; set; }

        public DbSet<InstanceEntity> Instances { get; set; }
        public DbSet<InstanceStatus> InstanceStatuses { get; set; }

        public DbSet<ProvisionedVmEntity> VMs { get; set; }
        public DbSet<ProvisionedVmStatus> VMsStatuses { get; set; }

        public DbSet<JobEntity> Jobs { get; set; }
        public DbSet<JobType> JobTypes { get; set; }
        public DbSet<JobStatus> JobStatuses { get; set; }
    }
}
