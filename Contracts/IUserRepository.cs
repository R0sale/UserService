using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsers(bool trackChanges);
        Task<User> GetUser(Guid id, bool trackChanges);
        void CreateUser(User user);
        void DeleteUser(User user);
        Task SaveAsync();
    }
}
