using HostingService.Application.Abstraction;
using HostingService.Domain.Services;

namespace HostingService.Application.Users.CreateToken
{
    public class CreateTokenCommandHandler : ICommandHandler<CreateTokenCommand, CreateTokenResponse>
    {
        private readonly IAuthServices _authServices;

        public CreateTokenCommandHandler(IAuthServices authServices)
        {
            _authServices = authServices;

        }

        public async Task<CreateTokenResponse> Handle(CreateTokenCommand request, CancellationToken cancellationToken)
        {
            var token = await _authServices.CreateToken();
            return new CreateTokenResponse
            {
                Token = token
            };
        }
    }
}