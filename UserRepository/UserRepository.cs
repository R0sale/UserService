using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace UserRepository
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        public UserRepository(UserRepositoryContext context) : base(context)
        { }

        public async Task<IEnumerable<User>> GetAllUsers(bool trackChanges)
        {
            return await FindAll(trackChanges)
                .OrderBy(u => u.FirstName)
                .ToListAsync();
        }

        public async Task<User> GetUser(Guid id, bool trackChanges)
        {
            return await FindByCondition(u => u.Id.Equals(id), trackChanges)
                .SingleOrDefaultAsync();
        }

        public void CreateUser(User user)
        {
            CreateUser(user);
        }

        public void DeleteUser(User user)
        {
            DeleteUser(user);
        }
    }
}
