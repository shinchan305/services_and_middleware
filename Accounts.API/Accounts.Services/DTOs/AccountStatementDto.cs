namespace Accounts.Services.DTOs
{
    public class AccountStatementDto
    {
        public string FromAccountNumber { get; set; }
        public string ToAccountNumber { get; set; }
        public string TransactionType { get; set; } = "Credit";
        public decimal Amount { get; set; }
        public DateTime TransactionDateTime { get; set; }
    }
}
