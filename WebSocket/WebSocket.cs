// using System;
// using System.Net.WebSockets;
// using System.Text;
// using System.Threading;
// using System.Threading.Tasks;

// partial class Program
// {
//     static async Task Main(string[] args)
//     {
//         var uri = new Uri("wss://192.168.10.3:9070/");
//         using var client = new ClientWebSocket();
//         await client.ConnectAsync(uri, CancellationToken.None);
//         Console.WriteLine("Connected to WebSocket server");

//         // Send a message
//         var message = Encoding.UTF8.GetBytes("{\"message\":\"Hello from C# client!\"}");
//         await client.SendAsync(new ArraySegment<byte>(message), WebSocketMessageType.Text, true, CancellationToken.None);

//         // Receive a message
//         var buffer = new byte[1024];
//         var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
//         Console.WriteLine("Received: " + Encoding.UTF8.GetString(buffer, 0, result.Count));

//         await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
//     }
// }

