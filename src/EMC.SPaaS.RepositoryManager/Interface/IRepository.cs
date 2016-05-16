using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMC.SPaaS.RepositoryManager
{
    public interface IRepository<TEnt, in TPk> where TEnt : class
    {

        IEnumerable<TEnt> GetAll();
        TEnt Find(TPk id);
        void Add(TEnt entity);
        void Remove(TEnt entity);

    }
}
