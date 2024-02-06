using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Plateform_2D_v9.NetWorkEngine_3._0.NetPlay;

namespace Plateform_2D_v9.NetWorkEngine_3._0.Client
{
    public static class ClientReader
    {

        public static void ReadPacket(string packet)
        {
            PacketType packetID = (PacketType)int.Parse(packet.Substring(0, 4));

            switch (packetID)
            {
                case PacketType.playerID:
                    Client.playerID = (Client.PlayerID)int.Parse(GetData(packet));
                    Console.WriteLine("[SERVER] Your ID : " + Client.playerID);
                    for (int i = 1; i <= (int)Client.playerID; i++)
                    {
                        Handler.AddPlayerV2(i);
                    }
                    break;

                case PacketType.gameStarted:
                    Main.gameState = GameState.Playing;
                    break;

                case PacketType.otherPlayerJoined:
                    int newPlayer = int.Parse(GetData(packet));
                    Console.WriteLine("[SERVER] Player " + newPlayer + " has joined.");
                    Handler.AddPlayerV2(newPlayer);
                    break;

            }

        }

        private static string GetData(string packet)
        {
            return packet.Substring(5, packet.Length - 5);
        }

    }
}
