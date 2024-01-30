using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Plateform_2D_v9
{
    /// <summary>
    /// DELETE
    /// </summary>
    class ServerSocket
    {

        public TcpClient client;
        public NetworkStream networkStream;
        public TcpListener tcpServer;
        public Socket socket;

        public string IP = "127.0.0.1";
        public Int32 port = 7777;
        public string message = "";


        public void StartServer(string IP = "127.0.0.1", int port = 7777, Int32 MaxConnection = 10)
        {

            IPAddress address = IPAddress.Parse(IP);

            try
            {
                tcpServer = new TcpListener(address, port);
                tcpServer.Start(MaxConnection);
                message = "server allume avec succes";

            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);

                if (e.ErrorCode == 1111)
                    message = "ERROR";

            }

        }

        public void StopServer()
        {
            try
            {
                if(tcpServer is null)
                    message = "error";
                else
                    tcpServer.Stop();

                //tcpServer.Server.Close();

                tcpServer = null;
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("SocketException: {0}", e);

            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);

                if (e.ErrorCode == 1111)
                    message = "ERROR";
            }

            NetPlay.IsMultiplaying = false;
            

        }


        //[Obsolete("Ne pas utiliser la méthode, non fonctionnelle.")]
        public async void ListenConnection()
        {

            if(tcpServer != null)
            {

                try
                {
                    message = "listen action";
                    //socket = await tcpServer.AcceptSocketAsync();
                    client = await tcpServer.AcceptTcpClientAsync();
                    networkStream = client.GetStream();
                }
                catch(ObjectDisposedException e)
                {
                }

            }
                
        }


        public void CheckSocket()
        {
            if (socket != null)
            {

                if (!socket.Connected)
                {
                    socket.Disconnect(false);
                }
            }
        }


        public void StopSocket()
        {
            if(socket != null)
                socket.Disconnect(false);
        }


        public void Send(int level)
        {

            byte[] msg = System.Text.Encoding.ASCII.GetBytes(level.ToString());

            networkStream.Write(msg, 0, msg.Length);

            networkStream.Write(msg, 0, 0);

        }


        public bool IsDisconnected()
        {
            if (tcpServer == null)
                return true;

            return false;
        }


        public string GetMessage()
        {
            return message;
        }

    }
}
