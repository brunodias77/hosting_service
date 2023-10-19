
using Azure.Core;
using HostingService.API.Models;
using HostingService.Application.Users.RegisterUser;
using HostingService.Infra.Repositories;
using HostingService.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IUserAuthenticationRepository _repository;
    private readonly IMediator _mediator;


    public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IUserAuthenticationRepository repository, IMediator mediator)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _repository = repository;
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginModel model)
    {
        Console.WriteLine(model);
        var user = await _userManager.FindByNameAsync(model.Username);
        var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            return Ok(new { Token = await _repository.CreateTokenAsync(user) });
        }
        else
        {
            return Unauthorized(new { message = "Nome de usuário ou senha incorretos." });
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand request)
    {
        var response = await _mediator.Send(new RegisterUserCommand(request.FirstName, request.LastName, request.Email, request.Password));
        return Ok(response);
    }
}

