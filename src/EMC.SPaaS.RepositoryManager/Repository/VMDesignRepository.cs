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
            
        }

        public VMDesignEntity Find(int id)
        {
            
            return _context.VMDesigns.FirstOrDefault(s=> s.DId == id);
        }

        public IEnumerable<VMDesignEntity> GetAll(int userID)
        {
            
            return _context.VMDesigns.Where(vm => vm.UserId == userID);
        }

        public void Remove(int designID,int userID)
        {
            var itemRm = _context.VMDesigns.FirstOrDefault(vm => vm.DId == designID && vm.UserId == userID);
            if(itemRm !=null)
            {
                _context.VMDesigns.Remove(itemRm);
            }
        }
    }
}
