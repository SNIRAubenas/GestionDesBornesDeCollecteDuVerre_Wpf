using System;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;

class Program
{
    static async Task Main(string[] args)
    {
        var factory = new MqttClientFactory();
        var client = factory.CreateMqttClient();

        var options = new MqttClientOptionsBuilder()
            .WithTcpServer("localhost", 1883)
            .Build();

        client.ApplicationMessageReceivedAsync += e =>
        {
            var payload = e.ApplicationMessage.Payload;

            if (!payload.IsEmpty)
            {
                var message = Encoding.UTF8.GetString(payload);
                Console.WriteLine("Message reçu : " + message);
            }

            return Task.CompletedTask;
        };

        await client.ConnectAsync(options);
        Console.WriteLine("Connecté au broker MQTT");

        await client.SubscribeAsync("capteur/niveau");
        Console.WriteLine("Abonné au topic capteur/niveau");

        Console.WriteLine("Appuyez sur une touche pour quitter...");
        Console.ReadKey();

        await client.DisconnectAsync();
    }
}