
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

try
{
    // This is the Producer who sends the message to the message broker

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

    // Send an example message to the queue
    var notifyMessage = new { 
        Product = "Payment", 
        PaymentId = "1000200201", 
        Amount="23.50", 
        Currency="ARS", 
        Status = "approved", 
        Message = "You have a new Payment in your account." 
    };

    var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(notifyMessage));

    // We send the message to the exchange (Default exchange)
    while (true)
    {
        channel.BasicPublish("", queueName, null, body);
        Console.WriteLine(JsonConvert.SerializeObject(notifyMessage));
    }
}
catch (Exception ex)
{
    Console.WriteLine("Exception message: " + ex.Message);
    Console.ReadLine();
}