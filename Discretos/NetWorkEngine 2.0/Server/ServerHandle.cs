using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Plateform_2D_v9.NetWorkEngine_2._0.Server
{
    class ServerHandle
    {

        public static void WelcomeReceived(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            string _username = _packet.ReadString();

            Console.WriteLine($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected succesfully and is now player {_fromClient}.");
            if(_fromClient != _clientIdCheck)
            {
                Console.WriteLine($"Player \"{_username}\" (ID: {_fromClient}) has assumed client ID ({_clientIdCheck})!");
            }
            // TODO: send player into game
            Handler.AddPlayerV2(_fromClient);

            ServerSend.NewPlayerConnected(_fromClient);

        }

        public static void UDPTestReceived(int _fromClient, Packet _packet)
        {
            string _msg = _packet.ReadString();

            Console.WriteLine($"Received packet via UDP. Contains message: {_msg}");
        }


        #region Added reception packet

        public static void PlayerPos(int _fromClient, Packet _packet)
        {
            int _clientID = _packet.ReadInt();
            int x = _packet.ReadInt();
            int y = _packet.ReadInt();

            //Console.WriteLine($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected succesfully and is now player {_fromClient}.");

            //Handler.playersV2[_clientID].Position = new Vector2(x, y);

            ServerSend.PlayerPos(_clientID, x, y);

        }

        public static void CollectedMoney(int _fromClient, Packet _packet)
        {
            int _clientId = _packet.ReadInt();
            int _money = _packet.ReadInt();

            ServerSend.Money(_clientId, _money);


        }

        public static void PlayerState(int _fromClient, Packet _packet)
        {
            int _clientId = _packet.ReadInt();
            bool _isLower = _packet.ReadBool();
            bool _wasLower = _packet.ReadBool();
            bool _isRight = _packet.ReadBool();
            bool _goRight = _packet.ReadBool();
            bool _goLeft = _packet.ReadBool();

            int _AttackRectX = _packet.ReadInt();
            int _AttackRectY = _packet.ReadInt();
            int _AttackRectW = _packet.ReadInt();
            int _AttackRectH = _packet.ReadInt();

            ServerSend.PlayerState(_clientId, _isLower, _wasLower, _isRight, _goRight, _goLeft, _AttackRectX, _AttackRectY, _AttackRectW, _AttackRectH);

        }

        public static void PlayerKey(int _fromClient, Packet _packet)
        {
            int _clientId = _packet.ReadInt();

            bool _rightKey = _packet.ReadBool();
            bool _leftKey = _packet.ReadBool();
            bool _upKey = _packet.ReadBool();
            bool _downKey = _packet.ReadBool();
            bool _attackKey = _packet.ReadBool();

            ServerSend.PLayerKey(_clientId, _rightKey, _leftKey, _upKey, _downKey, _attackKey);

        }

        #endregion

    }
}
