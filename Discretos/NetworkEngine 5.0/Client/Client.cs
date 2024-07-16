using NetworkEngine_5._0.Error;
using NetworkEngine_5._0.Server;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkEngine_5._0.Client
{
    public static class Client
    {



        public static TcpClient client = new TcpClient();
        public static UdpClient udpClient = new UdpClient();

        public static NetworkStream stream;
        public static StreamReader reader;
        public static StreamWriter writer;

        public static int ID = 0;

        private static bool disconnectRequest = false;

        private static ClientState state = ClientState.Disconnected;

        private static bool isLostConnection = false;

        public static bool clientLog = true;
        public static bool udpLog = false;
        public static bool exception = true;

        public static async Task Connect(string _ip, int _port = 7777)
        {
            if (state != ClientState.Connected)
            {

                try
                {

                    state = ClientState.Connecting;
                    isLostConnection = false;

                    client = new TcpClient();
                    await client.ConnectAsync(_ip, _port);

                    stream = client.GetStream();
                    reader = new StreamReader(stream);
                    writer = new StreamWriter(stream);
                    writer.AutoFlush = true;

                    var response = reader.ReadLine();

                    if (response == "#FULL")
                    {
                        throw new FullError();
                    }
                    if (response == "#NO")
                    {
                        throw new RefuseError();
                    }
                    else
                    {


                        udpClient = new UdpClient();
                        udpClient.Connect(_ip, _port);

                        //writer.WriteLine(udpClient.Client.LocalEndPoint);

                        string myID = reader.ReadLine();

                        ID = int.Parse(myID.Substring(1, myID.Length - 1));

                        SendUDP($"#CONNECTION {ID}");

                        RecepterTCP();
                        RecepterUDP();

                        state = ClientState.Connected;

                        if (myID != "#0")
                            print("Connexion réussi !", ConsoleColor.Yellow);


                    }


                }
                catch (SocketException e)
                {
                    print("Connection failed : Check IP and Port or Server is not online ! \n", ConsoleColor.Red);
                    if (writer != null) writer.Close();
                    if (reader != null) reader.Close();
                    if (stream != null) stream.Close();
                    if (client != null) client.Close();
                    if (udpClient != null) udpClient.Close();
                    state = ClientState.Disconnected;
                    if (exception) throw new Error.ConnectionError();
                }
                catch (FullError)
                {
                    print("Connection failed : Server is full !", ConsoleColor.Red);
                    writer.Close();
                    reader.Close();
                    stream.Close();
                    client.Close();
                    udpClient.Close();
                    state = ClientState.Disconnected;
                    if (exception) throw new Error.FullError();
                }
                catch (RefuseError)
                {
                    print("Connection failed : Server is full !", ConsoleColor.Red);
                    writer.Close();
                    reader.Close();
                    stream.Close();
                    client.Close();
                    udpClient.Close();
                    state = ClientState.Disconnected;
                    if (exception) throw new Error.RefuseError();
                }

            }
            else print("You are already connected !", ConsoleColor.Yellow);


        }

        public static void Disconnect()
        {
            if (state != ClientState.Disconnected)
            {
                disconnectRequest = true;
                if (client != null) client.GetStream().Socket.Close();
                if (client != null) client.Close();
                if (writer != null) writer.Close();
                if (reader != null) reader.Close();
                if (stream != null) stream.Close();
                if (udpClient != null) udpClient.Close();
                print("You are disconnected !", ConsoleColor.Yellow);
                state = ClientState.Disconnected;
            }
            else
                print("You are already disconnected !", ConsoleColor.Yellow);


        }

        public static ClientState GetState()
        {
            return state;
        }

        public static bool IsLostConnection()
        {
            return isLostConnection;
        }


        public static async void RecepterTCP()
        {

            try
            {

                while (true)
                {

                    var msg = await reader.ReadLineAsync();

                    print("new Message : " + msg, ConsoleColor.Cyan);
                    ClientReader.ReadTCPPacket(msg);


                }

            }
            catch (SocketException e)
            {
                Console.WriteLine("ERROR 404 : " + e);
            }
            catch (IOException e)
            {
                if (disconnectRequest == false && ID != 0)
                {
                    print("Connection lost !", ConsoleColor.Red);
                    isLostConnection = true;
                    //if (exception) throw new Error.LostConnectionError();
                }

                writer.Close();
                reader.Close();
                stream.Close();
                client.Close();
                udpClient.Close();
                disconnectRequest = false;
                state = ClientState.Disconnected;
            }

        }


        public static async void RecepterUDP()
        {

            try
            {

                while (true)
                {

                    UdpReceiveResult result = await udpClient.ReceiveAsync();
                    byte[] bytes = result.Buffer;
                    string msg = Encoding.UTF8.GetString(bytes);

                    if (udpLog)
                        print($"New UDP Message : " + msg + "\n", ConsoleColor.Cyan);

                    ClientReader.ReadUDPPacket(msg);

                }

            }
            catch (SocketException e)
            {
                //print("SO" + e.ToString(), ConsoleColor.Red);
            }
            catch (IOException e)
            {
                //print("IO" + e.ToString(), ConsoleColor.Red);
            }

        }


        public static async void SendTCP(string message)
        {
            await writer.WriteLineAsync(message);
        }

        public static async void SendUDP(string message)
        {
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(message);
                await udpClient.SendAsync(bytes);
            }
            catch (SocketException) { }

        }


        public static void print(string msg, ConsoleColor color)
        {
            if (clientLog)
            {
                Console.ForegroundColor = color;
                Console.WriteLine("[LOCAL] " + msg);
                Console.ForegroundColor = ConsoleColor.White;
            }

        }


        public enum ClientState
        {
            Disconnected = 0,
            Connecting = 1,
            Connected = 2,
        }


    }
}
