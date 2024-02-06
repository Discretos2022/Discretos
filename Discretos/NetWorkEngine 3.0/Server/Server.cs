using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using static Plateform_2D_v9.NetWorkEngine_3._0.NetPlay;

namespace Plateform_2D_v9.NetWorkEngine_3._0.Server
{
    class Server
    {

        public static int MaxPlayers { get; private set; }
        public static int Port { get; private set; }

        public static ServerClient[] clients;

        public static int numOfClient = 0;

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

            // GetPublicIP()
            Console.WriteLine("Server started ! | Public IP : " + "***.***.***.***" + " | Private IP : " + GetPrivateIP());
            Console.WriteLine($"Server started on {Port}.");

            


            try
            {
                while (true)
                {

                    if(numOfClient < MaxPlayers)
                    {
                        tcpListener.Start();

                        if (numOfClient == 0) Client.Client.Connect(GetPrivateIP(), Port);

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


        private static async void Send(string data, int clientID)
        {
            await clients[clientID].writer.WriteLineAsync(data);
        }


        public static string GetPublicIP()
        {
            try
            {
                String direction = "";
                HttpWebRequest request = HttpWebRequest.CreateHttp("http://checkip.dyndns.org/");
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                    {
                        direction = stream.ReadToEnd();
                    }
                }
                //Search for the ip in the html
                int first = direction.IndexOf("Address: ") + 9;
                int last = direction.LastIndexOf("");
                direction = direction.Substring(first, last - first - 16);
                return direction;
            }
            catch (Exception ex)
            {
                return "127.0.0.1";
            }
        }

        public static string GetPrivateIP()
        {
            for (int i = 0; i < Dns.GetHostEntry(Dns.GetHostName()).AddressList.Length; i++)
            {
                if (Dns.GetHostEntry(Dns.GetHostName()).AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    return Dns.GetHostEntry(Dns.GetHostName()).AddressList[i].ToString();
            }

            return "???";

        }



        ///*********************************    DISCRETOS    ********************************************///


        public static void AddPlayer(int ID)
        {
            Console.WriteLine("[SERVER] Un nouveau joueur avec l'ID : " + ID);
            Handler.AddPlayerV2(ID);
            SendID(ID, ID);
            if (ID == 3)
            {
                SendOtherPlayer(3, 1);
                SendOtherPlayer(3, 2);
            }
            else if(ID == 4)
            {
                SendOtherPlayer(4, 1);
                SendOtherPlayer(4, 2);
                SendOtherPlayer(4, 3);
            }
                

        }


        private static void SendPacket(PacketType type, string data, int clientID)
        {

            string segment = "";
            int packetID = (int)type;

            if (packetID < 10) segment += "000";
            else if (packetID < 100) segment += "00";
            else if (packetID < 1000) segment += "0";

            segment += packetID;
            segment += ":";
            segment += data;

            Send(segment, clientID-1);

        }


        public static void SendID(int playerID, int clientID)
        {
            SendPacket(PacketType.playerID, playerID.ToString(), clientID);
        }

        public static void SendGameStarted()
        {

            for (int i = 1; i <= numOfClient; i++)
                SendPacket(PacketType.gameStarted, "", i);
        }

        public static void SendOtherPlayer(int newPlayer, int clientID)
        {
            SendPacket(PacketType.otherPlayerJoined, newPlayer.ToString(), clientID);
        }

    }
}
