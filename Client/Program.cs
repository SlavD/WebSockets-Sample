using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Websocket.Client;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var exitEvent = new ManualResetEvent(false);
            var url = new Uri("ws://localhost:5000/ws");

            using (var client = new WebsocketClient(url))
            {
                client.ReconnectTimeout = TimeSpan.FromSeconds(30);
                client.ReconnectionHappened.Subscribe(info =>
                    Console.WriteLine($"Reconnection happened, type: {info.Type}"));

                client.MessageReceived.Subscribe(msg => Console.WriteLine($"Message received: {msg}"));
                client.Start();

                Task.Run(() => client.Send("test echo 123"));

                exitEvent.WaitOne();
            }

        }
    }
}
