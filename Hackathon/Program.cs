using System;

namespace Hackathon {
    public static class Program {
        [STAThread]
        static void Main() {
            using (var game = new Root())
                game.Run();
        }
    }
}
