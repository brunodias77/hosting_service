using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace HostingService.Application.Users.RegisterUser
{
    public class RegisterUserResponse : IdentityResult
    {
        public IdentityResult Result { get; private set; }

        public RegisterUserResponse(IdentityResult result)
        {
            Result = result;
        }
    }
}

