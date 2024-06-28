// Direct Exchange: Belirli bir routing key'e sahip mesajları uygun kuyruğa yönlendirir.
// 50 mesaj varsa toplam 50 mesaj olacak şekilde kuyruklara böler

using System.Text;
using RabbitMQ.Client;

namespace RabbitMQ.Publisher.Exchange.Topic;
public enum LogNames
{
    Critical=1,
    Error,
    Warning,
    Info
}

public static class Program
{

    public static void Main(string[] args)
    {

        var factory = new ConnectionFactory { HostName = "localhost" };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.ExchangeDeclare(exchange: "hello-exchange-topic", type: ExchangeType.Topic, durable: true, autoDelete: false);


        Random rnd = new();
        Enumerable.Range(1, 50).ToList().ForEach(x =>
        {
            LogNames log1 = (LogNames)rnd.Next(1, 5);
            LogNames log2 = (LogNames)rnd.Next(1, 5);
            LogNames log3 = (LogNames)rnd.Next(1, 5);

            var routeKey = $"{log1}.{log2}.{log3}";
            string message = $"log-type {log1}-{log2}-{log3}";
  
            var messageBody = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "hello-exchange-topic", routingKey: routeKey, basicProperties: null, body: messageBody);

            Console.WriteLine($"Log Gonderildi. Gönderilen mesaj: {message}");
        });

        Console.ReadLine();
    }
}