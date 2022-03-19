using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

try
{
    // This is the Consumer who gets the message from the message broker

    Console.WriteLine("Starting the process...");

    // protocol://user:password@ip:port
    var factory = new ConnectionFactory
    {
        Uri = new Uri("amqp://guest:guest@localhost:5672")
    };

    // We create the connection
    using var connection = factory.CreateConnection();

    // We create the channel
    using var channel = connection.CreateModel();

    // We set the queue name
    var queueName = "notify-queue";

    // We set the queue config
    channel.QueueDeclare(queueName,
        durable: true,
        exclusive: false,
        autoDelete: false,
        arguments: null);

    // We create the consumer

    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (sender, args) =>
    {
        var body = args.Body.ToArray(); 
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine(message);
    };

    // autoAck=true
    channel.BasicConsume(queueName, true, consumer);

    Console.WriteLine("Closing the process...");
    Console.ReadLine();
}
catch (Exception ex)
{
    Console.WriteLine("Exception message: " + ex.Message);
    Console.ReadLine();
}