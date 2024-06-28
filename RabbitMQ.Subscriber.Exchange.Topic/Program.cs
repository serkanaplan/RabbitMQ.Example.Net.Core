
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory { HostName = "localhost" };

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();


channel.BasicQos(0, 10, false);

var consumer = new EventingBasicConsumer(channel);

var queueName = channel.QueueDeclare().QueueName;
var routeKey = "*.Error.*";
// var routeKey2 = "*.*.Warning";
// var routeKey2 = "Info.#";

channel.QueueBind(queue: queueName, exchange: "hello-exchange-topic", routingKey: routeKey);

channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

Console.WriteLine("Gelen Mesajlar: ");

consumer.Received += (sender, e) =>
{
    var body = e.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Thread.Sleep(1000);
    Console.WriteLine($"Gelen Mesaj: {message}");
    channel.BasicAck(e.DeliveryTag, false);
};

Console.ReadLine();