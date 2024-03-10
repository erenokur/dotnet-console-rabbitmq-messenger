using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;


var factory = new ConnectionFactory()
{
    HostName = "localhost",
    UserName = "guest",
    Password = "guest"
};

using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    channel.QueueDeclare(queue: "rabbitmq-queue",
                         durable: false,
                         exclusive: false,
                         autoDelete: false,
                         arguments: null);

    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (model, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine(" [x] Received: {0}", message);

        if (message == "exit")
        {
            Console.WriteLine("Exit message received. Exiting...");
            Environment.Exit(0);
        }
    };

    channel.BasicConsume(queue: "rabbitmq-queue",
                         autoAck: true,
                         consumer: consumer);

    connection.ConnectionShutdown += (sender, ea) =>
    {
        Console.WriteLine("Connection to RabbitMQ server lost. Exiting...");
        Environment.Exit(0);
    };

    Console.WriteLine(" Press [enter] to exit.");
    Console.ReadLine();
}