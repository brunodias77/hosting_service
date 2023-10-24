using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HostingService.Application.Users.LoginUser;
using HostingService.Domain.Services;
using HostingService.Domain.User;
using HostingService.Infra.Data;
using HostingService.Shared.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace HostingService.Infra.Services
{
    public class AuthServices : IAuthServices
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

        public async Task<LoginUserResponse> LoginAsync(string username, string password)
        {
            var user = await _userManager.FindByEmailAsync(username);
            var result = await _signInManager.PasswordSignInAsync(username, password, false, lockoutOnFailure: false);
            if(result.Succeeded == true)
            {
                var token = CreateToken(user);
                return new LoginUserResponse
                {
                    SignInResult = result,
                    Token = token

                };
            }
            return new LoginUserResponse
            {
                SignInResult = result,
            };
        }

        public async Task<IdentityResult> RegisterAsync(User model)
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
            }
            return result;
        }

        public async Task<bool> ValidateUser(User user)
        {
            _user = await _userManager.FindByEmailAsync(user.Email.Value);

            var result = (_user != null && await _userManager.CheckPasswordAsync(_user, user.Password.Value));

            return result;
        }
        public string CreateToken(IdentityUser user)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("JwtSettings:Secret").Value);
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = GenerateClaims(user),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = credentials,
            };
            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
            //List<Claim> claims = new()
            //{
            //    new Claim(ClaimTypes.NameIdentifier, user.Id),
            //    new Claim(ClaimTypes.Email, user.Email!),
            //    new Claim(ClaimTypes.Name, user.UserName!)
            //};

            //SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_configuration.GetSection("JwtSettings:Secret").Value));
            //SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha512Signature);

            //SecurityTokenDescriptor tokenDescriptor = new()
            //{
            //    Subject = new ClaimsIdentity(claims),
            //    Expires = DateTime.UtcNow.AddMinutes(10),
            //    SigningCredentials = creds
            //};

            //JwtSecurityTokenHandler tokenHandler = new();
            //SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            //return tokenHandler.WriteToken(token);
        }
        private static ClaimsIdentity GenerateClaims(IdentityUser user)
        {
            var ci = new ClaimsIdentity();
            ci.AddClaim(new Claim(ClaimTypes.Name, user.Email));
            //foreach (var role in user.Roles)
            //    ci.AddClaim(new Claim(ClaimTypes.Role, role));
            return ci;
        }


        //public async Task<string> CreateToken(IdentityUser user)
        //{
        //    var signingCredentials = GetSigningCredentials();
        //    var claims = await GetClaims(user);
        //    var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
        //    return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        //}
        //private SigningCredentials GetSigningCredentials()
        //{
        //    var conf = _configuration.GetSection("JwtSettings:Secret").Value;
        //    var key = Encoding.UTF8.GetBytes(conf);
        //    var secret = new SymmetricSecurityKey(key);

        //    return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        //}
        //private async Task<List<Claim>> GetClaims(IdentityUser user)
        //{
        //    var claims = new List<Claim>
        //    {
        //        // new Claim("UserId", _user.Id),
        //        // new Claim("Username", _user.UserName),
        //        // new Claim("Email", _user.Email),
        //        new Claim("NovaClaim", "NovaClaim")
        //    };
        //    var roles = await _userManager.GetRolesAsync(user);

        //    foreach (var role in roles)
        //    {
        //        claims.Add(new Claim("Role", role));
        //    }
        //    return claims;
        //}

        //private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        //{
        //    var jwtSettings = _configuration.GetSection("JwtSettings");

        //    var tokenOptions = new JwtSecurityToken
        //    (
        //        issuer: jwtSettings["Issuer"],
        //        audience: jwtSettings["Audience"],
        //        claims: claims,
        //        expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expires"])),
        //        signingCredentials: signingCredentials
        //    );

        //    return tokenOptions;
        //}
    }
}

