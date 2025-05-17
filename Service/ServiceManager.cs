using AutoMapper;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IUserService> _userService;
        private readonly Lazy<IAuthenticationService> _authenticationService;

        public ServiceManager(IUserRepository userRepository, IMapper mapper, UserManager<User> userManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager)
        {
            _userService = new Lazy<IUserService>(() => new UserService(userRepository, mapper));
            _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(mapper, userManager, configuration, roleManager));
        }

        public IUserService UserService => _userService.Value;
        public IAuthenticationService AuthenticationService => _authenticationService.Value;
    }
}
