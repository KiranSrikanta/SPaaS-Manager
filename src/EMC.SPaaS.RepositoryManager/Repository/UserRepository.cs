using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMC.SPaaS.Entities;

namespace EMC.SPaaS.Repository
{
    public class UserRepository : IUserRepository
    {
        SPaaSDbContext Context { get; set; }

        public UserRepository(SPaaSDbContext context)
        {
            Context = context;
        }

        public UserEntity GetUser(int userId)
        {
            return Context.Users.FirstOrDefault(u => u.Id == userId);
        }

        public UserEntity GetUser(string userUniqueId)
        {
            return Context.Users.FirstOrDefault(u => u.UserId == userUniqueId);
        }

        public void AddOrUpdate(UserEntity user)
        {
            if(user.Id == default(int))
            {
                Context.Users.Add(user);
            }
            else
            {
                var dbUser = Context.Users.FirstOrDefault(u => u.Id == user.Id || u.UserId == user.UserId);
                if(user == null)
                {
                    Context.Users.Add(user);
                }
                else
                {
                    dbUser.AccessToken = user.AccessToken;
                    dbUser.AuthenticationProvider = user.AuthenticationProvider;
                    dbUser.UserId = user.UserId;
                    dbUser.UserName = user.UserName;
                }
            }
        }
    }
}
