using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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

        public async Task<SignInResult> LoginAsync(string username, string password)
        {
            var user = await _userManager.FindByEmailAsync(username);
            var result = await _signInManager.PasswordSignInAsync(username, password, false, lockoutOnFailure: false);
            return result;
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

        public async Task<string> CreateToken()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }
        private SigningCredentials GetSigningCredentials()
        {
            var conf = _configuration.GetSection("JwtSettings:Secret").Value;
            var key = Encoding.UTF8.GetBytes(conf);
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
            {
                // new Claim("UserId", _user.Id),
                // new Claim("Username", _user.UserName),
                // new Claim("Email", _user.Email),
                new Claim("NovaClaim", "NovaClaim")
            };
            var roles = await _userManager.GetRolesAsync(_user);

            foreach (var role in roles)
            {
                claims.Add(new Claim("Role", role));
            }
            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            var tokenOptions = new JwtSecurityToken
            (
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expires"])),
                signingCredentials: signingCredentials
            );

            return tokenOptions;
        }

    }
}

