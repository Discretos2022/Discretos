using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;

namespace Plateform_2D_v9.NetWorkEngine_3._0.Server
{
    class Server
    {

        public static int MaxPlayers { get; private set; }
        public static int Port { get; private set; }

        public static ServerClient[] clients;

        public static int numOfClient = 1;

        private static TcpListener tcpListener;

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
                        clients[numOfClient] = new ServerClient(await tcpListener.AcceptTcpClientAsync(), numOfClient + 1);
                        Console.WriteLine("[SERVER] Une connection accepté : " + clients[numOfClient].GetIP());
                        AddPlayer(numOfClient + 1);
                        clients[numOfClient].Recepter();
                        numOfClient += 1;
                    }
                    tcpListener.Stop();
                }

            }
            catch(SocketException e)
            {
                //Console.WriteLine("[SERVER] Le serveur est fermé !");
            }

        }

        public static void Stop()
        {

            if (tcpListener != null)
            {

                for(int i = 0; i < clients.Length; i++)
                {

                    if (clients[i] != null)
                    {
                        if (clients[i] != null)
                        {
                            clients[i].Disconnect();
                            clients[i] = null;
                            numOfClient -= 1;
                        }

                    }

                }

                tcpListener.Stop();

                tcpListener = null;

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Server Shutdown ! \n");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Le server est déjà hors ligne ! \n");
                Console.ForegroundColor = ConsoleColor.White;
            }

        }

        public static bool IsLaunched()
        {
            if (tcpListener == null)
                return false;
            else
                return true;
        }





        ///*********************************    DISCRETOS    ********************************************///


        public static void AddPlayer(int ID)
        {
            Console.WriteLine("[SERVER] Un nouveau joueur avec l'ID : " + ID);
            Handler.AddPlayerV2(ID);
        }


    }
}
