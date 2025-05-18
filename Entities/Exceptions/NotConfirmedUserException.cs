using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public class NotConfirmedUserException : ForbiddenException
    {
        public NotConfirmedUserException(string email) : base($"User has not confirmed email {email}")
        { }
    }
}
