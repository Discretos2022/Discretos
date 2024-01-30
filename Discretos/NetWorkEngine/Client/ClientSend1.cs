using System;
using System.Collections.Generic;
using System.Text;

namespace Plateform_2D_v9.NetWorkEngine.Client
{
    class ClientSend1
    {

        private static void SendTCPData(Packet1 _packet)
        {
            _packet.WriteLength();
            Client1.instance.tcp.SendData(_packet);
        }

        #region Packets
        public static void WelcomeReceived()
        {
            using (Packet1 _packet = new Packet1((int)ClientPackets.welcomeReceived))
            {
                _packet.Write(Client1.instance.myId);
                //_packet.Write(UIManager.instance.usernameField.text);

                SendTCPData(_packet);
            }
        }
        #endregion

    }
}
