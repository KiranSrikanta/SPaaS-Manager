using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMC.SPaaS.Entities;
namespace EMC.SPaaS.Repository
{
    public interface IVMDesignRepository
    {
        IEnumerable<VMDesignEntity> GetAll(int userID);
        VMDesignEntity Find(int id);
        void Add(VMDesignEntity entity);
        void Remove(int id,int userID);
    }
}
