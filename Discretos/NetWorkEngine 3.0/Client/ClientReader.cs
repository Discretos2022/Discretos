using Microsoft.Xna.Framework;
using Plateform_2D_v9.NetWorkEngine_2._0.Client;
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

                case PacketType.disconnectedPlayer:
                    int disconnectedPlayer = (int.Parse(GetData(packet).Split("/")[0]));
                    int newID = (int.Parse(GetData(packet).Split("/")[1]));
                    Console.WriteLine("[SERVER] Player " + disconnectedPlayer + " was disconnected.");
                    Console.WriteLine("[SERVER] Your ID : " + newID);
                    Client.playerID = (Client.PlayerID)newID;
                    Handler.playersV2[disconnectedPlayer] = null;

                    for (int i = 1; i < Handler.playersV2.Count; i++)
                    {
                        if (Handler.playersV2[i] is null)
                            Handler.playersV2[i] = Handler.playersV2[i + 1];

                    }

                    Handler.playersV2[newID].ID = newID;
                    Handler.playersV2[newID].clientID = newID;

                    Handler.playersV2.Remove(Handler.playersV2.Count);

                    break;

                case PacketType.otherPlayerWorldMapPosition:
                    int x = (int.Parse(GetData(packet).Split("/")[0]));
                    int y = (int.Parse(GetData(packet).Split("/")[1]));

                    WorldMap.SetLevelSelectorPos(new Vector2(x, y));

                    Console.WriteLine(x + "/" + y);

                    break;

            }

        }

        private static string GetData(string packet)
        {
            return packet.Substring(5, packet.Length - 5);
        }

    }
}
