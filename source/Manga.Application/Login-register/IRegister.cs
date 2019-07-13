using Manga.Application.Boundaries.Register;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manga.Application.Services
{
    public interface IRegister
    {
        RegisterOutput Execute(string username, string password);
    }
}
