using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace HostingService.Infra.Repositories
{
    public class UserAuthenticationRepository : IUserAuthenticationRepository
    {
        public Task<string> CreateTokenAsync(IdentityUser user)
        {
            throw new NotImplementedException();
        }
    }
}