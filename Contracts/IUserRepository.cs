using Entities.Models;
using Shared.DTOObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUser(Guid id);
        Task DeleteUser(User user);
        Task UpdateUser(User user, UserForUpdateDTO userForUpd);
        Task<IEnumerable<string>> GetRoles(User user);
        public Task PartiallyUpdateUser(User user, UserForUpdateDTO userForUpd);
    }
}
