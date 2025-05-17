using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;
using Shared.DTOObjects;

namespace Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<UserDTO> GetUserAsync(Guid id);
        Task DeleteUser(Guid id);
        Task UpdateUser(Guid id, UserForUpdateDTO userForUpd);
        Task<(UserForUpdateDTO userForUpd, User userEntity)> GetUserForPatialUpdate(Guid id);
        Task PartiallyUpdateUser(User user, UserForUpdateDTO userForUpd);
    }
}
