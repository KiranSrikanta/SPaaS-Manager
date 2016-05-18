using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMC.SPaaS.Entities;

namespace EMC.SPaaS.Repository
{
    public class InstanceRepository : IInstanceRepository
    {
        SPaaSDbContext Context { get; set; }

        public InstanceRepository(SPaaSDbContext context)
        {
            Context = context;
        }

        public void Create(InstanceEntity instance)
        {
            Context.Add(instance);
        }

        public void Delete(InstanceEntity instance)
        {
            Context.Remove(instance);
        }

        public IEnumerable<InstanceEntity> GetInstancesForUser(int userId)
        {
            return (from i in Context.Instances where i.Id == userId select i).AsEnumerable();
        }

        public InstanceEntity GetInstance(int instanceId, int userId)
        {
            return Context.Instances.FirstOrDefault(i => i.Id == instanceId && i.UserId == userId);
        }


        public void Provision(InstanceEntity instance, UserEntity user)
        {
            Context.Jobs.Add(new JobEntity
            {
                Instance = instance,
                StatusId = (int)JobStatus.NotStarted,
                TypeId = (int)JobType.Provision,
                User = user
            });

            //Create VMs!!
        }

        public void Provision(InstanceEntity instance, int userId)
        {
            Context.Jobs.Add(new JobEntity
            {
                Instance = instance,
                StatusId = (int)JobStatus.NotStarted,
                TypeId = (int)JobType.Provision,
                UserId = userId
            });
        }

        public void Release(InstanceEntity instance, UserEntity user)
        {
            Context.Jobs.Add(new JobEntity
            {
                Instance = instance,
                StatusId = (int)JobStatus.NotStarted,
                TypeId = (int)JobType.Provision,
                User = user
            });

            //Create VMs!!
        }

        public void Release(InstanceEntity instance, int userId)
        {
            Context.Jobs.Add(new JobEntity
            {
                Instance = instance,
                StatusId = (int)JobStatus.NotStarted,
                TypeId = (int)JobType.Release,
                UserId = userId
            });
        }

        public void TurnOff(InstanceEntity instance, UserEntity user)
        {
            Context.Jobs.Add(new JobEntity
            {
                Instance = instance,
                StatusId = (int)JobStatus.NotStarted,
                TypeId = (int)JobType.Release,
                User = user
            });
        }

        public void TurnOff(InstanceEntity instance, int userId)
        {
            Context.Jobs.Add(new JobEntity
            {
                Instance = instance,
                StatusId = (int)JobStatus.NotStarted,
                TypeId = (int)JobType.TurnOff,
                UserId = userId
            });
        }

        public void TurnOn(InstanceEntity instance, UserEntity user)
        {
            Context.Jobs.Add(new JobEntity
            {
                Instance = instance,
                StatusId = (int)JobStatus.NotStarted,
                TypeId = (int)JobType.TurnOn,
                User = user
            });
        }

        public void TurnOn(InstanceEntity instance, int userId)
        {
            Context.Jobs.Add(new JobEntity
            {
                Instance = instance,
                StatusId = (int)JobStatus.NotStarted,
                TypeId = (int)JobType.TurnOn,
                UserId = userId
            });
        }
    }
}
