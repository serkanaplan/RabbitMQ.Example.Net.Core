
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory { HostName = "localhost" };

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();


channel.BasicQos(0, 10, false); 

var consumer = new EventingBasicConsumer(channel);

channel.BasicConsume(queue: "direct-queue-Critical", autoAck: false, consumer: consumer);

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