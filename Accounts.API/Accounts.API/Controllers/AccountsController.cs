using Accounts.Services;
using Accounts.Services.DTOs;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;

namespace Accounts.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly ILogger<AccountsController> _logger;
        private readonly IAccountService _accountService;
        private readonly IConfiguration _configuration;

        public AccountsController(ILogger<AccountsController> logger, IAccountService accountService, IConfiguration configuration)
        {
            _logger = logger;
            _accountService = accountService;
            _configuration = configuration;
        }

        [HttpGet(Name = "AccountStatement")]
        public async Task<IActionResult> Get()
        {
            var grpcServiceUrl = _configuration["GrpcServiceUrl"];

            using var channel = GrpcChannel.ForAddress(grpcServiceUrl);
            var client = new Account.AccountClient(channel);

            var request = new AccountStamenetRequest
            {
                PublishEventToRabbitMQ = true
            };

            var response = await client.GetAccountStatementAsync(request);
            return Ok(response.AccountStatement.Select(x => new AccountStatementDto()
            {
                Amount = x.Amount,
                FromAccountNumber = x.FromAccountNumber,
                ToAccountNumber = x.ToAccountNumber,
                TransactionDateTime = x.TransactionDateTime,
                TransactionType = x.TransactionType
            }));
        }

        [HttpPost(Name = "CreateAccount")]
        public async Task<IActionResult> Post(AccountDetailsDto accountDetails)
        {
            return Ok(await _accountService.CreateAccount(accountDetails));
        }
    }
}
