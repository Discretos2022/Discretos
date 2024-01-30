using System;
using System.Collections.Generic;
using System.Text;

namespace Plateform_2D_v9.NetWorkEngine_2._0.Client
{
    class ClientSend
    {

        public static void SendTCPData(Packet _packet)
        {
            _packet.WriteLength();
            Client.instance.tcp.SendData(_packet);
        }

        public static void SendUDPData(Packet _packet)
        {
            _packet.WriteLength();
            Client.instance.udp.SendData(_packet);
        }

        #region Packets
        public static void WelcomeReceived()
        {
            using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
            {
                _packet.Write(Client.instance.myId);
                _packet.Write("Player 2");

                SendTCPData(_packet);
            }
        }

        public static void UDPTestReceived()
        {
            using (Packet _packet = new Packet((int)ClientPackets.udpTestReceived))
            {
                _packet.Write("Received a UDP packet.");

                SendTCPData(_packet);
            }
        }


        #region Added packet

        public static void PlayerPos()
        {
            using (Packet _packet = new Packet((int)ClientPackets.playerpos))
            {
                _packet.Write(Client.instance.myId);
                _packet.Write((int)Handler.playersV2[Client.instance.myId].Position.X);
                _packet.Write((int)Handler.playersV2[Client.instance.myId].Position.Y);

                SendTCPData(_packet);
            }
        }

        public static void CollectedMoney(int _money)
        {
            using (Packet _packet = new Packet((int)ClientPackets.collectedMoney))
            {
                _packet.Write(Client.instance.myId);
                _packet.Write(_money);

                SendUDPData(_packet);
            }
        }

        public static void PlayerState()
        {
            using (Packet _packet = new Packet((int)ClientPackets.playerState))
            {
                _packet.Write(Client.instance.myId);
                _packet.Write(Handler.playersV2[Client.instance.myId].IsLower());   // Le joueur est accroupi
                _packet.Write(Handler.playersV2[Client.instance.myId].wasLower);   // Le joueur était accroupi
                _packet.Write(Handler.playersV2[Client.instance.myId].IsRight());   // Le joueur regarde à droite 
                _packet.Write(Handler.playersV2[Client.instance.myId].IsGoRight()); // Le joueur va à droite
                _packet.Write(Handler.playersV2[Client.instance.myId].IsGoLeft());  // Le joueur va à gauche

                _packet.Write(Handler.playersV2[Client.instance.myId].GetAttackRectangle().X);  // Position X du rectangle d'attack
                _packet.Write(Handler.playersV2[Client.instance.myId].GetAttackRectangle().Y);  // Position Y du rectangle d'attack
                _packet.Write(Handler.playersV2[Client.instance.myId].GetAttackRectangle().Width);   // Largeur du rectangle d'attack
                _packet.Write(Handler.playersV2[Client.instance.myId].GetAttackRectangle().Height);  // Hauteur du rectangle d'attack

                SendUDPData(_packet);
            }
        }

        public static void PLayerKey()
        {
            using (Packet _packet = new Packet((int)ClientPackets.playerKey))
            {
                _packet.Write(Client.instance.myId);
                _packet.Write(Handler.playersV2[Client.instance.myId].RightKey2);
                _packet.Write(Handler.playersV2[Client.instance.myId].LeftKey2);
                _packet.Write(Handler.playersV2[Client.instance.myId].UpKey2);
                _packet.Write(Handler.playersV2[Client.instance.myId].DownKey2);
                _packet.Write(Handler.playersV2[Client.instance.myId].AttackKey2);

                SendUDPData(_packet);
            }
        }

        #endregion

        #endregion

    }
}
