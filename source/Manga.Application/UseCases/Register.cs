namespace Manga.Application.UseCases
{
    using System.Threading.Tasks;
    using Manga.Application.Boundaries.Register;
    using Manga.Application.Repositories;
    using Manga.Domain.Accounts;
    using Manga.Domain;
    using Manga.Application.Services;
    using System;
    using Manga.Domain.ValueObjects;

    public sealed class Register : IUseCase
    {
        private readonly IEntitiesFactory _entityFactory;
        private readonly IOutputHandler _outputHandler;
        private readonly ICustomerRepository _customerRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IRegister _registerUserService;

        public Register(
            IEntitiesFactory entityFactory,
            IOutputHandler outputHandler,
            ICustomerRepository customerRepository,
            IAccountRepository accountRepository,
            IRegister registerUserService)
        {
            _entityFactory = entityFactory;
            _outputHandler = outputHandler;
            _customerRepository = customerRepository;
            _accountRepository = accountRepository;
            _registerUserService = registerUserService;
        }

        public async Task Execute(Input input)
        {
            if (input == null)
            {
                _outputHandler.Error("Input is null.");
                return;
            }

            var registerOutput = _registerUserService.Execute(input.Name.ToString(), input.Password.ToString());
            if (registerOutput == null)
            {
                _outputHandler.Error("An error throw when registering user ID");
                return;
            }


            var customer = _entityFactory.NewCustomer(registerOutput.CustomerId, input.SSN, input.Name);
            var account = _entityFactory.NewAccount(customer.Id);

            ICredit credit = account.Deposit(input.InitialAmount);
            if (credit == null)
            {
                _outputHandler.Error("An error happened when depositing the amount.");
                return;
            }

            customer.Register(account.Id);

            await _customerRepository.Add(customer);
            await _accountRepository.Add(account, credit);

            Output output = new Output(customer, account, registerOutput.Token);
            _outputHandler.Handle(output);
        }
    }
}