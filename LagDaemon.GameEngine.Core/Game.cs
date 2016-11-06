using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagDaemon.GameEngine.Core
{
    public class Game
    {
        private GameWindow _window;

        public Game(GameWindow window)
        {
            _window = window;

            _window.Load += _window_Load;
            _window.Resize += _window_Resize;
            _window.UpdateFrame += _window_UpdateFrame;
            _window.RenderFrame += _window_RenderFrame;
            _window.Closing += _window_Closing;

            _window.Run();

        }

        private void _window_Load(object sender, EventArgs e)
        {
        }

        private void _window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        private void _window_Resize(object sender, EventArgs e)
        {
        }

        private void _window_UpdateFrame(object sender, FrameEventArgs e)
        {
        }

        private void _window_RenderFrame(object sender, FrameEventArgs e)
        {
            
        }



    }
}
