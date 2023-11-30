using System;
namespace HostingService.Domain.ValueObjects
{
    public record Token
    {
        public string AccessToken { get; init; }
    }
}

