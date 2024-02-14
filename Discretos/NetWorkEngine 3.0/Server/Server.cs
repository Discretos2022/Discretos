using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using static Plateform_2D_v9.NetWorkEngine_3._0.Client.Client;
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
        private static UdpClient udpListener;

        public static IPEndPoint localEP;

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

            localEP = new IPEndPoint(IPAddress.Any, 7777);
            udpListener = new UdpClient(7777);

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
                        clients[numOfClient].RecepterTCP();
                        udpListener.BeginReceive(ReceiveCallback, null);
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


        private static void SendTCP(string data, int clientID)
        {
            clients[clientID].SendTCP(data);
        }

        private static void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                IPEndPoint sender = null;
                byte[] _data = udpListener.EndReceive(_result, ref sender);
                udpListener.BeginReceive(ReceiveCallback, null);
                string msg = Encoding.UTF8.GetString(_data);

                ServerReader.UDP.ReadPacket(msg, sender);

            }
            catch (Exception)
            {
                //Disconnect();
            }
        }

        public static void SendUDP(string data, int playerID)
        {
            try
            {
                if (udpListener != null)
                {

                    byte[] bytes = Encoding.UTF8.GetBytes(DateTime.Now.ToString("yyyy:HH:mm:ss.ff") + "-" + data);
                    IPEndPoint ip = new IPEndPoint(clients[playerID - 1].endPoint.Address, 7777 + 1);

                    //Console.WriteLine("PACKETS SEND TO : " + ip);

                    udpListener.BeginSend(bytes, bytes.Length, ip, null, null);
                }

            }
            catch (Exception _ex)
            {
                Console.WriteLine($"Error sending data to server via UDP: {_ex}");
            }
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
            //Handler.AddPlayerV2(ID);
            SendID(ID, ID);
            if (ID == 2)
                SendOtherPlayer(2, 1);
            else if (ID == 3)
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

            Console.WriteLine("");
            for (int i = 0; i < clients.Length; i++)
            {
                Console.WriteLine(i + " : " + clients[i]);
            }

        }

        public static void RemovePlayer(int ID)
        {
            Console.WriteLine($"[SERVER] Player {ID} was disconnected !");

            clients[ID - 1] = null;

            Console.WriteLine("");
            for (int i = 0; i < clients.Length; i++)
            {
                Console.WriteLine(i + " : " + clients[i]);
            }

            for (int i = 0; i < clients.Length; i++)
            {
                int newID = i + 1;
                if (i + 1 >= ID)
                    newID = i;

                if (clients[i] != null)
                    SendDisconnectedPlayer(ID, newID, i + 1);

                if (i + 1 > ID)
                    clients[i - 1] = clients[i];

                

            }

            Console.WriteLine("");
            for (int i = 0; i < clients.Length; i++)
            {
                Console.WriteLine(i + " : " + clients[i]);
            }

        }


        #region Tcp

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

            SendTCP(segment, clientID-1);

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

        public static void SendDisconnectedPlayer(int disconnectedPlayer, int newID, int clientID)
        {
            SendPacket(PacketType.disconnectedPlayer, disconnectedPlayer.ToString() + "/" + newID.ToString(), clientID);
        }

        public static void SendLevelStated(int level)
        {
            for (int i = 2; i <= numOfClient; i++)
                SendPacket(PacketType.levelStated, level + "", i);
        }

        #endregion

        #region Udp

        private static void SendUDPPacket(PacketType type, string data, int clientID)
        {

            string segment = "";
            int packetID = (int)type;

            if (packetID < 10) segment += "000";
            else if (packetID < 100) segment += "00";
            else if (packetID < 1000) segment += "0";

            segment += packetID;
            segment += ":";
            segment += (int)playerID;
            segment += ":";
            segment += data;

            SendUDP(segment, clientID);

        }

        public static void SendTest(int clientID)
        {
            if (clients[clientID - 1] != null)
                SendUDPPacket(PacketType.None, "Salut !", clientID);
        }

        public static void SendWorldMapPositionPlayer(int x, int y)
        {
            for (int i = 2; i <= numOfClient; i++)
            {
                //Console.WriteLine("SEND POS ! " + clients[i - 1].endPoint);
                SendUDPPacket(PacketType.otherPlayerWorldMapPosition, x.ToString() + "/" + y.ToString(), i);
            }

        }

        #endregion

    }
}
