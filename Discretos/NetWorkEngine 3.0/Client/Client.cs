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

        public async static void Connect(string IP, int port = 7777)
        {

            try
            {
                client = new TcpClient();
                await client.ConnectAsync(IP, port);

                stream = client.GetStream();
                writer = new StreamWriter(stream, Encoding.UTF8);
                reader = new StreamReader(stream, Encoding.UTF8);
                writer.AutoFlush = true;


                while (true)
                {

                    try
                    {
                        string text = await reader.ReadLineAsync();

                        if(text != null)
                        {
                            /// TODO : Que faire du message ?
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

                }

            }
            catch (SocketException e)
            {

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

                Console.WriteLine(error);
            }

        }

    }
}
