using HostingService.Application.Abstraction;
using HostingService.Domain.Services;

namespace HostingService.Application.Users.LoginUser
{
    public class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, LoginUserResponse>
    {
        private readonly IAuthServices _authServices;

        public LoginUserCommandHandler(IAuthServices authServices)
        {
            _authServices = authServices;

        }

        public async Task<LoginUserResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _authServices.LoginAsync(request.Username, request.Password);
            return new LoginUserResponse
            {
                SignInResult = result,

            };
        }
    }
}

