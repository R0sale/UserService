using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IEmailService
    {
        string GenerateEmailLink(string userId, string code);
        Task SendEmailAsync(string email, string subject, string message);
        string GenerateRestoreLink(string userId, string code, string newPassword);
    }
}
