using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMC.SPaaS.Entities;

namespace EMC.SPaaS.Repository
{
    public class SolnEnvRepository : ISolnEnvRepository
    {
        private readonly SPaaSDbContext _context;
        public SolnEnvRepository(SPaaSDbContext context)
        {
            _context = context;
        }
        public IEnumerable<SolnEnvironmentEntity> GetAll()
        {

            return _context.SolnEnvironments.ToList< SolnEnvironmentEntity>();
        }
    }
}
