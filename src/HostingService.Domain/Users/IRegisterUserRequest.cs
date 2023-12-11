using System;
namespace HostingService.Domain.User
{
    public interface IRegisterUserRequest
    {
        string FirstName { get; }
        string LastName { get; }
        string Email { get; }
        string Password { get; }
    }
}

