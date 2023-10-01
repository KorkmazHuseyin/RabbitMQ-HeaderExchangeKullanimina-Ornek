using RabbitMQ.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RAbbitMQ.Publisher
{
    public enum LogNames
    {
        Critical = 1,
        Error = 2,
        Warning = 3,
        Info = 4
    }




    class Program
    {
        static void Main(string[] args)
        {

            var factory = new ConnectionFactory();

            factory.Uri = new Uri("amqps://vxdvxesj:VT6vpW9ieHhgHroI29tDgiM2yqHaXezh@toad.rmq.cloudamqp.com/vxdvxesj");



            using (var connnection = factory.CreateConnection())
            {


                var channel = connnection.CreateModel();

                //Bir önceki örnekte queue oluşturmuştuk. Bu sefer sadece Exchange oluşturuyorum.
                channel.ExchangeDeclare("header-Exchange", durable: true, type: ExchangeType.Headers);
                Dictionary<string, object> headers = new Dictionary<string, object>();
                headers.Add("format", "pdf");
                headers.Add("shape", "a4");

                var properties = channel.CreateBasicProperties();
                properties.Headers = headers;

                channel.BasicPublish("header-Exchange", string.Empty, properties, Encoding.UTF8.GetBytes(" Bu bir Header Mesajıdır"));

                Console.WriteLine("Mesaj Gönderilmiştir.......");

                Console.ReadKey();
            }
        }
    }

}
