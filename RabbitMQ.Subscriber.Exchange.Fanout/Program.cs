
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory
{
    // Uri = new Uri("amqps://fkucsvcj:LqAqt0v5ApdauHz4EbhyT-mizaXEKoEI@cow.rmq2.cloudamqp.com/fkucsvcj")
    HostName = "localhost"
};

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

//kuyruğu subscriber'da oluşturuldu.
var randomeQueueName = channel.QueueDeclare().QueueName;

// subscribe down olduğu zaman kuyruğun silinmemesini istersek yuarıdaki randomQueueName'i yoruma alıp aşağıdaki 2 satırı ekleyebiliriz
// var queueName = channel.QueueDeclare().QueueName;
// channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

channel.QueueBind(queue: randomeQueueName, exchange: "hello-exchange-fanout", routingKey: "");

channel.BasicQos(0, 10, false); 

var consumer = new EventingBasicConsumer(channel);

channel.BasicConsume(queue: randomeQueueName, autoAck: false, consumer: consumer);

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