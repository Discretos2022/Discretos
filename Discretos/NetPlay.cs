using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Net;
using System.Net.Sockets;

namespace Plateform_2D_v9
{


    class NetPlay
    {

        public static int serverPort = 7777;
        public static string portForwardIP;
        public static int portForwardPort;

        public static bool connected = false;

        public static bool server = false;
        public static bool client = false;

        public static int clientnum = 0;

        public static int num;

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

    }

}
