using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMC.SPaaS.Entities;
namespace EMC.SPaaS.Repository
{
    public interface IDesignRepository
    {
        IEnumerable<DesignEntity> GetAll(int userID);
        DesignEntity Find(int id);
        void Add(DesignEntity entity);
        void Remove(int entity);

    }
}
