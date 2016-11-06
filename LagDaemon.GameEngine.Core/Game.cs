using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagDaemon.GameEngine.Core
{
    public class Game
    {
        private GameWindow _window;
        private Scene scene = new Scene("Main Scene");


        public Game(GameWindow window)
        {
            _window = window;

            _window.Load += _window_Load;
            _window.Resize += _window_Resize;
            _window.UpdateFrame += _window_UpdateFrame;
            _window.RenderFrame += _window_RenderFrame;
            _window.Closing += _window_Closing;


        }

        public void Run()
        {
            _window.Run();
        }

        public void AddObject(GameObject obj)
        {
            scene.AddGameObject(obj);
        }

        private void _window_Load(object sender, EventArgs e)
        {
            scene.OnLoad();
        }

        private void _window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            scene.OnDestroy();
        }

        private void _window_Resize(object sender, EventArgs e)
        {
        }

        private void _window_UpdateFrame(object sender, FrameEventArgs e)
        {
            scene.OnUpdate(e.Time);
        }

        private void _window_RenderFrame(object sender, FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.ClearColor(Color.CornflowerBlue);

            scene.OnUpdate(e.Time);

            _window.SwapBuffers();
        }



    }
}
