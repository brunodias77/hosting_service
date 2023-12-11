using System;
using HostingService.Application.Abstraction;
using HostingService.Application.Response;
using HostingService.Application.Services;
using HostingService.Domain.User;
using HostingService.Domain.User.ValueObject;
using HostingService.Domain.ValueObjects;
using MediatR;

using Microsoft.AspNetCore.Identity;

namespace HostingService.Application.Users.RegisterUser
{

    public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, BaseResponse<IdentityResult>>
    {
        private readonly IAuthService _authServices;

        public RegisterUserCommandHandler(IAuthService authServices)
        {
            _authServices = authServices;
        }

        public async Task<BaseResponse<IdentityResult>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = User.CreateRegistered(new FirstName(request.FirstName), new LastName(request.LastName), new Email(request.Email), new Password(request.Password));
            BaseResponse<IdentityResult> response = await _authServices.RegisterAsync(user);
            Console.WriteLine(response);
            return response;
        }
    }
}

