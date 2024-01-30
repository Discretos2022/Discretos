using Plateform_2D_v9.NetWorkEngine_3._0.Client;
using Plateform_2D_v9.NetWorkEngine_3._0.Server;
using System;
using System.Threading;

namespace Plateform_2D_v9
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {


            //Server.Start(2, 7777);

            //while (true)
            //{
            //    Thread.Sleep(1000);
            //}

            //Client.Connect("192.168.1.25", 7777);

            //while (true)
            //{
            //    Thread.Sleep(1000);
            //}


            using (var game = new Main())
            {
                game.Run();
            }

        }
    }
}