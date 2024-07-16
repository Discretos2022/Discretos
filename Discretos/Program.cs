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