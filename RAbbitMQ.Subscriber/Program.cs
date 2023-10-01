using Microsoft.AspNet.SignalR.Infrastructure;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RAbbitMQ.Subscriber
{

    class Program
    {
        static void Main(string[] args)
        {


            var factory = new ConnectionFactory();

            factory.Uri = new Uri("amqps://vxdvxesj:VT6vpW9ieHhgHroI29tDgiM2yqHaXezh@toad.rmq.cloudamqp.com/vxdvxesj");
            using (var connnection = factory.CreateConnection())
            {

                var channel = connnection.CreateModel();
                channel.ExchangeDeclare("header-Exchange", durable: true, type: ExchangeType.Headers);


                channel.BasicQos(0, 1, false);

                var consumer = new EventingBasicConsumer(channel);

                // consumer kendien kuyruk oluşturacak.
                var queueName = channel.QueueDeclare().QueueName;
              
                Dictionary<string, object> headers = new Dictionary<string, object>();
                headers.Add("format", "pdf");
                headers.Add("shape", "a4");
                headers.Add("x-match", "any");//any : key yada value degerlerinden herhangi birinin olması yeter. Mesaj çalışır.   All : olsaydı. Hem string hem value nin 2 sininde varlığını arayacaktık.


                channel.QueueBind(queueName, "header-Exchange",string.Empty,headers);

                channel.BasicConsume(queueName, false, consumer);


                Console.WriteLine("Log alınıyor........");

                consumer.Received += (object sender, BasicDeliverEventArgs e) =>
                {
                    var message = Encoding.UTF8.GetString(e.Body.ToArray());
                    Console.WriteLine("Gelen Mesaj =======>" + message);
                   
                    channel.BasicAck(e.DeliveryTag, false);
                };

                Console.ReadKey();

            }

        }


    }
}

