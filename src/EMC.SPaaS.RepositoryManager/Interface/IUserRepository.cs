using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMC.SPaaS.Entities;

namespace EMC.SPaaS.Repository
{
    public interface IUserRepository
    {
        UserEntity GetUser(int userId);
        UserEntity GetUser(string userUniqueId);
        void AddOrUpdate(UserEntity user);
    }
}
