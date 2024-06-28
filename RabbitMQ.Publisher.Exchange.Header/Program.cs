// Direct Exchange: Belirli bir routing key'e sahip mesajları uygun kuyruğa yönlendirir.
// 50 mesaj varsa toplam 50 mesaj olacak şekilde kuyruklara böler

using System.ComponentModel;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Publisher.Exchange.Header;


var factory = new ConnectionFactory { HostName = "localhost" };

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "hello-exchange-header", type: ExchangeType.Headers, durable: true, autoDelete: false);

Dictionary<string, object> headers = [];
headers.Add("format", "pdf");
headers.Add("shape", "a4");

var properties = channel.CreateBasicProperties();
properties.Headers = headers;
properties.Persistent = true;// mesajın kalıcı olmasını sağlar

var product = new Product { Id = 1, Name = "Samsung", Category = "Phone", Price = 100 };

var productJsonString =JsonSerializer.Serialize(product);

channel.BasicPublish(exchange: "hello-exchange-header", routingKey: "", basicProperties: properties, body: Encoding.UTF8.GetBytes(productJsonString));


Console.WriteLine("Mesaj Gonderildi.");

Console.ReadLine();
