using System;
namespace HostingService.Domain.User
{
    public interface IUserRepository
    {
        Task<User> GetUserById(Guid id);
        Task<bool> AddUser(IRegisterUserRequest user);
    }
}

