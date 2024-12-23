using Accounts.CrossCutting;
using Accounts.Services.DTOs;
using Newtonsoft.Json;
using Constants = Accounts.CrossCutting.Constants;

namespace Accounts.Services
{
    public class AccountsService : IAccountService
    {
        public async Task<AccountDetailsDto> CreateAccount(AccountDetailsDto accountDetails)
        {
            if (accountDetails.IsValid())
            {
                var allAccounts = GetAccountDetails();
                accountDetails.AccountNumber = Utility.GenerateAccountNumber();
                allAccounts.Add(accountDetails);
                Utility.WriteDataToFile(JsonConvert.SerializeObject(allAccounts), Constants.ACCOUNT_DETAILS_FILE_NAME);
                await Utility.PublishEventToRabbitMQ(
                    JsonConvert.SerializeObject(accountDetails),
                    Constants.ACCOUNT_CREATION_EXCHANGE_NAME,
                    Constants.ACCOUNT_CREATION_ROUTING_KEY
                );
            }
            else
            {
                throw new ArgumentNullException("Please provide Account Holder name.");
            }

            return accountDetails;
        }

        public List<AccountDetailsDto> GetAccountDetails()
        {
            return Utility.ReadDataFromFile<List<AccountDetailsDto>>(Constants.ACCOUNT_DETAILS_FILE_NAME);
        }
    }
}
