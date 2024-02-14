using Plateform_2D_v9.NetWorkEngine_3._0.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using static Plateform_2D_v9.NetWorkEngine_3._0.NetPlay;
using static System.Net.Mime.MediaTypeNames;

namespace Plateform_2D_v9.NetWorkEngine_3._0.Client
{
    class Client
    {

        private static TcpClient client;
        private static UdpClient udpClient;

        private static IPEndPoint ip;

        private static NetworkStream stream;
        private static StreamWriter writer;
        private static StreamReader reader;
        private static bool isTimeOut = false;

        public static ClientState state = ClientState.Disconnected;

        public static PlayerID playerID = PlayerID.PLayerOne;

        public static string IPserver;
        public static int PortServer;

        public async static void Connect(string IP, int port = 7777)
        {

            try
            {

                IPserver = IP;
                PortServer = port;

                ip = new IPEndPoint(IPAddress.Parse(IPserver), PortServer);

                udpClient = new UdpClient(new IPEndPoint(IPAddress.Any, port + 1));
                udpClient.BeginReceive(ReceiveCallback, null);

                client = new TcpClient();
                isTimeOut = false;
                state = ClientState.Connecting;
                await client.ConnectAsync(IP, port);
                state = ClientState.Connected;

                NetPlay.IsMultiplaying = true;

                stream = client.GetStream();
                writer = new StreamWriter(stream, Encoding.UTF8);
                reader = new StreamReader(stream, Encoding.UTF8);
                writer.AutoFlush = true;

                while (true)
                {

                    try
                    {

                        string text = await reader.ReadLineAsync();

                        if (text != null)
                        {
                            /// TODO : Que faire du message ?
                            ClientReader.TCP.ReadPacket(text);
                        }
                        else
                        {
                            Disconnect("Server shutdown !");
                        }

                    }
                    catch (IOException e)
                    {
                        Disconnect("Connection lost !");
                    }
                    catch (ObjectDisposedException e)
                    {
                        Disconnect("Reader is null !");
                    }

                }

            }
            catch (SocketException e)
            {

                state = ClientState.Disconnected;
                isTimeOut = true;
                client = null;
                Console.WriteLine("Connection failed !");

            }

        }

        private static void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                IPEndPoint sender = null;
                byte[] _data = udpClient.EndReceive(_result, ref sender);
                udpClient.BeginReceive(ReceiveCallback, null);
                string msg = Encoding.UTF8.GetString(_data);

                ClientReader.UDP.ReadPacket(msg);

            }
            catch (Exception)
            {
                //Disconnect();
            }
        }

        public static void Disconnect(string error = "")
        {

            if(client != null)
            {
                stream.Close();
                writer.Close();
                reader.Close();
                client.Close();
                client.Dispose();
                client = null;

                udpClient.Dispose();
                udpClient = null;

                state = ClientState.Disconnected;

                Console.WriteLine(error);
            }

        }

        public static bool IsConnected()
        {
            if (client == null)
                return false;

            return client.Connected;
        }

        public static bool IsTimeOut()
        {
            return isTimeOut;
        }


        public enum ClientState
        {

            Disconnected = 0,
            Connected = 1,
            Connecting = 2,

        };

        public enum PlayerID
        {

            None = 0,
            PLayerOne = 1,
            PLayerTwo = 2,
            PlayerThree = 3,
            PlayerFour = 4,

        }

        private static async void Send(string data)
        {
            await writer.WriteLineAsync(data);
        }

        public static void SendUDP(string data)
        {
            try
            {
                if (udpClient != null)
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(DateTime.Now.ToString("yyyy:HH:mm:ss.ff") + "-" + data);
                    udpClient.BeginSend(bytes, bytes.Length, ip, null, null);
                }

            }
            catch (Exception _ex)
            {
                Console.WriteLine($"Error sending data to server via UDP: {_ex}");
            }
        }


        ///*********************************    DISCRETOS    ********************************************///

        #region Tcp

        private static void SendPacket(PacketType type, string data)
        {

            string segment = "";
            int packetID = (int)type;

            if (packetID < 10) segment += "000";
            else if (packetID < 100) segment += "00";
            else if (packetID < 1000) segment += "0";

            segment += packetID;
            segment += ":";
            segment += data;

            Send(segment);

        }

        #endregion

        #region Udp

        private static void SendUDPPacket(PacketType type, string data)
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

            SendUDP(segment);

        }

        public static void SendTest()
        {
            SendUDPPacket(PacketType.None, "12.12 vous me recevé ?");
        }

        public static void SendID()
        {
            SendUDPPacket(PacketType.firstMsgForPortPlayer, (int)playerID + "");
        }

        public static void SendWorldMapPosition(int x, int y)
        {
            SendUDPPacket(PacketType.playerOneWorldMapPosition, x.ToString() + "/" + y.ToString());
        }

        #endregion


    }
}
