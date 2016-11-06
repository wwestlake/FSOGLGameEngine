using LagDaemon.GameEngine.Core;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
{
    public class TestGameObject : GameObject
    {
        public override void OnDestroy()
        {
        }

        public override void OnInitialize()
        {
        }

        public override void OnLoad()
        {
        }

        public override void OnUpdate(double deltaTime)
        {
            GL.Begin(PrimitiveType.Quads);

            GL.Color3(Color.Red);
            GL.Vertex2(0, 0);
            GL.Color3(Color.Blue);
            GL.Vertex2(0.9f, 0);
            GL.Color3(Color.Orange);
            GL.Vertex2(1, -0.9f);
            GL.Color3(Color.Green);
            GL.Vertex2(0, -1);


            GL.End();
        }
    }
}
