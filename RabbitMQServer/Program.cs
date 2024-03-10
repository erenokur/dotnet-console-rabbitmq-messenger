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

    Console.Write("Enter a message (or 'exit' to quit): ");
    while (running)
    {
        string message = Console.ReadLine();

        channel.BasicPublish(exchange: "",
                             routingKey: "rabbitmq-queue",
                             basicProperties: null,
                             body: Encoding.UTF8.GetBytes(message));

        Console.WriteLine(" [x] Sent: {0}", message);

        if (message.ToLower() == "exit")
        {
            Console.WriteLine("Exit message received. Exiting...");
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