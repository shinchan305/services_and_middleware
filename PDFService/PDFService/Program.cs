using Grpc.Net.Client;
using Newtonsoft.Json;
using PDFService;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Constants = PDFService.Constants;

class Consumer
{
    static async Task Main(string[] args)
    {
        // Create a connection to the RabbitMQ server
        var factory = new ConnectionFactory
        {
            HostName = "my-rabbit",
            UserName = "user",
            Password = "password"
        };
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        // Declare a topic exchange
        string exchangeName = Constants.PDF_CREATION_EXCHANGE_NAME;
        await channel.ExchangeDeclareAsync(exchange: exchangeName, type: ExchangeType.Topic);
        await channel.ExchangeDeclareAsync(exchange: Constants.PDF_CREATED_EXCHANGE_NAME, type: ExchangeType.Fanout);

        // Create a unique queue for the consumer
        var queueResult = await channel.QueueDeclareAsync();
        string queueName = queueResult.QueueName;

        // Bind the queue to the exchange with a binding key
        string bindingKey = Constants.PDF_CREATION_ROUTING_KEY;
        await channel.QueueBindAsync(queue: queueName,
                          exchange: exchangeName,
                          routingKey: bindingKey);

        Console.WriteLine($" [PDF Service] Waiting for messages from {exchangeName} matching '{bindingKey}'. Press [Enter] to exit.");

        // Define a consumer to process messages
        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var routingKey = ea.RoutingKey;
            var exchange = ea.Exchange;
            Console.WriteLine($" [PDF Service] Received from Exchange '{exchange}' with Routing Key '{routingKey}': {message}");
            var data = await GetAccountStatement();
            Console.WriteLine($" [PDF Service] Generated PDF: {data}");
            Console.WriteLine($" [PDF Service] Publishing PDF to Exchange '{Constants.PDF_CREATED_EXCHANGE_NAME}'");
            await channel.BasicPublishAsync(Constants.PDF_CREATED_EXCHANGE_NAME, string.Empty, Encoding.UTF8.GetBytes(data));
            Console.WriteLine($" [PDF Service] Done!!");
        };

        // Start consuming
        await channel.BasicConsumeAsync(queue: queueName,
                             autoAck: true,
                             consumer: consumer);

        Console.ReadLine();
    }

    public static async Task<string> GetAccountStatement()
    {
        Console.WriteLine("Getting account statement from gRPC server");
        var grpcServiceUrl = "http://grpcserver:5001";

        using var channel = GrpcChannel.ForAddress(grpcServiceUrl);
        var client = new Account.AccountClient(channel);

        var request = new AccountStamenetRequest
        {
            PublishEventToRabbitMQ = false
        };

        var response = await client.GetAccountStatementAsync(request);

        Console.WriteLine("GRPC request successful!");

        return JsonConvert.SerializeObject(response);
    }
}
