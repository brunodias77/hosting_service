using Microsoft.AspNetCore.Identity;

namespace HostingService.Infra.Repositories
{
    public interface IUserAuthenticationRepository
    {
        Task<string> CreateTokenAsync(IdentityUser user);

    }
}