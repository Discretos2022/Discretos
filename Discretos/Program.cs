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

            using (var game = new Main())
            {
                game.Run();
            }

        }
    }
}