using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Plateform_2D_v9
{
    /// <summary>
    /// DELETE
    /// </summary>
    public class ClientSocket
    {

        //public string IP = "192.168.1.25";
        //public Int32 port = 7777;

        //public string message = "";

        //public NetworkStream networkStream;

        //public byte[] readBuffer = new byte[1024];
        //public byte[] writeBuffer = new byte[1024];

        //public string data;
        //public int Level = 0;


        //public TcpClient tcpClient = new TcpClient();

        //public void Connect(string IP = "127.0.0.1", int port = 7777)    ///"127.0.0.1" "192.168.1.25"
        //{
        //    this.IP = IP;
        //    this.port = port;

        //    try
        //    {
        //        //tcpClient.Close();
        //        message = "connection en cours...";
        //        tcpClient = new TcpClient();

        //        tcpClient.Connect(IP, port);
        //        message = "connection reussi";

        //        networkStream = tcpClient.GetStream();

        //        //networkStream.Read(readBuffer, 500, 250);
        //        int i;
        //        while ((i = networkStream.Read(readBuffer, 0, readBuffer.Length)) != 0)
        //        {
        //            data = System.Text.Encoding.ASCII.GetString(readBuffer, 0, i);
        //            Console.WriteLine("Received: {0}", data);

        //            Level = Int32.Parse(data);

        //            if (Level != 0)
        //                break;


        //        }

        //        Main.MapLoaded = false;
        //        Main.LevelSelector(Level);
        //        Main.inWorldMap = false;
        //        Main.inLevel = true;
        //        Camera.Zoom = 4f;
        //        Main.gameState = GameState.Playing;

        //    }
        //    catch (SocketException e)
        //    {

        //        if (e.ErrorCode == 10056)
        //            message = "deja connecte";
        //        else if (e.ErrorCode == 10061 || e.ErrorCode == 10060)
        //        {
        //            tcpClient = new TcpClient();
        //            message = "connection impossible";
        //        }

        //        Console.WriteLine(e);

        //    }
        //    catch (ObjectDisposedException e)
        //    {
        //        message = "connection en cours...";

        //        Console.WriteLine("aaaaaa");
                
        //    }
        //    catch (IOException e)
        //    {
        //        message = "server distant fermer";
        //        Console.WriteLine("server distant fermer");
        //        Disconnect();
        //        Main.gameState = GameState.Menu;

        //    }


        //}

        //public void ReceivePackage(object threadContext)
        //{
        //    networkStream = tcpClient.GetStream();

        //    int i;
        //    while ((i = networkStream.Read(readBuffer, 0, readBuffer.Length)) != 0)
        //    {
        //        data = System.Text.Encoding.ASCII.GetString(readBuffer, 0, i);
        //        Console.WriteLine("Received: {0}", data);

        //        Level = Int32.Parse(data);

        //        if (Level != 0)
        //            break;


        //    }
        //}

        //public void Disconnect()
        //{
        //    try
        //    {
        //        if(tcpClient.Client != null)
        //        {
        //            tcpClient.Client.Dispose();
        //            tcpClient.Close();
        //            tcpClient.Dispose();
        //            tcpClient = null;
        //            message = "deconnection reussi";
        //            NetPlay.IsMultiplaying = false;

        //        }
                
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //    }

        //}

        //public bool IsDisconnected()
        //{
        //    if(tcpClient != null)
        //        if (tcpClient.Connected)
        //            return true;

        //    return false;
        //}

        //public bool IsConnected()
        //{
        //    if (tcpClient != null)
        //        if (tcpClient.Connected)
        //            return true;

        //    return false;
        //}


        //public string GetMessage()
        //{
        //    return message;
        //}



    }
}
