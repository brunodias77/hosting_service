using System;
using HostingService.Application.Response;
using HostingService.Domain.User;
using HostingService.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace HostingService.Application.Services
{
    public interface IAuthService
    {
        Task<BaseResponse<Token>> LoginAsync(string username, string password);
        Task<BaseResponse<IdentityResult>> RegisterAsync(User model);
    }
}

