using Manga.Application.Boundaries.Register;
using Manga.Application.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;

namespace Manga.Infrastructure.IdentityAuthentication
{
    public sealed class RegisterUser : IRegister
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IGenerateToken generateToken;
        private RegisterOutput Output { get; set; }

        public RegisterUser(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IGenerateToken generateToken)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.generateToken = generateToken;
        }

        public RegisterOutput Execute(string username, string password)
        {
            return RegistrationAsync(username, password).Result;
        }

        private async Task<RegisterOutput> RegistrationAsync(string username, string password)
        {
            var user = new IdentityUser { UserName = username };
            var result = await userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: false);

                var token = await generateToken.GetToken(username, user);


                return new RegisterOutput { CustomerId = new Guid(user.Id), Token = token };
            }

            return null;

        }
    }
}
