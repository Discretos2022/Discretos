using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Plateform_2D_v9.NetWorkEngine
{
    class Server1
    {

        public static int MaxPlayers;
        public static int Port;
        public static string IP = "192.168.1.14";
        public static TcpListener tcpListener;
        public static Dictionary<int, ServerClient1> players = new Dictionary<int, ServerClient1>();

        public static void StartServer(int maxPlayers, int port)
        {
            MaxPlayers = maxPlayers;
            Port = port;

            IPAddress address = IPAddress.Parse(IP);

            Console.WriteLine("Starting server...");
            InitializeServerData();

            tcpListener = new TcpListener(address, Port);
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);

            //tcpListener.Start(MaxPlayer);

        }

        private static void TCPConnectCallback(IAsyncResult _result)
        {
            TcpClient _client = tcpListener.EndAcceptTcpClient(_result);
            tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);
            Console.WriteLine($"Incoming connection from {_client.Client.RemoteEndPoint}...");

            for (int i = 1; i <= MaxPlayers; i++)
            {
                if (players[i].tcp.socket == null)
                {
                    players[i].tcp.Connect(_client);
                    return;
                }
            }

            Console.WriteLine($"{_client.Client.RemoteEndPoint} failed to connect: Server full!");
        }

        private static void InitializeServerData()
        {
            for (int i = 1; i <= MaxPlayers; i++)
            {
                players.Add(i, new ServerClient1(i));
            }
        }

    }
}
