using System;
using MediatR;

namespace HostingService.Application.Abstraction
{
    public interface ICommandHandler<in TCommand, TResult> : IRequestHandler<TCommand, TResult> where TCommand : ICommand<TResult>
    {
    }
}

