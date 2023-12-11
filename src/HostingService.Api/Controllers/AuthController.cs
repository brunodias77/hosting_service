using HostingService.Api.Models;
using HostingService.Application.Users.LoginUser;
using HostingService.Application.Users.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HostingService.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand request)
        {
            var response = await _mediator.Send(new LoginUserCommand(request.Username, request.Password));

            // Verifique se a autenticação foi bem-sucedida
            if (!response.Success)
            {
                // Pode-se incluir a mensagem de erro da resposta, se desejado
                return Unauthorized(response.Error);
            }

            // Em caso de sucesso, retorne o token
            return Ok(response.Data.AccessToken);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand request)
        {
            //if (!ModelState.IsValid)
            //{
            //    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            //    return BadRequest(new { Status = "Error", Message = "Erro de validação", Errors = errors });
            //}
            //var response = await _mediator.Send(new RegisterUserCommand(request.FirstName, request.LastName, request.Email, request.Password));
            //if (!response.Success)
            //{
            //    var errorDescription = response.Error.FirstOrDefault() ?? "Erro desconhecido";
            //    return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "Não foi possível criar o usuário", Error = errorDescription });
            //}
            //return Ok(new AuthResponse { Status = "Success", Message = "Usuário criado com sucesso!" });

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { Status = "Error", Message = "Erro de validação", Errors = errors });
            }

            var response = await _mediator.Send(request);

            if (!response.Success)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "Não foi possível criar o usuário", Error = response.Error });
            }

            return Ok(new { Status = "Success", Message = "Usuário criado com sucesso!", Token = response.Data });
        }
    }
}

