namespace Accounts.Services.DTOs
{
    public class AccountDetailsDto
    {
        public string AccountNumber { get; set; } = string.Empty;
        public string AccountHolder { get; set; } = string.Empty;
        public string AccountType { get; set; } = "Savings";
        public decimal Balance { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(AccountHolder);
        }
    }
}
