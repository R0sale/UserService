using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.DTOObjects;

namespace UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;

        public UserRepository(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var users = await _userManager.Users
                .OrderBy(u => u.FirstName)
                .ToListAsync();

            return users;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            return user;
        }

        public async Task<IEnumerable<string>> GetRoles(User user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<User> GetUser(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            return user;
        }

        public async Task DeleteUser(User user)
        {
            await _userManager.DeleteAsync(user);
        }

        public async Task UpdateUser(User user, UserForUpdateDTO userForUpd)
        {
            await _userManager.UpdateAsync(user);

            await _userManager.RemoveFromRolesAsync(user, await GetRoles(user));

            await _userManager.AddToRolesAsync(user, userForUpd.Roles);
        }

        public async Task PartiallyUpdateUser(User user, UserForUpdateDTO userForUpd)
        {
            await _userManager.UpdateAsync(user);

            var roles = userForUpd.Roles;

            if (roles != null)
            {
                await _userManager.RemoveFromRolesAsync(user, await GetRoles(user));
                await _userManager.AddToRolesAsync(user, roles);
            }
                
        }
    }
}
