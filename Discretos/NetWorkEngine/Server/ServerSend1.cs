using System;
using System.Collections.Generic;
using System.Text;

namespace Plateform_2D_v9.NetWorkEngine
{
    class ServerSend1
    {
        private static void SendTCPData(int _toClient, Packet1 _packet)
        {
            _packet.WriteLength();
            Server1.players[_toClient].tcp.SendData(_packet);
        }

        private static void SendTCPDataToAll(Packet1 _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server1.MaxPlayers; i++)
            {
                Server1.players[i].tcp.SendData(_packet);
            }
        }
        private static void SendTCPDataToAll(int _exceptClient, Packet1 _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server1.MaxPlayers; i++)
            {
                if (i != _exceptClient)
                {
                    Server1.players[i].tcp.SendData(_packet);
                }
            }
        }

        public static void Welcome(int _toClient, string _msg)
        {
            using (Packet1 _packet = new Packet1((int)ServerPackets.welcome))
            {
                _packet.Write(_msg);
                _packet.Write(_toClient);

                SendTCPData(_toClient, _packet);
            }
        }
    }
}
