// RabbitMq için Mesaj gönderecek.
using RabbitMQ.Client;
using System.Text;

Console.WriteLine("Mesaj Gönderiliyor");

var factory = new ConnectionFactory();      //Factory Design Pattern kullanırız.
factory.Uri = new Uri("amqps://dxmtamsk:QTRueb-UJ67m5bfhAaAKHNs-joh1bTzd@moose.rmq.cloudamqp.com/dxmtamsk ");   //RabbitMq bağlantı linki.
using var connection = factory.CreateConnection();      //Bağlantı oluşturduk.

var channel = connection.CreateModel();     //haberleşme yapımızı kurduk

//channel.QueueDeclare("myqueue", durable: true, exclusive: false, autoDelete: false);

channel.ExchangeDeclare("message-fanout", ExchangeType.Direct, true, false, null);  //fanout exchane'e mesaj göndereceğiz.

channel.QueueDeclare("message-queue", true, false, false, null);

channel.QueueBind("message-queue", "message-direct", "message-route", null);

Enumerable.Range(1, 50).ToList().ForEach(x =>
{
    var message = $"Message {x}";

    var byteMessage = Encoding.UTF8.GetBytes(message);  //ingilizce dışında bir dille kuyruğa mesaj atacaksak UTF8 kullanmamız lazım.
    //channel.BasicPublish(string.Empty, "myqueue", null, byteMessage.ToArray());      //direkt kuyrğa mesaj gönderiyoruz.
    channel.BasicPublish("message-fanout", string.Empty, null, byteMessage.ToArray());
});

Console.WriteLine("Mesaj Gönderildi");