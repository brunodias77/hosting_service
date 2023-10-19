using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HostingService.Domain.Services
{
    public interface IAuthServices
    {
        Task RegisterAsync(HostingService.Domain.User.User model);
        Task LoginAsync(string username, string password);
    }
}

