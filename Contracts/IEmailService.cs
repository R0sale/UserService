using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IEmailService
    {
        string GenerateLink(string userId, string code);
        Task SendEmailAsync(string email, string subject, string message);
    }
}
