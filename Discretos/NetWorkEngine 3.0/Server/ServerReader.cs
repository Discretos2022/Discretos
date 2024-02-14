using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static Plateform_2D_v9.NetWorkEngine_3._0.NetPlay;

namespace Plateform_2D_v9.NetWorkEngine_3._0.Server
{
    public static class ServerReader
    {

        public static class TCP
        {
            public static void ReadPacket(string packet)
            {
                PacketType packetID = (PacketType)int.Parse(packet.Substring(0, 4));

                switch (packetID)
                {
                    case PacketType.playerID:
                        break;

                    case PacketType.gameStarted:
                        break;

                    case PacketType.otherPlayerJoined:
                        break;

                }

            }

            private static string GetData(string packet)
            {
                return packet.Substring(5, packet.Length - 5);
            }
        }


        public static class UDP
        {
            private static DateTime lastPacket = DateTime.Now;

            public static void ReadPacket(string packet, IPEndPoint result)
            {

                DateTime packetTime = DateTime.ParseExact(packet.Substring(0, 16), "yyyy:HH:mm:ss.ff", null);

                if (DateTime.Compare(lastPacket, packetTime) >= 0)
                    goto L_1;

                PacketType packetID = (PacketType)int.Parse(packet.Substring(17, 4));

                switch (packetID)
                {
                    case PacketType.None:
                        Console.WriteLine("[SERVER] " + packet);
                        break;

                    case PacketType.playerID:
                        break;

                    case PacketType.gameStarted:
                        break;

                    case PacketType.otherPlayerJoined:
                        break;

                    case PacketType.disconnectedPlayer:
                        break;

                    case PacketType.otherPlayerWorldMapPosition:
                        break;

                    case PacketType.playerOneWorldMapPosition:

                        int x = (int.Parse(GetData(packet).Split("/")[0]));
                        int y = (int.Parse(GetData(packet).Split("/")[1]));

                        Server.SendWorldMapPositionPlayer(x, y);

                        break;

                    case PacketType.firstMsgForPortPlayer: 
                        
                        string data = GetData(packet);
                        int clientID = int.Parse(data);

                        Server.clients[clientID - 1].endPoint = result;

                        Console.WriteLine("[SERVER] Player " + clientID + "UDP : " + result);

                        break;

                }

            L_1:;

                lastPacket = packetTime;

            }

            private static string GetData(string packet)
            {
                return packet.Substring(24, packet.Length - 24);
            }
        }


    }
}
