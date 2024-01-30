using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Plateform_2D_v9.NetWorkEngine_2._0.Server
{
    class Server
    {

        public static int MaxPlayers { get; private set; }
        public static int Port { get; private set; }
        public static Dictionary<int, ServerClient> clients = new Dictionary<int, ServerClient>();
        public delegate void PacketHandler(int _fromClient, Packet _packet);
        public static Dictionary<int, PacketHandler> packetHandlers;

        private static TcpListener tcpListener;
        private static UdpClient udpListener;

        public static bool Player2isConnected = false;

        /// <summary>
        /// The port max is 65535
        /// </summary>
        /// <param name="_maxPlayers"></param>
        /// <param name="_port"></param>
        public static void Start(int _maxPlayers, int _port)
        {
            MaxPlayers = _maxPlayers;
            Port = _port;

            Console.WriteLine("Starting server...");
            InitializeServerData();

            tcpListener = new TcpListener(IPAddress.Any, Port);
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);

            udpListener = new UdpClient(Port);
            udpListener.BeginReceive(UDPReceiveCallback, null);

            Console.WriteLine($"Server started on {Port}.");

        }

        /// <summary>
        /// Added
        /// </summary>
        public static void Stop()
        {

            if(tcpListener != null)
            {
                tcpListener.Server.Close();
                tcpListener.Stop();
            }
            
            udpListener.Close();

            clients.Clear();

        }

        private static void TCPConnectCallback(IAsyncResult _result)
        {

            try
            {
                TcpClient _client = tcpListener.EndAcceptTcpClient(_result);
                tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);
                Console.WriteLine($"Incoming connection from {_client.Client.RemoteEndPoint}...");

                for (int i = 1; i <= MaxPlayers; i++)
                {
                    if (clients[i].tcp.socket == null)
                    {
                        clients[i].tcp.Connect(_client);
                        return;
                    }
                }

                Console.WriteLine($"{_client.Client.RemoteEndPoint} failed to connect: Server full!");
            }
            catch (ObjectDisposedException)
            {

                for (int i = 1; i < clients.Count; i++)
                {
                    if(clients[i].tcp.socket != null)
                        clients[i].tcp.socket.Close();
                }

                tcpListener = null;
                Console.WriteLine("TCP connection closed ! (SERVER)");
            }

        }

        private static void UDPReceiveCallback(IAsyncResult _result)
        {
            try
            {
                IPEndPoint _clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] _data = udpListener.EndReceive(_result, ref _clientEndPoint);
                udpListener.BeginReceive(UDPReceiveCallback, null);

                if(_data.Length < 4)
                {
                    return;
                }

                using (Packet _packet = new Packet(_data))
                {
                    int _clientId = _packet.ReadInt();

                    if(_clientId == 0)
                    {
                        return;
                    }

                    if(clients[_clientId].udp.endPoint == null)
                    {
                        clients[_clientId].udp.Connect(_clientEndPoint);
                        return;
                    }

                    if(clients[_clientId].udp.endPoint.ToString() == _clientEndPoint.ToString())
                    {
                        clients[_clientId].udp.HandleData(_packet);
                    }

                }

            }
            catch (ObjectDisposedException)
            {
                udpListener = null;
                Console.WriteLine("UPD connection closed ! (SERVER)");
            }
            catch (Exception _ex)
            {
                Console.WriteLine($"Error receiving UDP data: {_ex}");
            }
        }

        public static void SendUDPData(IPEndPoint _clientEndPoint, Packet _packet)
        {
            try
            {
                if(_clientEndPoint != null)
                {
                    udpListener.BeginSend(_packet.ToArray(), _packet.Length(), _clientEndPoint, null, null);
                }
            }
            catch (Exception _ex)
            {
                Console.WriteLine($"Error to sending data to {_clientEndPoint} via UDP: {_ex}");
            }
        }

        private static void InitializeServerData()
        {
            for(int i = 1; i <= 4; i++) // The max player game is 4.
            {
                clients.Add(i, new ServerClient(i));
            }

            packetHandlers = new Dictionary<int, PacketHandler>()
            {
                { (int)ClientPackets.welcomeReceived, ServerHandle.WelcomeReceived },
                { (int)ClientPackets.playerpos, ServerHandle.PlayerPos },               /// added
                { (int)ClientPackets.udpTestReceived, ServerHandle.UDPTestReceived },
                { (int)ClientPackets.collectedMoney, ServerHandle.CollectedMoney },     /// added
                { (int)ClientPackets.playerState, ServerHandle.PlayerState },           /// added
                { (int)ClientPackets.playerKey, ServerHandle.PlayerKey },           /// added
            };
            Console.WriteLine("Initialized packets.");

        }
    }
}
