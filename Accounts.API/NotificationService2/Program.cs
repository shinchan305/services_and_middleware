using Accounts.CrossCutting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Constants = Accounts.CrossCutting.Constants;

class Program
{
    static async Task Main(string[] args)
    {
        // Create a connection to the RabbitMQ server
        var factory = Utility.GetRabbitMQConnectionFactory();
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        // Declare a topic & fanout exchanges to consume events from both services
        await channel.ExchangeDeclareAsync(exchange: Constants.ACCOUNT_CREATION_EXCHANGE_NAME, type: ExchangeType.Topic);
        await channel.ExchangeDeclareAsync(exchange: Constants.PDF_CREATED_EXCHANGE_NAME, type: ExchangeType.Fanout);

        // Create a unique queue for the consumer
        var queueResult = await channel.QueueDeclareAsync();
        string queueName = queueResult.QueueName;

        // Bind the queue to the Fanout Exchange
        await channel.QueueBindAsync(queue: queueName, exchange: Constants.PDF_CREATED_EXCHANGE_NAME, routingKey: "");

        // Bind the queue to the Topic Exchange with a specific binding key
        await channel.QueueBindAsync(queue: queueName,
                          exchange: Constants.ACCOUNT_CREATION_EXCHANGE_NAME,
                          routingKey: Constants.ACCOUNT_CREATION_ROUTING_KEY);

        Console.WriteLine($" [Notification Service 2] Waiting for messages matching '{Constants.ACCOUNT_CREATION_ROUTING_KEY}'. Press [Enter] to exit.");
        Console.WriteLine($" [Notification Service 2] Waiting for messages from '{Constants.PDF_CREATED_EXCHANGE_NAME}'. Press [Enter] to exit.");

        // Define a consumer to process messages
        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var routingKey = ea.RoutingKey;
            var exchange = ea.Exchange;
            Console.WriteLine($" [x] Received from Exchange '{exchange}' with Routing Key '{routingKey}': {message}");
        };

        // Start consuming
        await channel.BasicConsumeAsync(queue: queueName,
                             autoAck: true,
                             consumer: consumer);

        Console.ReadLine();
    }
}
