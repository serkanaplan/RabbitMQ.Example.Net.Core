
using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory
{
    // Uri = new Uri("amqps://fkucsvcj:LqAqt0v5ApdauHz4EbhyT-mizaXEKoEI@cow.rmq2.cloudamqp.com/fkucsvcj")
    HostName = "localhost"
};

using var connection = factory.CreateConnection();//Bu metod, RabbitMQ sunucusuna bir bağlantı oluşturur.
using var channel = connection.CreateModel();//Bu metod, mesaj göndermek veya almak için kullanılacak bir kanal oluşturur.

// queue: Kuyruğun adı. Bu örnekte "hello-queue".
// durable: Kuyruğun kalıcı olup olmadığını belirtir.
    // true: Kuyruk kalıcı olur, yani RabbitMQ sunucusu yeniden başlatıldığında bile varlığını korur.
    // false: Kuyruk geçici olur, yani RabbitMQ sunucusu yeniden başlatıldığında kaybolur.
// exclusive: Kuyruğun özel olup olmadığını belirtir.
//     true: Kuyruk sadece bu bağlantı tarafından kullanılabilir ve bu bağlantı kapandığında kuyruk otomatik olarak silinir.
//     false: Kuyruk diğer bağlantılar tarafından da kullanılabilir.
// autoDelete: Kuyruğun otomatik olarak silinip silinmeyeceğini belirtir.

// true: Kuyruk, onu dinleyen son abone bağlantısını kapattığında otomatik olarak silinir.
//     false: Kuyruk, onu dinleyen son abone bağlantısını kapattığında bile silinmez.
//     arguments: Ek kuyruk argümanları. Özel özellikler ayarlamak için kullanılabilir. Bu örnekte null olarak ayarlanmış, yani ek bir özellik yok.
channel.QueueDeclare(queue: "hello-queue", false, false, false, arguments: null);

Enumerable.Range(1, 50).ToList().ForEach(x =>
{
    string message = $"Message {x}";

    var messageBody = Encoding.UTF8.GetBytes(message);//Mesajı byte dizisine çevirir.

    // Mesajı RabbitMQ kuyruğuna gönderir. Burada: exchange: Mesajın hangi exchange'e gönderileceğini belirtir. Boş string(default exchange), direkt olarak kuyruğa gönderileceğini ifade eder.
    // routingKey: Mesajın gönderileceği kuyruk adı. basicProperties: Mesajın özelliklerini belirtir. Bu örnekte null bırakılmış.
    // body: Gönderilecek mesajın kendisi (byte dizisi olarak).
    channel.BasicPublish(exchange: "", routingKey: "hello-queue", basicProperties: null, body: messageBody);

    Console.WriteLine($"Mesaj Gonderildi. Gönderilen mesaj: {message}");
});

Console.ReadLine();