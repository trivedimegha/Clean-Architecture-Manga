using Manga.Application.Boundaries.Login;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manga.Application.Services
{
    public interface ILogin
    {
        LoginOutput Execute(string username, string password);
    }
}
