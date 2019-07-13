using System;
using System.Collections.Generic;
using System.Text;

namespace Manga.Infrastructure.IdentityAuthentication.Basic
{
    public class JWTConfigs
    {
        public string JwtKey { get; set; }
        public string JwtExpireDays { get; set; }
        public string JwtIssuer { get; set; }

    }
}
