using Microsoft.Xna.Framework;
using Plateform_2D_v9;
using Plateform_2D_v9.NetCore;
using Plateform_2D_v9.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NetworkEngine_5._0.Client
{
    public static class ClientReader
    {

        public static void ReadTCPPacket(string packet)
        {
            NetPlay.PacketType packetID = (NetPlay.PacketType)GetPacketID(packet);

            switch (packetID)
            {
                case NetPlay.PacketType.otherPlayerJoined:
                    //if(!Handler.playersV2.ContainsKey(int.Parse(GetData(packet)) + 1))
                        //Handler.AddPlayerV2(int.Parse(GetData(packet)) + 1, NetPlay.MyPlayerID());

                    int id = int.Parse(GetData(packet));

                    if (id == 1)
                        NetPlay.usedPlayerID.Clear();

                    if (!NetPlay.usedPlayerID.Contains(id))
                        NetPlay.usedPlayerID.Add(id);

                    NetPlay.usedPlayerID.OrderBy(n => n);

                    break;

                case NetPlay.PacketType.gameStarted:

                    for (int i = 0; i < NetPlay.usedPlayerID.Count; i++)
                    {
                        Handler.AddPlayerV2(NetPlay.usedPlayerID[i], NetPlay.MyPlayerID());
                    }

                    Main.playState = PlayState.InWorldMap;
                    Camera.Zoom = 1f;
                    Main.gameState = GameState.Playing;

                    break;


                case NetPlay.PacketType.levelStarted:

                    string data = GetData(packet);
                    int level = int.Parse(data);

                    Main.StartLevel(level);

                    break;

                case NetPlay.PacketType.gamePaused:

                    bool isPaused = bool.Parse(GetData(packet));
                    Main.isPaused = isPaused;

                    break;

                case NetPlay.PacketType.distroyedObject:

                    int index = int.Parse(GetData(packet));

                    if(index < Handler.actors.Count)
                    {
                        if (Handler.actors[index].actorType == Actor.ActorType.Item)
                            Main.Money += Handler.actors[index].ID;
                        if (Handler.actors[index].actorType == Actor.ActorType.Object)
                            if (Handler.actors[index].ID == 1)
                                Main.Money += 1;

                        LightManager.lights.Remove(Handler.actors[index].light);
                        Handler.actors.RemoveAt(index);
                    }
                        
                    break;

                case NetPlay.PacketType.createItem:

                    string[] itemData = GetData(packet).Split(";");

                    float x = float.Parse(itemData[0]);
                    float y = float.Parse(itemData[1]);
                    int idItem = int.Parse(itemData[2]);
                    float vx = float.Parse(itemData[3]);
                    float vy = float.Parse(itemData[4]);

                    Handler.actors.Add(new ItemV2(new Vector2(x, y), idItem, new Vector2(vx, vy)));

                    break;

                case NetPlay.PacketType.collectedKey:

                    itemData = GetData(packet).Split(";");

                    id = int.Parse(itemData[0]);
                    index = int.Parse(itemData[1]);

                    ((Key)Handler.actors[index]).isCollected = true;
                    Handler.playersV2[id].AddCollectedObject((Key)Handler.actors[index]);
                    Handler.actors.RemoveAt(index);
                        
                    break;

                case NetPlay.PacketType.openDoor:

                    itemData = GetData(packet).Split(";");

                    id = int.Parse(itemData[0]);
                    index = int.Parse(itemData[1]);

                    for (int o = 0; o < Handler.playersV2[id].GetCollectedObjectList().Count; o++)
                    {
                        if (((Door)Handler.actors[index]).trigger == ((Key)Handler.playersV2[id].GetCollectedObjectList()[o]).trigger && ((Door)Handler.actors[index]).hitbox.isEnabled)
                        { ((Door)Handler.actors[index]).isLocked = false; ((Door)Handler.actors[index]).hitbox.isEnabled = false; Handler.playersV2[id].GetCollectedObjectList().Remove(Handler.playersV2[id].GetCollectedObjectList()[o]); }
                    }

                    break;

                case NetPlay.PacketType.breakPlatform:

                    itemData = GetData(packet).Split(";");

                    id = int.Parse(itemData[0]);
                    x = int.Parse(itemData[1]);
                    y = int.Parse(itemData[2]);

                    if(Handler.Level[(int)x, (int)y].isBreakable)
                        Handler.Level[(int)x, (int)y].Break();

                    break;


                case NetPlay.PacketType.checkpointHited:

                    itemData = GetData(packet).Split(";");

                    id = int.Parse(itemData[0]);
                    index = int.Parse(itemData[1]);

                    ObjectV2 element = (ObjectV2)Handler.actors[index];

                    if (Level.lastCheckPointNumber < ((CheckPoint)element).number) Level.setCheckPoint(((CheckPoint)element));
                    ((CheckPoint)element).hited = true;

                    break;
            }

        }

        public static int GetPacketID(string packet)
        {
            return int.Parse(packet.Substring(1, 4));
        }

        public static string GetData(string packet)
        {
            return packet.Substring(5, packet.Length - 5);
        }





        public static void ReadUDPPacket(string packet)
        {
            NetPlay.PacketType packetID = (NetPlay.PacketType)GetPacketID(packet);

            switch (packetID)
            {
                case NetPlay.PacketType.playerOneWorldMapPosition:

                    string data = GetData(packet);

                    int x = int.Parse(data.Split(';')[0]);
                    int y = int.Parse(data.Split(';')[1]);

                    WorldMap.SetLevelSelectorPos(new Vector2(x, y));

                    break;


                case NetPlay.PacketType.playerPosition:

                    string[] playerPos = GetData(packet).Split(";");

                    int id = int.Parse(playerPos[0]);
                    float pX = float.Parse(playerPos[1]);
                    float pY = float.Parse(playerPos[2]);
                    bool isRight = bool.Parse(playerPos[3]);

                    Handler.playersV2[id].Position = new Vector2(pX, pY);
                    Handler.playersV2[id].isRight = isRight;

                    break;


            }
        }

    }
}
