using Accounts.Services.DTOs;

namespace Accounts.Services
{
    public interface IAccountService
    {
        Task CreateAccount(AccountDetailsDto accountDetails);
        List<AccountDetailsDto> GetAccountDetails();
    }
}
