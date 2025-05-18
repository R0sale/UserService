using AutoMapper;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Shared.DTOObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using MailKit;
using Entities.Exceptions;

namespace Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;

        private User? _user;

        public AuthenticationService(IMapper mapper, UserManager<User> userManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager, IEmailService emailService)
        {
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _emailService = emailService;
        }

        public async Task<IdentityResult> RegisterUser(UserForRegistrationDTO userForRegistration)
        {
            bool rolesExist = true;

            var user = _mapper.Map<User>(userForRegistration);

            var result = await _userManager.CreateAsync(user, userForRegistration.Password);

            foreach(var role in userForRegistration.Roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                    rolesExist = false;
            }

            if (result.Succeeded && rolesExist)
            {
                await _userManager.AddToRolesAsync(user, userForRegistration.Roles);

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var callBackUri = _emailService.GenerateEmailLink(user.Id, code);

                await _emailService.SendEmailAsync(user.Email, "Confirm your email", $"To confirm your email follow the link <a href=\"{callBackUri}\">link</a>");
            }

            return result;
        }

        public async Task<bool> ValidateUser(UserForAuthenticationDTO userForAuth)
        {
            _user = await _userManager.FindByNameAsync(userForAuth.UserName);

            var result = (_user != null && await _userManager.CheckPasswordAsync(_user, userForAuth.Password));

            return result;
        }

        public async Task<string> CreateToken()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(string userId, string code)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code))
                throw new NullParameterException("userId or code");

            if (!Guid.TryParse(userId, out var id))
                throw new NotApproproateParam("Id isn't in appropriate format");

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                throw new UserNotFoundException();

            var result = await _userManager.ConfirmEmailAsync(user, code);

            return result;
        }

        public async Task RestorePassword(RestorePasswordUserDTO restoreUser)
        {
            var user = await _userManager.FindByEmailAsync(restoreUser.Email);

            if (user == null)
                throw new UserNotFoundException();

            if (!await _userManager.IsEmailConfirmedAsync(user))
                throw new NotConfirmedUserException(user.Email);

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            var callBackUri = _emailService.GenerateRestoreLink(user.Id, code, restoreUser.NewPassword);

            await _emailService.SendEmailAsync(user.Email, "Restore password", $"To change your password follow this link <a href=\"{callBackUri}\">link</a>");
        }

        public async Task<IdentityResult> ConfirmPasswordAsync(string userId, string code, string newPassword)
        {
            if (string.IsNullOrEmpty(userId))
                throw new NullParameterException("userId");

            if (string.IsNullOrEmpty(code))
                throw new NullParameterException("code");

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                throw new UserNotFoundException();

            var result = await _userManager.ResetPasswordAsync(user, code, newPassword);

            return result;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = Environment.GetEnvironmentVariable("SECRET");
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            return new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, _user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(_user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            var tokenOptions = new JwtSecurityToken
            (
                issuer: jwtSettings["validIssuer"],
                audience: jwtSettings["validAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expires"])),
                signingCredentials: signingCredentials
            );

            return tokenOptions;
        }
    }
}
