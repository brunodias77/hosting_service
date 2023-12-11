using System;
using Microsoft.AspNetCore.Identity;

namespace HostingService.Application.Users.LoginUser
{
    public class LoginUserResponse
    {
        public SignInResult SignInResult { get; set; }
        public string? Token { get; set; }
    }
}

