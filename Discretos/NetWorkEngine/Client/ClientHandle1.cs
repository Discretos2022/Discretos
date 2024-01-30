using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Plateform_2D_v9.NetWorkEngine.Client
{
    class ClientHandle1
    {

        public static void Welcome(Packet1 _packet)
        {
            string _msg = _packet.ReadString();
            int _myId = _packet.ReadInt();

            DEBUG.Log($"Message from server: {_msg}");
            Client1.instance.myId = _myId;
            ClientSend1.WelcomeReceived();
        }

    }
}
