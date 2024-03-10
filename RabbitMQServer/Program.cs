using System;
using RabbitMQ.Client;
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

    while (true)
    {
        Console.Write("Enter a message (or 'exit' to quit): ");
        string message = Console.ReadLine();

        if (message.ToLower() == "exit")
        {
            break;
        }

        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: "",
                             routingKey: "rabbitmq-queue",
                             basicProperties: null,
                             body: body);

        Console.WriteLine(" [x] Sent {0}", message);
    }
}