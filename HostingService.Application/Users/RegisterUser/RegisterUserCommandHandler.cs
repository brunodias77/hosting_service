using System;
using HostingService.Application.Abstraction;
using HostingService.Domain.Services;
using HostingService.Domain.User;
using HostingService.Domain.User.ValueObject;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace HostingService.Application.Users.RegisterUser
{

    public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, RegisterUserResponse>
    {
        private readonly IAuthServices _authServices;

        public RegisterUserCommandHandler(IAuthServices authServices)
        {
            _authServices = authServices;
        }

        public async Task<RegisterUserResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = User.CreateRegistered(new FirstName(request.FirstName), new LastName(request.LastName), new Email(request.Email), new Password(request.Password));
            IdentityResult response = await _authServices.RegisterAsync(user);
            Console.WriteLine(response);
            return new RegisterUserResponse(response);
        }
    }
}

