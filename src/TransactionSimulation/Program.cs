using System.Net.Mqtt;
using System.Text;
using System.Text.Json;
using TransactionSimulation;

Console.WriteLine("Hello, World from .net6!");

var users = Enumerable.Range(1, 100).Select(x => 10000+x).ToList();
var merchants = Enumerable.Range(1,50).Select(x => 1000+x).ToList();

Random rndUser = new Random();
Random rndMerchant = new Random();
Random rndAmount = new Random();

var mqttHost = Environment.GetEnvironmentVariable("MQTT_HOST") ?? "localhost";
IMqttClient client = MqttClient.CreateAsync(mqttHost, 1883).Result;
var sessionState = client.ConnectAsync(new MqttClientCredentials(clientId: $"simulator")).Result;

while (true)
{
    try
    {   
        int user = users[rndUser.Next(1, 100)];  
        int merchant = merchants[rndMerchant.Next(1, 50)];
        Console.WriteLine($"Making a transaction at merchant: {merchant}");

        var eventJson = JsonSerializer.Serialize(new Transaction
        {
            TransactionId = Guid.NewGuid(),
            UserId = user, 
            MerchantId = merchant,
            Amount = rndAmount.Next(10, 50000),
            Timestamp = DateTime.Now
        });
        var message = new MqttApplicationMessage("transaction/new", Encoding.UTF8.GetBytes(eventJson));
        client.PublishAsync(message, MqttQualityOfService.AtMostOnce);
        Task.Run(() => Thread.Sleep(3000)).Wait();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error in simulation. ErroeMessage: {ex.Message}");
    }
}


//var serviceInstances = Enumerable.Range(1, 3).ToList();

// Parallel.ForEach(users, user =>{
//     var sessionState = client.ConnectAsync(new MqttClientCredentials(clientId: $"user{user}")).Result;
//     Console.WriteLine($"Session for user: {user} => {sessionState.GetHashCode().ToString()}");
    
// });

// Task.Run(() => Thread.Sleep(Timeout.Infinite)).Wait();
