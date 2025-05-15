using AutoMapper;
using Contracts;
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

        public ServiceManager(IUserRepository userRepository, IMapper mapper)
        {
            _userService = new Lazy<IUserService>(() => new UserService(userRepository, mapper));
        }

        public IUserService UserService => _userService.Value;
    }
}
