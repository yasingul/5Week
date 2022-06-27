// RabbitMq için Mesajı alan kısımdır
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.WriteLine("Mesaj Dinleniyor");

var factory = new ConnectionFactory();      //Factory Design Pattern kullanırız.
factory.Uri = new Uri("amqps://dxmtamsk:QTRueb-UJ67m5bfhAaAKHNs-joh1bTzd@moose.rmq.cloudamqp.com/dxmtamsk ");   //RabbitMq bağlantı linki.
using var connection = factory.CreateConnection();      //Bağlantı oluşturduk.

var channel = connection.CreateModel();     //haberleşme yapımızı kurduk

var queueName = channel.QueueDeclare().QueueName;

channel.QueueBind(queueName, "message-fanout", String.Empty, null);

var consumer = new EventingBasicConsumer(channel);
//channel.BasicConsume("myqueue", false, consumer);
channel.BasicConsume(queueName, false, consumer);

//Mesaj kuyruk sistemimizin eventini uyguladık. Foreach yapmamıza gerek yok.
consumer.Received += (object sender, BasicDeliverEventArgs args) =>
{
    var message = Encoding.UTF8.GetString(args.Body.ToArray());
    Console.WriteLine(message);
    channel.BasicAck(args.DeliveryTag, true);   //Mesajlar ulaştıktan sonra silinsin istiyorsak bunu yapmamız lazım
    Thread.Sleep(2000);
};

Console.ReadKey();