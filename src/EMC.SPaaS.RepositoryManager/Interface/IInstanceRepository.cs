using System.Collections.Generic;
using EMC.SPaaS.Entities;

namespace EMC.SPaaS.Repository
{
    public interface IInstanceRepository
    {
        void Create(InstanceEntity instance);
        void Delete(InstanceEntity instance);
        IEnumerable<InstanceEntity> GetInstancesForUser(int userId);
        InstanceEntity GetInstance(int instanceId, int userId);


        void CreateJob(InstanceEntity instance, JobType jobType, UserEntity user);
        void CreateJob(InstanceEntity instance, JobType jobType, int userId);
        void Provision(InstanceEntity instance, UserEntity user);
        void Provision(InstanceEntity instance, int userId);
        void TurnOff(InstanceEntity instance, UserEntity user);
        void TurnOff(InstanceEntity instance, int userId);
        void TurnOn(InstanceEntity instance, UserEntity user);
        void TurnOn(InstanceEntity instance, int userId);
        void Release(InstanceEntity instance, UserEntity user);
        void Release(InstanceEntity instance, int userId);


        void AddVM(InstanceEntity instance, ProvisionedVmEntity vm);
        void AddVM(InstanceEntity instance, IEnumerable<ProvisionedVmEntity> vmCollection);
    }
}