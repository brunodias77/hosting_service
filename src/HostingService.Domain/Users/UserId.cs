using System;
using HostingService.Domain.Abstractions;

namespace HostingService.Domain.User
{
    public class UserId : TypeIdValueBase
    {
        public UserId(Guid value) : base(value)
        {

        }
    }
}

