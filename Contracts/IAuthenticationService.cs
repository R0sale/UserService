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
        Task ValidateUser(UserForAuthenticationDTO userForAuth);
        Task<string> CreateToken();
        Task<IdentityResult> ConfirmEmailAsync(string userId, string code);
        Task RestorePassword(RestorePasswordUserDTO restoreUser);
        Task<IdentityResult> ConfirmPasswordAsync(string userId, string code, string newPassword);
    }
}
