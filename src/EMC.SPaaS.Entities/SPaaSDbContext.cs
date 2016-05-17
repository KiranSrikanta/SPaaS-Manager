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
        public DbSet<InstanceStatusEntity> InstanceStatuses { get; set; }

        public DbSet<ProvisionedVmEntity> VMs { get; set; }
        public DbSet<ProvisionedVmStatusEntity> VMsStatuses { get; set; }

        public DbSet<JobEntity> Jobs { get; set; }
        public DbSet<JobTypeEntity> JobTypes { get; set; }
        public DbSet<JobStatusEntity> JobStatuses { get; set; }
    }
}
