using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Plateform_2D_v9.NetWorkEngine_3._0.NetPlay;

namespace Plateform_2D_v9.NetWorkEngine_3._0.Server
{
    public static class ServerReader
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

                case PacketType.playerOneWorldMapPosition:

                    int x = (int.Parse(GetData(packet).Split("/")[0]));
                    int y = (int.Parse(GetData(packet).Split("/")[1]));

                    Server.SendWorldMapPositionPlayer(x, y);

                    break;

            }

        }

        private static string GetData(string packet)
        {
            return packet.Substring(5, packet.Length - 5);
        }

    }
}
