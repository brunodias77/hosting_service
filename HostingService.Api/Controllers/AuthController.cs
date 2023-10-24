using HostingService.Api.Models;
using HostingService.Application.Users.LoginUser;
using HostingService.Application.Users.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HostingService.Api.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand request)
        {
            var response = await _mediator.Send(new LoginUserCommand(request.Username, request.Password));
            if (response.SignInResult.Succeeded)
            {
                //    return Ok(new { Token = await _repository.CreateTokenAsync(user) });
                return Ok();
            }

            //Console.WriteLine(model);
            //var user = await _userManager.FindByNameAsync(model.Username);
            //var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, lockoutOnFailure: false);
            //if (result.Succeeded)
            //{
            //    return Ok(new { Token = await _repository.CreateTokenAsync(user) });
            //}
            //else
            //{
            //    return Unauthorized(new { message = "Nome de usuário ou senha incorretos." });
            //}
            return Unauthorized();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand request)
        {
            //if (!ModelState.IsValid) return BadRequest("Sua mensagem de erro aqui");

            //// teste
            //var response = await _mediator.Send(new RegisterUserCommand(request.FirstName, request.LastName, request.Email, request.Password));
            //if (!response.Succeeded)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError, new AuthResponse { Status = "Error", Message = "Nao foi possivel criar o usuario " + response.Errors.FirstOrDefault().Description });
            //}
            //return Ok(new AuthResponse { Status = "Success", Message = "Usuario criado com sucesso !" });

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { Status = "Error", Message = "Erro de validação", Errors = errors });
            }

            var response = await _mediator.Send(new RegisterUserCommand(request.FirstName, request.LastName, request.Email, request.Password));
            if (!response.Result.Succeeded)
            {
                var errorDescription = response.Result.Errors.FirstOrDefault()?.Description ?? "Erro desconhecido";
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "Não foi possível criar o usuário", Error = errorDescription });
            }

            return Ok(new AuthResponse { Status = "Success", Message = "Usuário criado com sucesso!" });
        }
    }
}

