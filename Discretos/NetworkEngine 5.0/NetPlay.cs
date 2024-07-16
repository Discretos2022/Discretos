using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NetworkEngine_5._0.Client;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Plateform_2D_v9.NetCore
{


    public static class NetPlay
    {
        public static bool IsMultiplaying = false;

        public static string LocalIPAddress()
        {
            string result = "";
            IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress[] addressList = hostEntry.AddressList;
            for (int i = 0; i < addressList.Length; i++)
            {
                IPAddress iPAddress = addressList[i];
                if (iPAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    result = iPAddress.ToString();
                    break;
                }
            }
            return result;
        }

        public enum PacketType
        {
            None = 0,
            playerID = 1,
            gameStarted = 2,
            otherPlayerJoined = 3,
            disconnectedPlayer = 4,
            playerOneWorldMapPosition = 5,
            otherPlayerWorldMapPosition = 6,
            firstMsgForPortPlayer = 7,
            levelStarted = 8,
            playerPosition = 9,
            gamePaused = 10,
            distroyedObject = 11,
            createItem = 12,
            collectedKey = 13,
            openDoor = 14,
        }

        public static int MyPlayerID()
        {
            if (NetworkEngine_5._0.Client.Client.GetState() == Client.ClientState.Connected)
                return NetworkEngine_5._0.Client.Client.ID + 1;

            return 1;
        }

        public static List<int> usedPlayerID = new List<int>();

    }

}
