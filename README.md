# dotnet-console-rabbitmq-messenger

This is a simple .NET Core console application that sends and receives messages from a RabbitMQ server.

## Running the application

First, you need to have a RabbitMQ server running. You can use the following command to run a RabbitMQ server using Docker:

```bash
docker run -d --name my-rabbit -p 5672:5672 -p 15672:15672 -e RABBITMQ_DEFAULT_USER=guest -e RABBITMQ_DEFAULT_PASS=guest rabbitmq:3-management
```

Then press F5 in Visual Studio to start Server/Client debugging configuration.
