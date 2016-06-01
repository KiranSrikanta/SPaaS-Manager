using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMC.SPaaS.Entities;

namespace EMC.SPaaS.Repository
{
    public interface ISolnEnvRepository
    {
        IEnumerable<SolnEnvironmentEntity> GetAll();
    }
}
