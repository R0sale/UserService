using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Shared.DTOObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IAuthenticationService
    {
        Task<IdentityResult> RegisterUser(UserForRegistrationDTO userForRegistration);
        Task<bool> ValidateUser(UserForAuthenticationDTO userForAuth);
        Task<string> CreateToken();
        Task<IdentityResult> ConfirmEmailAsync(User user, string code);
        Task<User> GetUserToConfirmEmail(Guid id);
    }
}
