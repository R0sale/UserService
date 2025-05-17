using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Entities.ErrorModel
{
    public class ErrorDetails
    {
        public string? Message { get; set; }
        public int StatusCode { get; set; }

        public override string ToString() => JsonSerializer.Serialize(this);
    }
}
