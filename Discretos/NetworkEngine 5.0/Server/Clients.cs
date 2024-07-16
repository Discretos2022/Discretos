using NetworkEngine_5._0.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkEngine_5._0.Server
{
    public class Clients
    {

        private TcpClient client;
        private int ID;
        public IPEndPoint udpEndPoint;

        public UdpClient udpClient;

        public StreamReader reader;
        public StreamWriter writer;
        public NetworkStream stream;

        public Clients(TcpClient _client, int _ID)
        {
            client = _client;
            ID = _ID;
            udpClient = new UdpClient();

            stream = client.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream);
            writer.AutoFlush = true;

        }


        public async void RecepterTCP()
        {

            try
            {

                while (true)
                {

                    var msg = await reader.ReadLineAsync();

                    Server.print($"New Message : " + msg, ConsoleColor.Cyan, $"[CLIENT { ID}]");
                    ServerReader.ReadTCPPacket(msg, ID);

                }

            }
            catch (IOException e)
            {
                if(Server.stopRequest == false)
                    Server.print($"Client {ID} disconnected ! \n", ConsoleColor.DarkMagenta);
                Server.clients.Remove(this);
                Server.usedID.Remove(ID);
            }

        }


        public async void Send(string msg)
        {
            await writer.WriteLineAsync(msg);
        }


        public void Disconnect()
        {

            if (stream != null) stream.Socket.Close();
            if (stream != null) stream.Close();
            if (client != null) client.Close();
            if (udpClient != null) udpClient.Close();
            if (writer != null) writer.Close();
            if (reader != null) reader.Close();

        }


        public string GetIP()
        {
            if(client.Client.LocalEndPoint != null)
                return client.Client.RemoteEndPoint.ToString();

            return "ERROR";
        }

        public int GetID()
        {
            return ID;
        }

    }
}
