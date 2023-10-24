using Microsoft.AspNetCore.Identity;


namespace HostingService.Domain.Services
{
    public interface IAuthServices
    {
        Task RegisterAsync(HostingService.Domain.User.User model);
        Task LoginAsync(string username, string password);
        string CreateToken(IdentityUser user);
    }
}

