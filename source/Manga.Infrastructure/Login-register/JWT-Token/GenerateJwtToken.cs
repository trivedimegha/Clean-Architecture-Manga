using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Manga.Infrastructure.IdentityAuthentication.Basic
{
    public sealed class GenerateJwtToken : IGenerateToken
    {
        private readonly JWTConfigs jWTConfigs;

        public GenerateJwtToken(JWTConfigs jWTConfigs)
        {
            this.jWTConfigs = jWTConfigs;
        }

        async Task<string> IGenerateToken.GetToken(string username, IdentityUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jWTConfigs.JwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(jWTConfigs.JwtExpireDays));

            var token = new JwtSecurityToken(
                jWTConfigs.JwtIssuer,
                jWTConfigs.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
