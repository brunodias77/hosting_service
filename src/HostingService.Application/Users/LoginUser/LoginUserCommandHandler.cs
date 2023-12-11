using HostingService.Application.Abstraction;
using HostingService.Application.Response;
using HostingService.Application.Services;
using HostingService.Domain.ValueObjects;

namespace HostingService.Application.Users.LoginUser
{
    public class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, BaseResponse<Token>>
    {
        private readonly IAuthService _authServices;

        public LoginUserCommandHandler(IAuthService authServices)
        {
            _authServices = authServices;

        }
        public async Task<BaseResponse<Token>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            // Chama o método de login do serviço de autenticação
            var result = await _authServices.LoginAsync(request.Username, request.Password);

            // Retorna a resposta do método de login diretamente
            // Assumindo que result já é do tipo BaseResponse<Token>
            return result;
        }
    }
}

