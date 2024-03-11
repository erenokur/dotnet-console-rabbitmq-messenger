using NUnit.Framework;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;


[TestFixture]
public class ProgramTests
{
    [Test]
    public void TestSendMessageToRabbitMQ()
    {
        // Arrange
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };
        var message = "Test Message";
        bool messageReceived = false;

        // Act
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
                var receivedMessage = Encoding.UTF8.GetString(body);
                if (receivedMessage == message)
                {
                    messageReceived = true;
                }
            };

            channel.BasicConsume(queue: "rabbitmq-queue",
                                 autoAck: true,
                                 consumer: consumer);

            channel.BasicPublish(exchange: "",
                                 routingKey: "rabbitmq-queue",
                                 basicProperties: null,
                                 body: Encoding.UTF8.GetBytes(message));

            // Wait for the message to be received
            Thread.Sleep(1000);
        }

        // Assert
        Assert.That(messageReceived, "Message should have been received");
    }
}
