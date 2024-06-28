// Fanout Exchange: mesajları tüm bağlı kuyruklara yönlendirir. Yönlendirme anahtarı kullanılmaz.
// 50 mesaj varsa her birine 50 mesaj gider
using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory
{
    // Uri = new Uri("amqps://fkucsvcj:LqAqt0v5ApdauHz4EbhyT-mizaXEKoEI@cow.rmq2.cloudamqp.com/fkucsvcj")
    HostName = "localhost"
};

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "hello-exchange-fanout", type: ExchangeType.Fanout, durable: true, autoDelete: false);



Enumerable.Range(1, 50).ToList().ForEach(x =>
{
    string message = $"Message {x}";

    var messageBody = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(exchange: "hello-exchange-fanout", routingKey: "hello-queue", basicProperties: null, body: messageBody);

    Console.WriteLine($"Mesaj Gonderildi. Gönderilen mesaj: {message}");
});

Console.ReadLine();