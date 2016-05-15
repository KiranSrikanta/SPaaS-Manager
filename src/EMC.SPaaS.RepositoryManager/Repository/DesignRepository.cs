using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMC.SPaaS.RepositoryManager
{
    public class DesignRepository : IRepository<DesignItem, string>
    {
        private readonly RepositoryContext _context;
        public DesignRepository(RepositoryContext context)
        {
            _context = context;
        }

        //static Dictionary<string, DesignItem> designItems = new Dictionary<string, DesignItem>();
        public void Add(DesignItem nItem)
        {
            nItem.DesignID = Guid.NewGuid().ToString();
            //Save to DB
            _context.DesignItems.Add(nItem);
        }

        public DesignItem Find(string designID)
        {
            DesignItem fItem;
            IEnumerable<DesignItem> _designItems = _context.DesignItems.ToList();
            //fItem = null;
           fItem = _designItems.SingleOrDefault(s => s.DesignID == designID);
            return fItem;
        }

        public IEnumerable<DesignItem> GetAll()
        {
            //return designItems.Values;
            return _context.DesignItems.ToList();
        }

        public void Remove(DesignItem designItem)
        {
           var obj = _context.DesignItems.Single(m => m.DesignID == designItem.DesignID);
            _context.DesignItems.Remove(obj);

        }

        public void Update(DesignItem oItem)
        {
            // _context.DesignItems.Update()
            //designItems[oItem.DesignID] = oItem;
            //ToDO

        }
    }
}

