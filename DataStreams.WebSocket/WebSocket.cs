using System;
using System.Collections.Generic;
using System.Configuration;
using Fleck;

namespace DataStreams.WebSocket
{
    public class WebSocket
    {
        private static readonly object LockObject = new object();
        private static IList<IWebSocketConnection> _clients = new List<IWebSocketConnection>();

        public static void Main(string[] args)
        {
            var server = new WebSocketServer(ConfigurationManager.AppSettings["serverAddress"]);
            server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    Console.WriteLine($"Client connected!");
                    lock (LockObject)
                    {
                        _clients.Add(socket);
                    }
                };

                socket.OnClose = () =>
                {
                    Console.WriteLine("Client removed!");
                    lock (LockObject)
                    {
                        _clients.Remove(socket);
                    }
                };

                socket.OnMessage = (message) =>
                {
                    Console.WriteLine("Message all clients");
                    lock (LockObject)
                    {
                        var clients = new List<IWebSocketConnection>();
                        foreach (var client in _clients)
                        {
                            if (client.IsAvailable)
                            {
                                client.Send(message);
                                clients.Add(client);
                                continue;
                            }

                            //remove those clients that are closed
                            client.Close();
                        }

                        _clients = clients;
                    }

                };
            });

            Console.WriteLine("Press any key to stop...\n");
            Console.ReadKey();
        }
    }
}
