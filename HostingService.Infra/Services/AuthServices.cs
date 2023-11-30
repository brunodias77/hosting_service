using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HostingService.Application.Response;
using HostingService.Application.Services;
using HostingService.Domain.User;
using HostingService.Domain.ValueObjects;
using HostingService.Infra.Data;
using HostingService.Shared.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace HostingService.Infra.Services
{
    public class AuthServices : IAuthService
    {
        readonly UserManager<IdentityUser> _userManager;
        readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private IdentityUser? _user;


        public AuthServices(ApplicationDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _configuration = configuration;
        }

        public async Task<BaseResponse<Token>> LoginAsync(string username, string password)
        {
            var user = await _userManager.FindByEmailAsync(username);
            var result = await _signInManager.PasswordSignInAsync(username, password, false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var token = CreateToken(user);
                return BaseResponse<Token>.SuccessResponse(token); // Sucesso
            }
            else
            {
                // Construa uma resposta de falha
                return new BaseResponse<Token>
                {
                    Success = false,
                    Data = null,
                    Error = "Falha na autenticação" // Mensagem de erro personalizada
                };
            }
        }


        public async Task<BaseResponse<IdentityResult>> RegisterAsync(User model)
        {
            var user = new IdentityUser
            {
                UserName = model.Email.Value,
                Email = model.Email.Value,
                EmailConfirmed = true
            };
            var result = await _userManager.CreateAsync(user, model.Password.Value);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, UserRoles.User);
                // Retorne uma resposta de sucesso com o resultado da criação do usuário
                return BaseResponse<IdentityResult>.SuccessResponse(result);
            }
            else
            {
                // Retorne uma resposta de falha com o resultado e uma mensagem de erro
                return new BaseResponse<IdentityResult>
                {
                    Success = false,
                    Data = result,
                    Error = "Falha ao registrar o usuário" // Você pode incluir detalhes mais específicos aqui
                };
            }
        }


        public async Task<bool> ValidateUser(User user)
        {
            _user = await _userManager.FindByEmailAsync(user.Email.Value);

            var result = (_user != null && await _userManager.CheckPasswordAsync(_user, user.Password.Value));

            return result;
        }
        public Token CreateToken(IdentityUser user)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.UserName!)
            };

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_configuration.GetSection("JwtSettings:Secret").Value));
            SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return new Token
            {
                AccessToken = tokenHandler.WriteToken(token)
            };
        }
    }
}

