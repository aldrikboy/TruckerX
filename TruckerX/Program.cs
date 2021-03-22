using System;

namespace TruckerX
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new TruckerX())
                game.Run();
        }
    }
}
