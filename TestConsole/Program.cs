using LagDaemon.GameEngine.Core;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game(new GameWindow(800, 600));
        }
    }
}
