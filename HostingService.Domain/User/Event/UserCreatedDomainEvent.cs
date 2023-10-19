using System;
using HostingService.Domain.Abstractions;

namespace HostingService.Domain.User.Event
{
    public class UserCreatedDomainEvent : DomainEventBase
    {
        public UserId UserId { get; }

        public UserCreatedDomainEvent(UserId userId)
        {
            UserId = userId;
        }
    }
}

