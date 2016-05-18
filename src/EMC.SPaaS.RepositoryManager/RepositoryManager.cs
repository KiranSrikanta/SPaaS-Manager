using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using EMC.SPaaS.Entities;

namespace EMC.SPaaS.Repository
{
    public class RepositoryManager
    {
        SPaaSDbContext Context { get; set; }

        public RepositoryManager(SPaaSDbContext context)
        {
            Context = context;
        }
        public IDesignRepository Designs
        {
            get
            {
                return new DesignRepository(Context);
            }
        }
        public IUserRepository Users
        {
            get
            {
                return new UserRepository(Context);
            }
        }
        public IInstanceRepository Instances
        {
            get
            {
                return new InstanceRepository(Context);
            }
        }
        public IJobRepository Jobs
        {
            get
            {
                return new JobRepository(Context);
            }
        }
        public IVMDesignRepository VMDesigns
        {
            get
            {
                return new VMDesignRepository(Context);
            }
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }
}
