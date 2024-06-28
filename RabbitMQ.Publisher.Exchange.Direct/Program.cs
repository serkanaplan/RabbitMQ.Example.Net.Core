// Direct Exchange: Belirli bir routing key'e sahip mesajları uygun kuyruğa yönlendirir.
// 50 mesaj varsa toplam 50 mesaj olacak şekilde kuyruklara böler

using System.Text;
using RabbitMQ.Client;

namespace RabbitMQ.Publisher.Exchange.Direct;
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

        channel.ExchangeDeclare(exchange: "hello-exchange-direct", type: ExchangeType.Direct, durable: true, autoDelete: false);


        Enum.GetNames(typeof(LogNames)).ToList().ForEach(x =>
        {
            var routeKey = $"route-{x}";
            var queueName=$"direct-queue-{x}";
            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null); 

            channel.QueueBind(queue: queueName, exchange: "hello-exchange-direct", routingKey: routeKey);  
        }); 

        Enumerable.Range(1, 50).ToList().ForEach(x =>
        {
            LogNames log = (LogNames)new Random().Next(1, 5);

            string message = $"log-type {log}";
  
            var messageBody = Encoding.UTF8.GetBytes(message);

            var routeKey = $"route-{log}";
            
            channel.BasicPublish(exchange: "hello-exchange-direct", routingKey: routeKey, basicProperties: null, body: messageBody);

            Console.WriteLine($"Log Gonderildi. Gönderilen mesaj: {message}");
        });

        Console.ReadLine();
    }
}