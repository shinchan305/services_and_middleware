namespace Accounts.CrossCutting
{
    public static class Constants
    {
        public const string ACCOUNT_STATEMENT_FILE_NAME = "AccountStatement.json";
        public const string ACCOUNT_DETAILS_FILE_NAME = "AccountDetails.json";

        public const string ACCOUNT_CREATION_EXCHANGE_NAME = "event_account_created";
        public const string ACCOUNT_CREATION_ROUTING_KEY = "event.account.created";

        public const string PDF_CREATION_EXCHANGE_NAME = "event_pdf_generation";
        public const string PDF_CREATION_ROUTING_KEY = "event.pdf.create";
        public const string PDF_CREATED_EXCHANGE_NAME = "event_pdf_generated";
    }
}
