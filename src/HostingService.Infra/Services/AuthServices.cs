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
            if (user != null)
            {
                // A senha é verificada aqui
                var result = await _signInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var token = CreateToken(user); // Crie o token aqui
                    return BaseResponse<Token>.SuccessResponse(token);
                }
            }
            // Construa uma resposta de falha
            return new BaseResponse<Token>
            {
                Success = false,
                Data = null,
                Error = "Falha na autenticação" // Mensagem de erro personalizada
            };
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

        public Token CreateToken(IdentityUser user)
        {
            // Cria uma lista de claims (direitos) com informações do usuário
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id), // Identificador único do usuário
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty), // Email do usuário
                new Claim(ClaimTypes.Name, user.UserName) // Nome do usuário
                // Você pode adicionar mais claims aqui se necessário
            };

            // Obtém a chave secreta de configuração para assinar o token
            var secret = _configuration.GetSection("JwtSettings:Secret").Value;
            // Verifica se a chave secreta é válida
            if (string.IsNullOrEmpty(secret) || secret.Length < 16)
            {
                throw new InvalidOperationException("Secret key is not set properly.");
            }

            // Cria a chave simétrica para assinar o token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            // Define as credenciais de assinatura usando o algoritmo HMAC SHA-512
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // Configura o token com as claims, a data de expiração e as credenciais de assinatura
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                // Define a expiração do token para um tempo futuro com base nas configurações
                Expires = DateTime.UtcNow.AddHours(_configuration.GetValue<int>("JwtSettings:ExpirationInHours")),
                SigningCredentials = creds
            };

            // Criando o token
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Retornando o token em um formato serializável
            return new Token
            {
                AccessToken = tokenHandler.WriteToken(token)
            };
        }

        // Método para criar um token JWT
        //public Token CreateToken(IdentityUser user)
        //{
        //    var claims = GetClaims(user); // Obtém as claims do usuário
        //    var key = GetSymmetricSecurityKey(); // Obtém a chave simétrica para assinatura
        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature); // Cria as credenciais para a assinatura

        //    // Configura o descritor do token
        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(claims),
        //        Expires = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("JwtSettings:TokenExpiry")),
        //        SigningCredentials = creds
        //    };

        //    // Cria o token JWT
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var token = tokenHandler.CreateToken(tokenDescriptor);

        //    // Retorna o token em formato serializável
        //    return new Token { AccessToken = tokenHandler.WriteToken(token) };
        //}

        //// Método privado para gerar as claims do usuário
        //private List<Claim> GetClaims(IdentityUser user)
        //{
        //    return new List<Claim>
        //    {
        //        new Claim(ClaimTypes.NameIdentifier, user.Id),
        //        new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
        //        new Claim(ClaimTypes.Name, user.UserName)
        //        // Outras claims podem ser adicionadas aqui
        //    };
        //}

        //// Método privado para obter a chave simétrica de assinatura do token
        //private SymmetricSecurityKey GetSymmetricSecurityKey()
        //{
        //    var secret = _configuration.GetSection("JwtSettings:Secret").Value;
        //    // Validação da chave secreta
        //    if (string.IsNullOrEmpty(secret) || secret.Length < 16)
        //    {
        //        throw new InvalidOperationException("Secret key is not set properly.");
        //    }

        //    return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        //}

    }
}

