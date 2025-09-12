using Microsoft.AspNetCore.SignalR.Client;

var connection = new HubConnectionBuilder()
    .WithUrl("http://localhost:5000/chat")
    .WithAutomaticReconnect()
    .Build();

Console.WriteLine("Connecting...");

connection.On<string>("Receive", (message) =>
{
    Console.WriteLine($"Received message: {message}");
});

try
{
    await connection.StartAsync();
    Console.WriteLine("Connection started");
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}


while (true)
{
    var message = Console.ReadLine();
    await connection.InvokeAsync("Send", message);
}
