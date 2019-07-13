using Manga.Application.Boundaries.Login;
using Manga.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manga.Application.UseCases
{
    public class Login : IUseCase
    {
        private readonly IOutputHandler outputHandler;
        private readonly ILogin loginUserService;

        public Login(IOutputHandler outputHandler,
            ILogin loginUserService)
        {
            this.outputHandler = outputHandler;
            this.loginUserService = loginUserService;
        }

        public async Task Execute(Input input)
        {
            if (input == null)
            {
                outputHandler.Error("Input is null.");
                return;
            }

            var loginOutput = loginUserService.Execute(input.Name.ToString(), input.Password.ToString());
            if (loginOutput == null)
            {
                outputHandler.Error("An error throw when Login with user password");
                return;
            }

            Output output = new Output(loginOutput.CustomerId, loginOutput.Name, loginOutput.Token);
            outputHandler.Handle(output);
        }
    }
}
