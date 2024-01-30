using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Plateform_2D_v9.NetWorkEngine_2._0.Client
{
    class Client
    {
        public static Client instance;
        public static int dataBufferSize = 4096;

        public string ip = "127.0.0.1";  // 127.0.0.1  192.168.1.14
        public int port = 7777;
        public int myId = 0;
        public TCP tcp;
        public UDP udp;

        private bool isConnected = false;
        private delegate void PacketHandler(Packet _packet);
        private static Dictionary<int, PacketHandler> packetHandlers;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else if (instance != null)
            {
                Console.WriteLine("Instance already exists, destroying object!");
                instance = null;
            }
        }

        private void Start()
        {
            tcp = new TCP();
            udp = new UDP();
        }

        public void ConnectToServer(int _port)
        {
            InitializeClientData();
            instance = this;
            port = _port;
            Start();

            isConnected = true;
            tcp.Connect();
        }

        public void Disconnect()
        {

            if (isConnected)
            {
                isConnected = false;

                tcp.socket.Close();
                if(udp.socket != null)
                    udp.socket.Close();

                Console.WriteLine("You are disconnected ! (Server shutdown)");

                Main.gameState = GameState.Menu;

            }

            
        }


        public class TCP
        {
            public TcpClient socket;

            private NetworkStream stream;
            private Packet receivedData;
            private byte[] receiveBuffer;

            public void Connect()
            {
                socket = new TcpClient
                {
                    ReceiveBufferSize = dataBufferSize,
                    SendBufferSize = dataBufferSize
                };

                receiveBuffer = new byte[dataBufferSize];
                socket.BeginConnect(instance.ip, instance.port, ConnectCallback, socket);
            }

            private void ConnectCallback(IAsyncResult _result)
            {
                try 
                {
                    socket.EndConnect(_result);

                    if (!socket.Connected)
                    {
                        return;
                    }

                    stream = socket.GetStream();

                    receivedData = new Packet();

                    stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
                }
                catch (Exception)
                {
                    Console.WriteLine("Server is not launched !");
                }

                
            }

            public void SendData(Packet _packet)
            {
                try
                {
                    if (socket != null)
                    {
                        stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);
                    }
                }
                catch (Exception _ex)
                {
                    Console.WriteLine($"Error sending data to server via TCP: {_ex}");
                }
            }

            private void ReceiveCallback(IAsyncResult _result)
            {
                try
                {
                    int _byteLength = stream.EndRead(_result);
                    if (_byteLength <= 0)
                    {
                        instance.Disconnect();
                        return;
                    }

                    byte[] _data = new byte[_byteLength];
                    Array.Copy(receiveBuffer, _data, _byteLength);

                    //for (int i = 0; i < _data.Length; i++)
                      //  Console.WriteLine(_data[i]);

                    receivedData.Reset(HandleData(_data));
                    stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
                }
                catch(Exception e)
                {
                    Disconnect();
                }
            }

            private bool HandleData(byte[] _data)
            {
                int _packetLength = 0;

                receivedData.SetBytes(_data);

                if (receivedData.UnreadLength() >= 4)
                {
                    _packetLength = receivedData.ReadInt();
                    if (_packetLength <= 0)
                    {
                        return true;
                    }
                }

                while (_packetLength > 0 && _packetLength <= receivedData.UnreadLength())
                {
                    byte[] _packetBytes = receivedData.ReadBytes(_packetLength);
                    //ThreadManager.ExecuteOnMainThread(() =>
                    //{
                        using (Packet _packet = new Packet(_packetBytes))
                        {
                            int _packetId = _packet.ReadInt();
                            packetHandlers[_packetId](_packet);
                            //ClientHandle.Welcome(_packet);
                        }
                    //});

                    _packetLength = 0;
                    if (receivedData.UnreadLength() >= 4)
                    {
                        _packetLength = receivedData.ReadInt();
                        if (_packetLength <= 0)
                        {
                            return true;
                        }
                    }
                }

                if (_packetLength <= 1)
                {
                    return true;
                }

                return false;
            }
            
            /// <summary>
            /// Added
            /// </summary>
            private void Disconnect()
            {
                instance.Disconnect();

                stream = null;
                receivedData = null;
                receiveBuffer = null;
                socket = null;

            }

        }

        public class UDP
        {
            public UdpClient socket;
            public IPEndPoint endPoint;

            public UDP()
            {
                endPoint = new IPEndPoint(IPAddress.Parse(instance.ip), instance.port);
            }

            public void Connect(int _localPort)
            {
                socket = new UdpClient(_localPort);

                socket.Connect(endPoint);
                socket.BeginReceive(ReceiveCallback, null);

                using (Packet _packet = new Packet())
                {
                    SendData(_packet);
                }

            }

            public void SendData(Packet _packet)
            {
                try
                {
                    _packet.InsertInt(instance.myId);
                    if(socket != null)
                    {
                        socket.BeginSend(_packet.ToArray(), _packet.Length(), null, null);
                    }
                }
                catch (Exception _ex)
                {
                    Console.WriteLine($"Error sending data to server via UDP: {_ex}");
                }
            }

            private void ReceiveCallback(IAsyncResult _result)
            {
                try
                {
                    byte[] _data = socket.EndReceive(_result, ref endPoint);
                    socket.BeginReceive(ReceiveCallback, null);

                    if(_data.Length < 4)
                    {
                        instance.Disconnect();
                        return;
                    }

                    HandleData(_data);


                }
                catch (Exception)
                {
                    Disconnect();
                }
            }

            private void HandleData(byte[] _data)
            {
                using (Packet _packet = new Packet(_data))
                {
                    int _packetLength = _packet.ReadInt();
                    _data = _packet.ReadBytes(_packetLength);
                }

                using (Packet _packet = new Packet(_data))
                {
                    int _packetId = _packet.ReadInt();
                    packetHandlers[_packetId](_packet);
                }

            }

            /// <summary>
            /// Added
            /// </summary>
            private void Disconnect()
            {
                instance.Disconnect();

                endPoint = null;
                socket = null;
            }


        }

        private void InitializeClientData()
        {
            packetHandlers = new Dictionary<int, PacketHandler>()
        {
            { (int)ServerPackets.welcome, ClientHandle.Welcome },
            { (int)ServerPackets.level, ClientHandle.Level },               /// added
            { (int)ServerPackets.udpTest, ClientHandle.UDPTest },
            { (int)ServerPackets.money, ClientHandle.CollectedMoney },      /// added
            { (int)ServerPackets.playerState, ClientHandle.PlayerState },   /// added
            { (int)ServerPackets.playerKey, ClientHandle.PlayerKey },       /// added
            { (int)ServerPackets.playerPos, ClientHandle.PlayerPos },       /// added
            { (int)ServerPackets.newPLayerConnected, ClientHandle.NewPlayer },       /// added
        };
            Console.WriteLine("Initialized packets.");
        }


    }
}
