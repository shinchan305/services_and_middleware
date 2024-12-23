using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Constants = Notification.CrossCutting.Constants;
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
        string exchangeName = Constants.ACCOUNT_CREATION_EXCHANGE_NAME;
        await channel.ExchangeDeclareAsync(exchange: exchangeName, type: ExchangeType.Topic);

        // Create a unique queue for the consumer
        var queueResult = await channel.QueueDeclareAsync();
        string queueName = queueResult.QueueName;

        // Bind the queue to the exchange with a binding key
        string bindingKey = Constants.ACCOUNT_CREATION_ROUTING_KEY;
        await channel.QueueBindAsync(queue: queueName,
                          exchange: exchangeName,
                          routingKey: bindingKey);

        Console.WriteLine($" [Notification Service 1] Waiting for messages matching '{bindingKey}'. Press [Enter] to exit.");

        // Define a consumer to process messages
        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var routingKey = ea.RoutingKey;
            var exchange = ea.Exchange;
            Console.WriteLine($" [Notification Service 1] Received from Exchange '{exchange}' with Routing Key '{routingKey}': {message}");
        };

        // Start consuming
        await channel.BasicConsumeAsync(queue: queueName,
                             autoAck: true,
                             consumer: consumer);

        Console.ReadLine();
    }
}
