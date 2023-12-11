using System;
using MediatR;

namespace HostingService.Application.Abstraction
{
    public interface ICommand<out TResult> : IRequest<TResult>
    {
    }
}

