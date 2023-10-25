using System;
using System.ComponentModel.DataAnnotations;
using HostingService.Application.Abstraction;
using HostingService.Application.Users.RegisterUser;

namespace HostingService.Application.Users.LoginUser
{
    public class LoginUserCommand : ICommand<LoginUserResponse>
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

