using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Accounts.CrossCutting
{
    public static class Utility
    {
        // Return a 10 digit account number in string format
        public static string GenerateAccountNumber()
        {
            Random random = new Random();
            return random.Next(100000000, 999999999).ToString();
        }

        /// <summary>
        /// Writes data to a file
        /// </summary>
        /// <param name="fileContent"></param>
        /// <param name="fileName"></param>
        public static void WriteDataToFile(string fileContent, string fileName)
        {
            string basePath = AppContext.BaseDirectory;
            string filePath = Path.Combine(basePath, "Data", fileName);
            File.WriteAllText(filePath, fileContent);
        }

        /// <summary>
        /// Reads data from a file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static T ReadDataFromFile<T>(string fileName)
        {
            string basePath = AppContext.BaseDirectory;
            string filePath = Path.Combine(basePath, "Data", fileName);
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath));
        }

        public static async Task PublishEventToRabbitMQ(string message, string exchangeName, string routingKey, string exchangeType = ExchangeType.Topic)
        {
            var factory = new ConnectionFactory
            {
                HostName = "my-rabbit",
                UserName = "user",
                Password = "password"
            };
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();
            await channel.ExchangeDeclareAsync(exchangeName, exchangeType);

            await channel.BasicPublishAsync(exchangeName, routingKey, Encoding.UTF8.GetBytes(message));
        }
    }
}
