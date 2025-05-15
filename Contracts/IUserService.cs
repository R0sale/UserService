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
        Task<IEnumerable<UserDTO>> GetAllUsersAsync(bool trackChanges);
        Task<UserDTO> GetUserAsync(Guid id, bool trackChanges);
        Task<UserDTO> CreateUser(ProductForCreationDTO productForCreation);
        Task DeleteUser(Guid id, bool trackChanges);
        Task UpdateUser(Guid id, UserForUpdateDTO productForUpd, bool trackChanges);
        Task<(UserForUpdateDTO productForUpd, User productEntity)> GetProductForPatialUpdate(Guid id, bool trackChanges);
        Task SaveChangesForPatrialUpdate(UserForUpdateDTO productForUpd, User product);
    }
}
