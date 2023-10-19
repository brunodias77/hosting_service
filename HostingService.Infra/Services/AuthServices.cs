using System;
using HostingService.Domain.Services;
using HostingService.Domain.User;
using HostingService.Infra.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HostingService.Infra.Services
{
    public class AuthServices : IAuthServices
    {
        readonly UserManager<IdentityUser> _userManager;
        readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public AuthServices(ApplicationDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public async Task LoginAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            var result = await _signInManager.PasswordSignInAsync(username, password, false, lockoutOnFailure: false);
        }

        public async Task RegisterAsync(User model)
        {
            //if (!ModelState.IsValid) return false;
            var user = new IdentityUser
            {
                UserName = model.Email.Value,
                Email = model.Email.Value,
                EmailConfirmed = true
            };
            var result = await _userManager.CreateAsync(user, model.Password.Value);
        }
    }
}

