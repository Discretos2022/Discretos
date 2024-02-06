﻿using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace Plateform_2D_v9.NetWorkEngine_3._0.Server
{
    public class ServerClient
    {

        public TcpClient tcpClient;
        public NetworkStream stream;
        public StreamReader reader;
        public StreamWriter writer;

        public int ID;

        public ServerClient(TcpClient client, int ID)
        {

            tcpClient = client;

            this.ID = ID;

            stream = client.GetStream();
            reader = new StreamReader(stream, Encoding.UTF8);
            writer = new StreamWriter(stream, Encoding.UTF8);
            writer.AutoFlush = true;

        }

        public async void Recepter()
        {

            while (true)
            {

                try
                {

                    string text = await reader.ReadLineAsync();

                    if (text != null)
                    {

                        /// TODO : Que faire du message ?
                        ServerReader.ReadPacket(text);

                    }
                    else
                        break; /// Client is disconnected

                }
                catch(IOException e)
                {
                    Console.WriteLine("ERROR : " + e);
                    break;
                }

            }

            reader.Close();
            writer.Close();
            stream.Close();
            tcpClient.Close();
            tcpClient = null;


        }

        public string GetIP()
        {
            return tcpClient.Client.RemoteEndPoint.ToString();
        }

        public void Disconnect()
        {
            if (stream != null)
            {
                writer.Close();
                reader.Close();
                stream.Close();
                stream = null;
                reader = null;
                writer = null;
            }

            tcpClient.Close();
            tcpClient = null;

        }

    }
}