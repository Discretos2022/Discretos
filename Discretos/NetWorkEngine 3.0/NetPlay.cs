using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Net;
using System.Net.Sockets;

namespace Plateform_2D_v9.NetWorkEngine_3._0
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
        }

    }

}
