using Microsoft.Xna.Framework;
using Plateform_2D_v9;
using Plateform_2D_v9.NetCore;
using Plateform_2D_v9.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Plateform_2D_v9.ItemV2;

namespace NetworkEngine_5._0.Server
{
    public static class ServerReader
    {

        public static void ReadTCPPacket(string packet, int sender)
        {

            NetPlay.PacketType packetID = (NetPlay.PacketType)GetPacketID(packet);

            switch (packetID)
            {

                case NetPlay.PacketType.distroyedObject:

                    int index = int.Parse(GetDataTCP(packet));

                    if (index < Handler.actors.Count && index >= 0)
                    {
                        if (Handler.actors[index].actorType == Actor.ActorType.Item)
                            Main.Money += (int)((ItemV2)Handler.actors[index]).ID;
                        if (Handler.actors[index].actorType == Actor.ActorType.Object)
                            if (((ObjectV2)Handler.actors[index]).objectID == ObjectV2.ObjectID.coin)
                                Main.Money += 1;

                        ServerSender.SendDistroyedObject(index, sender);

                        LightManager.lights.Remove(Handler.actors[index].light);
                        Handler.actors.RemoveAt(index);
                    }

                    break;

                case NetPlay.PacketType.createItem:

                    string[] itemData = GetDataTCP(packet).Split(";");

                    float x = float.Parse(itemData[0]);
                    float y = float.Parse(itemData[1]);
                    int idItem = int.Parse(itemData[2]);
                    float vx = float.Parse(itemData[3]);
                    float vy = float.Parse(itemData[4]);

                    Handler.actors.Add(new ItemV2(new Vector2(x, y), (ItemID)idItem, new Vector2(vx, vy)));

                    ServerSender.SendCreatedItem(x, y, idItem, vx, vy, sender);

                    break;

                case NetPlay.PacketType.collectedKey:

                    itemData = GetDataTCP(packet).Split(";");

                    int id = int.Parse(itemData[0]);
                    index = int.Parse(itemData[1]);

                    ((Key)Handler.actors[index]).isCollected = true;
                    Handler.playersV2[id].AddCollectedObject((Key)Handler.actors[index]);
                    Handler.actors.RemoveAt(index);

                    ServerSender.SendCollectedKey(index, id, sender);

                    break;


                case NetPlay.PacketType.openDoor:

                    itemData = GetDataTCP(packet).Split(";");

                    id = int.Parse(itemData[0]);
                    index = int.Parse(itemData[1]);

                    for (int o = 0; o < Handler.playersV2[id].GetCollectedObjectList().Count; o++)
                    {
                        if (((Door)Handler.actors[index]).trigger == ((Key)Handler.playersV2[id].GetCollectedObjectList()[o]).trigger && ((Door)Handler.actors[index]).hitbox.isEnabled)
                        { ((Door)Handler.actors[index]).isLocked = false; ((Door)Handler.actors[index]).hitbox.isEnabled = false; Handler.playersV2[id].GetCollectedObjectList().Remove(Handler.playersV2[id].GetCollectedObjectList()[o]); }
                    }

                    ServerSender.SendOpenDoor(index, id, sender);

                    break;


                case NetPlay.PacketType.breakPlatform:

                    itemData = GetDataTCP(packet).Split(";");

                    id = int.Parse(itemData[0]);
                    x = int.Parse(itemData[1]);
                    y = int.Parse(itemData[2]);

                    if (Handler.Level[(int)x, (int)y].isBreakable)
                        Handler.Level[(int)x, (int)y].Break();

                    ServerSender.SendBreakPlatform((int)x, (int)y, id, sender);

                    break;


                case NetPlay.PacketType.checkpointHited:

                    itemData = GetDataTCP(packet).Split(";");

                    id = int.Parse(itemData[0]);
                    index = int.Parse(itemData[1]);

                    ObjectV2 element = (ObjectV2)Handler.actors[index];

                    if (Level.lastCheckPointNumber < ((CheckPoint)element).number) Level.setCheckPoint(((CheckPoint)element));
                    ((CheckPoint)element).hited = true;

                    ServerSender.SendCheckPointHited(index, id, sender);

                    break;


            }

        }

        public static int GetPacketID(string data)
        {
            return int.Parse(data.Substring(1, 4));
        }

        public static string GetDataUDP(string packet)
        {
            return packet.Substring(7, packet.Length - 7);
        }

        public static string GetDataTCP(string packet)
        {
            return packet.Substring(5, packet.Length - 5);
        }

        public static string GetSenderID(string packet)
        {
            return packet.Substring(6, 1);
        }



        public static void ReadUDPPacket(string packet)
        {

            NetPlay.PacketType packetID = (NetPlay.PacketType)GetPacketID(packet);
            int playerID = int.Parse(GetSenderID(packet));
            string[] data = GetDataUDP(packet).Split(";");

            switch (packetID)
            {

                case NetPlay.PacketType.playerPosition:

                    int id = int.Parse(data[0]);
                    float pX = float.Parse(data[1]);
                    float pY = float.Parse(data[2]);
                    bool isRight = bool.Parse(data[3]);

                    Handler.playersV2[id].Position = new Vector2(pX, pY);
                    Handler.playersV2[id].isRight = isRight;

                    ServerSender.SendPositionPlayer(playerID, Handler.playersV2[id].Position.X, Handler.playersV2[id].Position.Y, Handler.playersV2[id].isRight);

                    break;



            }
        }

    }
}
