using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteLagoon.Application.Contract
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string fromaddress, string toaddress, string subject, string message);
    }
}
