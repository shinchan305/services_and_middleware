using Accounts.CrossCutting;
using Grpc.Core;
using System.Transactions;

namespace Account.Services.gRPC.Services
{
    public class AccountService : Account.AccountBase
    {
        public async override Task<AccountStatementReply> GetAccountStatement(AccountStamenetRequest request, ServerCallContext context)
        {
            var statementDetails = Utility.ReadDataFromFile<List<AccountStatementDto>>(Constants.ACCOUNT_STATEMENT_FILE_NAME);
            if (request.PublishEventToRabbitMQ)
            {
                await Utility.PublishEventToRabbitMQ(
                            string.Empty,
                            Constants.PDF_CREATION_EXCHANGE_NAME,
                            Constants.PDF_CREATION_ROUTING_KEY
                        ); 
            }

            AccountStatementReply reply = new AccountStatementReply();
            reply.AccountStatement.AddRange(statementDetails);

            return reply;
        }
    }
}
