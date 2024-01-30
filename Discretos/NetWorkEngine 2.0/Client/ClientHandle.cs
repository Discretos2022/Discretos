using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Plateform_2D_v9.NetWorkEngine_2._0.Client
{
    class ClientHandle
    {

        public static void Welcome(Packet _packet)
        {
            string _msg = _packet.ReadString();
            int _myId = _packet.ReadInt();

            Console.WriteLine($"Message from server: {_msg}");
            Client.instance.myId = _myId;
            Handler.AddPlayerV2(1);
            Handler.AddPlayerV2(_myId);
            ClientSend.WelcomeReceived();

            Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);

        }

        public static void UDPTest(Packet _packet)
        {
            string _msg = _packet.ReadString();

            Console.WriteLine($"Received packet via UDP. Contains message: {_msg}");
            ClientSend.UDPTestReceived();

        }


        #region Added receptionPacket

        public static void Level(Packet _packet)
        {
            int _level = _packet.ReadInt();

            Console.WriteLine($"Level selected is the level {_level}");

            Main.MapLoaded = false;
            Main.LevelSelector(_level);
            Main.playState = PlayState.InLevel;
            Camera.Zoom = 4f;
            Main.gameState = GameState.Multiplaying;

        }

        public static void PlayerPos(Packet _packet)
        {
            int _clientID = _packet.ReadInt();

            int _x = _packet.ReadInt();
            int _y = _packet.ReadInt();

            Handler.playersV2[_clientID].Position = new Vector2(_x, _y);

        }

        public static void CollectedMoney(Packet _packet)
        {
            int _money = _packet.ReadInt();

            Main.Money += _money;

        } 

        public static void PlayerState(Packet _packet)
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

            Handler.playersV2[_clientId].isLower = _isLower;
            Handler.playersV2[_clientId].wasLower = _wasLower;
            Handler.playersV2[_clientId].isRight = _isRight;
            Handler.playersV2[_clientId].goRight = _goRight;
            Handler.playersV2[_clientId].goLeft = _goLeft;

            Handler.playersV2[_clientId].AttackRect = new Rectangle(_AttackRectX, _AttackRectY, _AttackRectW, _AttackRectH);

        }


        public static void PlayerKey(Packet _packet)
        {
            int _clientId = _packet.ReadInt();

            bool _rightKey = _packet.ReadBool();
            bool _leftKey = _packet.ReadBool();
            bool _upKey = _packet.ReadBool();
            bool _downKey = _packet.ReadBool();
            bool _attackKey = _packet.ReadBool();

            Handler.playersV2[_clientId].RightOldKey = Handler.playersV2[_clientId].RightKey;
            Handler.playersV2[_clientId].LeftOldKey = Handler.playersV2[_clientId].LeftKey;
            Handler.playersV2[_clientId].UpOldKey = Handler.playersV2[_clientId].UpKey;
            Handler.playersV2[_clientId].DownOldKey = Handler.playersV2[_clientId].DownKey;
            Handler.playersV2[_clientId].AttackOldKey = Handler.playersV2[_clientId].AttackKey;

            Handler.playersV2[_clientId].RightKey = _rightKey;
            Handler.playersV2[_clientId].LeftKey = _leftKey;
            Handler.playersV2[_clientId].UpKey = _upKey;
            Handler.playersV2[_clientId].DownKey = _downKey;
            Handler.playersV2[_clientId].AttackKey = _attackKey;

            Handler.playersV2[_clientId].Attack();


        }

        public static void NewPlayer(Packet _packet)
        {
            int _clientId = _packet.ReadInt();

            Console.WriteLine("New Player has connected : Player " + _clientId);

            Handler.AddPlayerV2(_clientId);


        }



        #endregion

    }
}
