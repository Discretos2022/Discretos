using System;
using System.Collections.Generic;
using System.Text;

namespace Plateform_2D_v9.NetWorkEngine_2._0.Server
{
    class ServerSend
    {
        private static void SendTCPData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            Server.clients[_toClient].tcp.SendData(_packet);
        }

        private static void SendUDPData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            Server.clients[_toClient].udp.SendData(_packet);
        }

        private static void SendTCPDataToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                Server.clients[i].tcp.SendData(_packet);
            }
        }
        private static void SendTCPDataToAll(int _exceptClient, Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if (i != _exceptClient)
                {
                    Server.clients[i].tcp.SendData(_packet);
                }
            }
        }

        private static void SendUDPDataToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                Server.clients[i].udp.SendData(_packet);
            }
        }
        private static void SendUDPDataToAll(int _exceptClient, Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if (i != _exceptClient)
                {
                    Server.clients[i].udp.SendData(_packet);
                }
            }
        }


        #region Packets

        public static void Welcome(int _toClient, string _msg)
        {
            using (Packet _packet = new Packet((int)ServerPackets.welcome))
            {
                _packet.Write(_msg);
                _packet.Write(_toClient);

                SendTCPData(_toClient, _packet);
            }
        }

        public static void UDPTest(int _toClient)
        {
            using (Packet _packet = new Packet((int)ServerPackets.udpTest))
            {
                _packet.Write("A test packet for UDP.");

                SendTCPData(_toClient, _packet);
            }
        }


        #region Added Packet

        public static void Level(int _toClient, int _level)
        {
            using (Packet _packet = new Packet((int)ServerPackets.level))
            {
                _packet.Write(_level);
                //_packet.Write(_toClient);

                SendTCPData(_toClient, _packet);
            }
        }

        public static void PlayerPos(int _playerID, int x, int y)
        {
            using (Packet _packet = new Packet((int)ServerPackets.playerPos))
            {
                _packet.Write(_playerID);
                _packet.Write(x);
                _packet.Write(y);

                SendUDPDataToAll(_playerID, _packet);
            }
        }

        public static void Money(int _exceptClient, int _money)
        {
            using (Packet _packet = new Packet((int)ServerPackets.money))
            {
                _packet.Write(_money);

                SendUDPDataToAll(_exceptClient, _packet);
            }
        }

        public static void PlayerState(int _playerIDState, bool _isLower, bool _wasLower, bool _isRight, bool _goRight, bool _goLeft, int _AttackRectX, int _AttackRectY, int _AttackRectW, int _AttackRectH)
        {
            using (Packet _packet = new Packet((int)ServerPackets.playerState))
            {
                _packet.Write(_playerIDState);
                _packet.Write(_isLower);
                _packet.Write(_wasLower);
                _packet.Write(_isRight);
                _packet.Write(_goRight);
                _packet.Write(_goLeft);

                _packet.Write(_AttackRectX);
                _packet.Write(_AttackRectY);
                _packet.Write(_AttackRectW);
                _packet.Write(_AttackRectH);

                SendUDPDataToAll(_playerIDState, _packet);
            }

        }


        public static void PLayerKey(int _playerID, bool _rightKey, bool _leftKey, bool _upKey, bool _downKey, bool _attackKey)
        {
            using (Packet _packet = new Packet((int)ServerPackets.playerKey))
            {
                _packet.Write(_playerID);
                _packet.Write(_rightKey);
                _packet.Write(_leftKey);
                _packet.Write(_upKey);
                _packet.Write(_downKey);
                _packet.Write(_attackKey);

                SendUDPDataToAll(_playerID, _packet);
            }
        }


        public static void NewPlayerConnected(int _playerID)
        {
            using (Packet _packet = new Packet((int)ServerPackets.newPLayerConnected))
            {
                _packet.Write(_playerID);

                SendUDPDataToAll(_playerID, _packet);
            }
        }


        #endregion

        #endregion

    }
}
