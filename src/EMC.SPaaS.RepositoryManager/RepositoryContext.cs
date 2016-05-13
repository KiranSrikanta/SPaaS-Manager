using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;

namespace EMC.SPaaS.RepositoryManager
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext() : base()
        {
           Database.EnsureCreated();
        }
        public DbSet<DesignItem> DesignItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<DesignItem>().Property(u => u.DesignID).IsRequired();

        }



    }
}
