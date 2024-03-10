using System;
using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory()
{
    HostName = "localhost",
    UserName = "guest",
    Password = "guest"
};

bool running = true;
using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    channel.QueueDeclare(queue: "rabbitmq-queue",
                         durable: false,
                         exclusive: false,
                         autoDelete: false,
                         arguments: null);

    while (running)
    {
        Console.Write("Enter a message (or 'exit' to quit): ");
        string message = Console.ReadLine();

        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: "",
                             routingKey: "rabbitmq-queue",
                             basicProperties: null,
                             body: body);

        Console.WriteLine(" [x] Sent: {0}", message);

        if (message.ToLower() == "exit")
        {
            break;
        }
    }

    connection.ConnectionShutdown += (sender, ea) =>
    {
        Console.WriteLine("Connection to RabbitMQ server lost. Exiting...");
        running = false;
        Environment.Exit(0);
    };
}