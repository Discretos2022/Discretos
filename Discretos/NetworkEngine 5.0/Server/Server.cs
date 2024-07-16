using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkEngine_5._0.Server
{
    public static class Server
    {

        private static TcpListener tcpListener;
        private static UdpClient udpListener;

        public static List<Clients> clients;

        private static NetworkStream networkStream;
        private static StreamReader reader;
        private static StreamWriter writer;

        private static string publicIP = "127.0.0.1";

        public static bool stopRequest = false;

        private static ServerStatus status = ServerStatus.Offline;

        //private static int IDcount = 0;

        public static List<int> usedID = new List<int>();

        public static bool serverLog = true;
        public static bool udpLog = false;

        private static bool acceptConnection = true;

        public static async void Start(int port = 7777, int _maxClient = 1000, bool serverIsUser = true)
        {
            if (status == ServerStatus.Offline)
            {
                //WriteTitle();
                StartTCP(port, _maxClient);
                StartUDP(port);

                if (serverIsUser)
                {
                    while (GetStatus() == ServerStatus.Starting) await Task.Delay(1);
                    Client.Client.Connect(GetPrivateIP(), port);
                }
                else usedID.Add(0);



            }
            else
                print("Server is already started !", ConsoleColor.DarkRed);

        }


        private static async void StartTCP(int port = 7777, int _maxClient = 1000)
        {

            stopRequest = false;
            status = ServerStatus.Starting;

            tcpListener = new TcpListener(IPAddress.Any, port);

            //tcpListener.AllowNatTraversal(true);

            string privateIP = GetPrivateIP();


            #region SearchPublicIP

            try
            {
                String direction = "";
                HttpWebRequest request = HttpWebRequest.CreateHttp("http://checkip.dyndns.org/");
                using (WebResponse response = await request.GetResponseAsync())
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
                publicIP = direction;
            }
            catch (Exception ex)
            {
                publicIP = "127.0.0.1";
            }

            #endregion


            clients = new List<Clients>();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Le server a été lancé ! | Public IP : " + publicIP + " | Private IP : " + privateIP + " | Port : " + port + " | Max : " + _maxClient);
            Console.ForegroundColor = ConsoleColor.White;

            status = ServerStatus.Online;

            Console.WriteLine("");

            try
            {
                while (true)
                {

                    tcpListener.Start();

                    TcpClient cl = await tcpListener.AcceptTcpClientAsync();

                    int id = 0;
                    for (int i = 0; i < usedID.Count; i++)
                    {
                        if (!usedID.Contains(i))
                        {
                            id = i;
                            break;
                        }
                        id = usedID.Count;
                    }

                    Clients newC = new Clients(cl, id);
                    usedID.Add(id);

                    tcpListener.Stop();

                    if (clients.Count == _maxClient)
                    {
                        newC.writer.WriteLine("#FULL");
                        print("Une connection refusé !", ConsoleColor.Red);
                    }
                    else if (!acceptConnection)
                    {
                        newC.writer.WriteLine("#NO");
                        print("Une connection refusé !", ConsoleColor.Red);
                    }
                    else
                    {
                        newC.writer.WriteLine("#YES");
                        try
                        {
                            //var endPointUDP = newC.reader.ReadLine();
                            //Console.WriteLine($"{endPointUDP}");
                            //var ip = endPointUDP.Split(":")[0];
                            //var p = int.Parse(endPointUDP.Split(":")[1]);
                            //newC.udpEndPoint = new IPEndPoint(IPAddress.Parse(ip), p);

                            //newC.udpClient.Connect(newC.);

                            newC.writer.WriteLine("#" + newC.GetID());

                            newC.RecepterTCP();
                            clients.Add(newC);

                            if (newC.GetID() != 0)
                                print("Une connection établie : " + newC.GetIP() + " ID : " + newC.GetID(), ConsoleColor.DarkMagenta);

                            //IDcount += 1;

                        }
                        catch (IOException e)
                        {
                            print("Connection failed : UDP was failed !", ConsoleColor.Red);
                        }

                    }

                }

            }
            catch (SocketException e)
            {

            }





        }

        private static void StartUDP(int port = 7777)
        {
            udpListener = new UdpClient(port);
            RecepterUDP();
        }


        public static void StopServer()
        {

            //IDcount = 0;
            usedID.Clear();

            if (tcpListener != null)
            {
                stopRequest = true;

                if (networkStream != null)
                {
                    writer.Close();
                    reader.Close();
                    networkStream.Close();
                }

                tcpListener.Stop();
                tcpListener = null;

                udpListener.Close();
                udpListener = null;


                for (int i = 0; i < clients.Count; i++)
                    clients[i].Disconnect();

                clients.Clear();


                print("Server Shutdown ! \n", ConsoleColor.Yellow);
                status = ServerStatus.Offline;
            }
            else
            {
                print("Server is not online !\n", ConsoleColor.DarkRed);
            }

        }



        public static ServerStatus GetStatus()
        {
            return status;
        }

        public static List<Clients> GetClients()
        {
            return clients;
        }



        public static async void SendTCP(string data, int clientID = 0, int except = -1)
        {
            if (clientID != 0)
                await clients[clientID].writer.WriteLineAsync(data);
            else
            {
                for (int i = 1; i < clients.Count; i++)
                    if (i != except)
                        await clients[i].writer.WriteLineAsync(data);
            }
        }

        /**
         Change TCPSend()
         */
        public static async void SendUDP(string message, int clientID = 0, int except = -1)
        {

            byte[] bytes = Encoding.UTF8.GetBytes(message);

            if (clientID != 0)
            {
                if (clients[clientID].udpEndPoint != null)
                    await udpListener.SendAsync(bytes, bytes.Length, clients[clientID].udpEndPoint);
                else
                    SendTCP(message + " (only TCP)", clientID);
            }

            else
            {
                for (int i = 1; i < clients.Count; i++)
                {
                    if (clients[clientID].udpEndPoint != null)
                    {
                        if (i != except)
                            await udpListener.SendAsync(bytes, bytes.Length, clients[i].udpEndPoint);
                        
                    }
                    else
                        SendTCP(message + " (only TCP)", i);

                }
            }



        }

        public static async void RecepterUDP()
        {

            try
            {

                while (true)
                {

                    UdpReceiveResult result = await udpListener.ReceiveAsync();
                    byte[] bytes = result.Buffer;
                    string msg = Encoding.UTF8.GetString(bytes);

                    if (msg.Split(" ")[0] != "#CONNECTION")
                    {
                        if(udpLog)
                            print($"New UDP Message : " + msg, ConsoleColor.Cyan);
                        ServerReader.ReadUDPPacket(msg);
                    }

                    else
                    {
                        for (int i = 0; i < clients.Count; i++)
                        {
                            if (msg.Split(" ")[1] == clients[i].GetID().ToString())
                            {
                                clients[i].udpEndPoint = result.RemoteEndPoint;
                                if (i != 0)
                                    print($"User {clients[i].GetID()} UDP endPoint added : " + result.RemoteEndPoint.ToString(), ConsoleColor.DarkMagenta);
                            }
                        }
                    }

                }

            }
            catch (IOException e)
            {

            }
            catch (SocketException e)
            {

            }

        }


        private static async void SearchPublicIP()
        {
            try
            {
                String direction = "";
                HttpWebRequest request = HttpWebRequest.CreateHttp("http://checkip.dyndns.org/");
                using (WebResponse response = await request.GetResponseAsync())
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
                publicIP = direction;
            }
            catch (Exception ex)
            {
                publicIP = "127.0.0.1";
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


        public static string GetPublicIP()
        {
            return publicIP;
        }


        public static void SetAcceptConnection(bool accept)
        {
            acceptConnection = accept;
        }

        public static bool IsAcceptConnection() { return acceptConnection; }


        public static void print(string msg, ConsoleColor color, string log = "[SERVER]")
        {
            if (serverLog)
            {
                Console.ForegroundColor = color;
                Console.WriteLine($"{log} " + msg);
                Console.ForegroundColor = ConsoleColor.White;
            }

        }

        public enum ServerStatus
        {
            Offline = 0,
            Starting = 1,
            Online = 2,
        };


        public static void WriteTitle()
        {
            string title = "NetworkEngine 5.0  Copyright © 2024 SIEDEL Joshua \n";

            int[] table = { 1, 3, 9, 11, 10, 2, 14, 6, 12, 4, 5, 13 };

            int color = 0;
            int v = 1;
            for (int i = 0; i < title.Length; i++)
            {
                printColor(title[i].ToString(), (ConsoleColor)table[color]);
                color += v;
                if (color >= table.Length - 1 || color <= 0)
                    v *= -1;
            }

            Console.WriteLine("");
        }

        public static void printColor(string msg, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(msg);
            Console.ForegroundColor = ConsoleColor.White;
        }


    }
}