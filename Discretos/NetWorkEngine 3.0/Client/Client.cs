using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Plateform_2D_v9.NetWorkEngine_3._0.Client
{
    class Client
    {

        private static TcpClient client;

        private static NetworkStream stream;
        private static StreamWriter writer;
        private static StreamReader reader;
        private static bool isTimeOut = false;

        public static ClientState state = ClientState.Disconnected;

        public static PlayerID playerID = PlayerID.None;

        public async static void Connect(string IP, int port = 7777)
        {

            try
            {
                client = new TcpClient();
                isTimeOut = false;
                state = ClientState.Connecting;
                await client.ConnectAsync(IP, port);
                state = ClientState.Connected;

                stream = client.GetStream();
                writer = new StreamWriter(stream, Encoding.UTF8);
                reader = new StreamReader(stream, Encoding.UTF8);
                writer.AutoFlush = true;

                while (true)
                {

                    try
                    {

                        //string text = await reader.ReadLineAsync();

                        await writer.WriteAsync("Salut");

                        //if (text != null)
                        //{
                        //    /// TODO : Que faire du message ?
                        //}
                        //else
                        //{
                        //    Disconnect("Server shutdown !");
                        //}

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

    }
}
