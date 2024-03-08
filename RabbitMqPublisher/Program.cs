using RabbitMQ.Client;
using System.Text;

//Bağlantı Oluşturma
ConnectionFactory factory = new();
factory.Uri = new("amqps://wchpyyvv:xFLMWyo7qrUU2fBHE7wNgq1_L9nzrWUF@toad.rmq.cloudamqp.com/wchpyyvv");

//Bağlantıyı Aktifleştirme ve Kanal Açma
//using kullanıyoruz çünkü bellekte yer etmesin.
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

#region default
////Queue Oluşturma
////durable parametresi mesajların kuyrukta ne kadar kalacağını belirtir.
////exclusive parametresi kuyruğun özel olup olmadığı. Birden fazla kuyruk ile özel olarak işlem yapıp yapamayacağımızı belirtir.
////autoDelete tüm mesajların tüketildiğinde kuyruğun silinip silinemeyeceğine dair yapılanmadır.
////durable: true kuyruğun kalıcılığı için konfigürasyon (message durability)
//channel.QueueDeclare(queue: "example-queue", exclusive: false, durable: true);

////Queue'ya Mesaj Gönderme
//////Kuyruğa atılan mesajlar byte türünden olmalı. RabbitMq
////byte[] message = Encoding.UTF8.GetBytes("Merhaba");

//////exchange boş bırakırsak default exhange olur. yani direct exchange
////channel.BasicPublish(exchange:"", routingKey:"example-queue",body: message);

////mesajın kalıcılığı için (message durability)
//IBasicProperties properties = channel.CreateBasicProperties();
//properties.Persistent = true;

//for (int i = 0; i < 10; i++)
//{
//    await Task.Delay(200);
//    byte[] message = Encoding.UTF8.GetBytes("Merhaba " + i);
//    channel.BasicPublish(exchange: "", routingKey: "example-queue", body: message, basicProperties: properties);
//}
#endregion

#region DirectExchange

//channel.ExchangeDeclare(exchange: "direct-exchange-example", type: ExchangeType.Direct);

//while (true)
//{
//    Console.Write("Mesaj : ");
//    string message = Console.ReadLine();
//    byte[] byteMessage = Encoding.UTF8.GetBytes(message);

//    channel.BasicPublish(exchange: "direct-exchange-example",
//                         routingKey:"direct-queue-example",
//                         body: byteMessage
//        );
//}

#endregion

#region FanoutExchange

//channel.ExchangeDeclare(exchange: "fanout-exchange-example", type: ExchangeType.Fanout);

//for (int i = 0; i < 50; i++)
//{
//    await Task.Delay(200);

//    byte[] message = Encoding.UTF8.GetBytes($"Merhaba {i} " );
//    channel.BasicPublish(
//        exchange: "fanout-exchange-example",
//        routingKey: string.Empty,
//        body: message);
//}

#endregion

#region TopicExchange

channel.ExchangeDeclare(
    exchange: "topic-exchange-example",
    type: ExchangeType.Topic
    );

for (int i = 0; i < 100; i++)
{
    await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes($"Merhaba {i} ");
    Console.WriteLine("Mesajın Gönderileceği Topic Formatını Giriniz " );
    string topic = Console.ReadLine();

    channel.BasicPublish(
        exchange: "topic-exchange-example",
        routingKey: topic,
        body: message
        );
}
#endregion

Console.Read();