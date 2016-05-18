using Microsoft.Data.Entity;
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


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>().Property(u => u.UserId).IsRequired();
            modelBuilder.Entity<UserEntity>().Property(u => u.UserName).IsRequired();
        }

        public DbSet<UserEntity> Users { get; set; }
        
        public DbSet<DesignEntity> Designs { get; set; }

        public DbSet<VMDesignEntity> VMDesigns { get; set; }
    }
}
