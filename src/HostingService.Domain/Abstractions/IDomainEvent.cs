using System;
using MediatR;

namespace HostingService.Domain.Abstractions
{
    public interface IDomainEvent : INotification
    {
    }
}

