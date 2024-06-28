
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

//publisher projesnde bu kuyruk varsa bu satırı silebilirsin ama subscriberdan önce publisher projesini çalıştırman gerek yoksa hello-queue diye bi kuyruk oluşmadığı için hata verecektir.
// Eğer kuyruk publisher da varsa onu bağlanır yoksa yeni oluşturur.
channel.QueueDeclare(queue: "hello-queue", false, false, false, arguments: null);

channel.BasicQos(0, 10, false);

var consumer = new EventingBasicConsumer(channel);
channel.BasicConsume(queue: "hello-queue", autoAck: false, consumer: consumer);

consumer.Received += (sender, e) => 
{
    var body = e.Body.ToArray();  
    var message = Encoding.UTF8.GetString(body);
    Thread.Sleep(1000);
    Console.WriteLine($"Gelen Mesaj: {message}");
    channel.BasicAck(e.DeliveryTag, false);
};

Console.ReadLine();