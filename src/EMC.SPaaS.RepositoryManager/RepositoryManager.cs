﻿using System;
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
        public IDesignRepository Designs { get; set; }

        public void Save()
        {
            Context.SaveChanges();
        }
    }
}