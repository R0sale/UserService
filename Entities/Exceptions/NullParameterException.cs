using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public class NullParameterException : BadRequestException
    {
        public NullParameterException(string parameter) : base($"The parameter {parameter} is null")
        { }
    }
}
