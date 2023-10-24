using Microsoft.AspNetCore.Identity;


namespace HostingService.Domain.Services
{
    public interface IAuthServices
    {
        Task<IdentityResult> RegisterAsync(HostingService.Domain.User.User model);
        Task<SignInResult> LoginAsync(string username, string password);
        Task<string> CreateToken();
    }
}

