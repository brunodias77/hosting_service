using HostingService.Api.Models;
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
        public async Task<IActionResult> Login(LoginModel model)
        {
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
            return Ok();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand request)
        {
            // teste
            var response = await _mediator.Send(new RegisterUserCommand(request.FirstName, request.LastName, request.Email, request.Password));
            return Ok(response);
        }
    }
}

