using Plateform_2D_v9;
using Plateform_2D_v9.NetCore;
using System.IO;
using System.Linq;

namespace NetworkEngine_5._0.Server
{
    public class ServerSender
    {

        public static void SendID(int ID, int clientID)
        {

            string packet = CreateTCPpacket(ID.ToString(), NetPlay.PacketType.playerID);
            Server.SendTCP(packet, clientID);
            //Handler.AddPlayerV2(ID, 1);


        }

        public static void SendOtherPlayerID(int otherID)
        {

            if (otherID == 1)
                NetPlay.usedPlayerID.Clear();

            string packet = CreateTCPpacket(otherID.ToString(), NetPlay.PacketType.otherPlayerJoined);
            Server.SendTCP(packet);
            //Handler.AddPlayerV2(otherID, 1);
            if(!NetPlay.usedPlayerID.Contains(otherID))
                NetPlay.usedPlayerID.Add(otherID);

            NetPlay.usedPlayerID.OrderBy(n => n);

        }


        public static void SendGameStarted()
        {
            string packet = CreateTCPpacket("START", NetPlay.PacketType.gameStarted);
            Server.SendTCP(packet);

            for (int i = 0; i < NetPlay.usedPlayerID.Count; i++)
            {
                Handler.AddPlayerV2(NetPlay.usedPlayerID[i], NetPlay.MyPlayerID());
            }

            Main.playState = PlayState.InWorldMap;
            Camera.Zoom = 1f;
            Main.gameState = GameState.Playing;

        }


        public static void SendLevelStarted(int level)
        {
            string packet = CreateTCPpacket(level.ToString("000"), NetPlay.PacketType.levelStarted);
            Server.SendTCP(packet);

        }

        public static void SendGamePaused(bool isPaused)
        {
            string packet = CreateTCPpacket(isPaused.ToString(), NetPlay.PacketType.gamePaused);
            Server.SendTCP(packet);

        }

        public static void SendDistroyedObject(int index, int except = -1)
        {
            string packet = CreateTCPpacket(index.ToString(), NetPlay.PacketType.distroyedObject);
            Server.SendTCP(packet, 0, except);
        }

        public static void SendCreatedItem(float x, float y, int id, float vx, float vy, int except = -1)
        {
            string packet = CreateTCPpacket(x + ";" + y + ";" + id + ";" + vx + ";" + vy, NetPlay.PacketType.createItem);
            Server.SendTCP(packet, 0, except);
        }

        public static void SendCollectedKey(int index, int playerID, int except = -1)
        {
            string packet = CreateTCPpacket(playerID.ToString() + ";" + index.ToString(), NetPlay.PacketType.collectedKey);
            Server.SendTCP(packet, 0, except);
        }

        public static void SendOpenDoor(int index, int playerID, int except = -1)
        {
            string packet = CreateTCPpacket(playerID.ToString() + ";" + index.ToString(), NetPlay.PacketType.openDoor);
            Server.SendTCP(packet, 0, except);
        }

        public static void SendBreakPlatform(int x, int y, int playerID, int except = -1)
        {
            string packet = CreateTCPpacket(playerID.ToString() + ";" + x + ";" + y, NetPlay.PacketType.breakPlatform);
            Server.SendTCP(packet, 0, except);
        }

        public static void SendCheckPointHited(int index, int playerID, int except = -1)
        {
            string packet = CreateTCPpacket(playerID.ToString() + ";" + index, NetPlay.PacketType.checkpointHited);
            Server.SendTCP(packet, 0, except);
        }


        public static void SendWorldMapPositionPlayer(int x, int y)
        {
            string packet = CreateUDPpacket(x + ";" + y, NetPlay.PacketType.playerOneWorldMapPosition);
            Server.SendUDP(packet);
        }

        public static void SendPositionPlayer(int PlayerID, float x, float y, bool isRight)
        {
            string packet = CreateUDPpacket(PlayerID + ";" + x + ";" + y + ";" + isRight, NetPlay.PacketType.playerPosition);
            Server.SendUDP(packet, except:PlayerID-1);
        }


        private static string CreateTCPpacket(string data, NetPlay.PacketType type)
        {
            return "$" + ((int)type).ToString("0000") + " " + data;
        }

        private static string CreateUDPpacket(string data, NetPlay.PacketType type)
        {
            return "$" + ((int)type).ToString("0000") + " " + data;
        }


    }
}
