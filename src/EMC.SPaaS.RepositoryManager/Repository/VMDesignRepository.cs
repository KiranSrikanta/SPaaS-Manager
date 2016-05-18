using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMC.SPaaS.Entities;

namespace EMC.SPaaS.Repository
{
    public class VMDesignRepository : IVMDesignRepository
    {
        private readonly SPaaSDbContext _context;
        public VMDesignRepository(SPaaSDbContext context)
        {
            _context = context;
        }
        public void Add(VMDesignEntity entity)
        {
            _context.VMDesigns.Add(entity);
            _context.SaveChanges();
        }

        public VMDesignEntity Find(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<VMDesignEntity> GetAll(int userID)
        {
            throw new NotImplementedException();
        }

        public void Remove(int entity)
        {
            throw new NotImplementedException();
        }
    }
}
