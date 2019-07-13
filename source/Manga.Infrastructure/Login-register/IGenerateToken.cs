using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manga.Infrastructure.IdentityAuthentication
{
    public interface IGenerateToken
    {
        Task<string> GetToken(string username, IdentityUser user);
    }
}
