using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Plateform_2D_v9.NetWorkEngine_3._0.Server
{
    class Server
    {

        public static int MaxPlayers { get; private set; }
        public static int Port { get; private set; }

        public static ServerClient[] clients;

        public static int numOfClient = 1;

        private static TcpListener tcpListener;
        private static UdpClient udpListener;

        /// <summary>
        /// The port max is 65535
        /// </summary>
        /// <param name="_maxPlayers"></param>
        /// <param name="_port"></param>
        public static async void Start(int _maxPlayers, int _port)
        {
            MaxPlayers = _maxPlayers;
            Port = _port;

            clients = new ServerClient[_maxPlayers];

            Console.WriteLine("Starting server...");

            tcpListener = new TcpListener(IPAddress.Any, _port);

            /// TODO : Print IP
            Console.WriteLine($"Server started on {Port}.");


            try
            {

                while (true)
                {
                    tcpListener.Start();

                    if(numOfClient < MaxPlayers)
                    {
                        clients[numOfClient] = new ServerClient(await tcpListener.AcceptTcpClientAsync());
                        Console.WriteLine("[SERVER] Une connection accepté : " + clients[numOfClient].GetIP());
                        clients[numOfClient].Recepter();
                        numOfClient += 1;
                    }
                    tcpListener.Stop();
                        
                }

            }
            catch(IOException e)
            {

            }

        }

    }
}
