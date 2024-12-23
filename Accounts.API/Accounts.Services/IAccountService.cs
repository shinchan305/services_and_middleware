using Accounts.Services.DTOs;

namespace Accounts.Services
{
    public interface IAccountService
    {
        Task<AccountDetailsDto> CreateAccount(AccountDetailsDto accountDetails);
        List<AccountDetailsDto> GetAccountDetails();
    }
}
