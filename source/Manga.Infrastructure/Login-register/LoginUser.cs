using Manga.Application.Boundaries.Login;
using Manga.Application.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manga.Infrastructure.IdentityAuthentication
{
    public class LoginUser : ILogin
    {
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IGenerateToken generateToken;

        public LoginUser(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IGenerateToken generateToken)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.generateToken = generateToken;
        }

        public LoginOutput Execute(string username, string password)
        {
            return LoginAsync(username, password).Result;
        }

        private async Task<LoginOutput> LoginAsync(string username, string password)
        {
            var result = await signInManager.PasswordSignInAsync(username, password, true, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                var user = userManager.FindByNameAsync(username).Result;

                var token = await generateToken.GetToken(username, user);


                return new LoginOutput { CustomerId = new Guid(user.Id), Name = user.UserName, Token = token };
            }

            return null;

        }
    }
}
