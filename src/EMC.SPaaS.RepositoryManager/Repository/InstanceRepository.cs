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
            Provision(instance, user.Id);

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
            Release(instance, user.Id);

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
            TurnOff(instance, user.Id);
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
            TurnOn(instance, user.Id);
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

        public void CreateJob(InstanceEntity instance, JobType jobType, UserEntity user)
        {
            CreateJob(instance, jobType, user.Id);
        }

        public void CreateJob(InstanceEntity instance, JobType jobType, int userId)
        {
            switch (jobType)
            {
                case JobType.Provision:
                    Provision(instance, userId);
                    break;
                case JobType.Release:
                    Release(instance, userId);
                    break;
                case JobType.TurnOff:
                    TurnOff(instance, userId);
                    break;
                case JobType.TurnOn:
                    TurnOn(instance, userId);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(jobType));
            }
        }

        public IEnumerable<ProvisionedVmEntity> GetVMs(InstanceEntity instance)
        {
            return instance.VMs;
        }

        public IEnumerable<ProvisionedVmEntity> GetVMs(int instanceId, int userId)
        {
            return GetInstance(instanceId, userId).VMs;
        }

        public void AddVM(InstanceEntity instance, ProvisionedVmEntity vm)
        {
            vm.InstanceId = instance.Id;
            Context.VMs.Add(vm);
        }

        public void AddVM(InstanceEntity instance, IEnumerable<ProvisionedVmEntity> vmCollection)
        {
            foreach(var vm in vmCollection)
            {
                AddVM(instance, vm);
            }
        }
    }
}
