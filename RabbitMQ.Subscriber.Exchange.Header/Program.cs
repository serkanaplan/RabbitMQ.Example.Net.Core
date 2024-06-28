
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Publisher.Exchange.Header;

var factory = new ConnectionFactory { HostName = "localhost" };

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();


channel.BasicQos(0, 10, false);

var consumer = new EventingBasicConsumer(channel);

var queueName = channel.QueueDeclare().QueueName;

Dictionary<string, object> headers = [];
headers.Add("format", "pdf");
headers.Add("shape", "a4");
headers.Add("x-match", "all");// publisher ve subscribe key value çiftlerinin uyumlu olması lazım yani publisher da shape subscribe da shabe2 şeklinde tutarsızlık varsa diğer key value çifterine bakılmaz ve mesaj alınmaz
// headers.Add("x-match", "any");// publisher ve subscribe key value çiftlerinin en az bir tanesi uyumlu olsa bile mesajı alır

channel.QueueBind(queue: queueName,exchange: "hello-exchange-header", routingKey: "", arguments: headers);

channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

Console.WriteLine("Gelen Mesajlar: ");

consumer.Received += (sender, e) =>
{
    var message = Encoding.UTF8.GetString(e.Body.ToArray());

    Product product = JsonSerializer.Deserialize<Product>(message)!;

    Thread.Sleep(1000);
    Console.WriteLine($"Gelen Mesaj: {product.Id} - {product.Name} - {product.Category} - {product.Price}");
    channel.BasicAck(e.DeliveryTag, false);
};

Console.ReadLine();