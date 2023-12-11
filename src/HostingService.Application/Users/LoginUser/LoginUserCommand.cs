using System;
using System.ComponentModel.DataAnnotations;
using HostingService.Application.Abstraction;
using HostingService.Application.Response;
using HostingService.Application.Users.RegisterUser;
using HostingService.Domain.ValueObjects;

namespace HostingService.Application.Users.LoginUser
{
    public class LoginUserCommand : ICommand<BaseResponse<Token>>
    {
        [Required(ErrorMessage = "User Name is required")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

        public LoginUserCommand(string userName, string password)
        {
            Username = userName;
            Password = password;
        }
    }
}

