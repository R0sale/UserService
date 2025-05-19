using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public class InvalidUserNameOrPasswordException : UnauthorizedException
    {
        public InvalidUserNameOrPasswordException(string username) : base($"The userName {username} or password invalid")
        {}
    }
}
