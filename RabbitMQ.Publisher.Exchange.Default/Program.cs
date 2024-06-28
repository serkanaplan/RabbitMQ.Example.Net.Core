//default exchange'de sadece kuyruk var.
using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory
{
    // Uri = new Uri("amqps://fkucsvcj:LqAqt0v5ApdauHz4EbhyT-mizaXEKoEI@cow.rmq2.cloudamqp.com/fkucsvcj")
    HostName = "localhost"
};

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

// durable : true ise mesaj kalıcı(diskte) false ise geçicidir(memoryde)
// exclusive : true ise başka kanallardan bağlanılabilir
// autoDelete : false ise kuyruğu dinleyen subscriber down olursa silinmez tekrar ayağa kalktığında kaldığı yerden göndermeye devam eder
channel.QueueDeclare(queue: "hello-queue",durable: false,exclusive: false,autoDelete: false, arguments: null);


Enumerable.Range(1, 50).ToList().ForEach(x =>
{
    string message = $"Message {x}";

    var messageBody = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(exchange: "", routingKey: "hello-queue", basicProperties: null, body: messageBody);

    Console.WriteLine($"Mesaj Gonderildi. Gönderilen mesaj: {message}");
});

Console.ReadLine();