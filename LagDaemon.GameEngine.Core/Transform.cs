using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagDaemon.GameEngine.Core
{
    public struct Transform
    {
        public Vector3 Location;
        public Vector3 Rotation;
        public Vector3 Scale;

        private Transform(Vector3 location, Vector3 rotationb, Vector3 scale)
        {
            Location = location;
            Rotation = rotationb;
            Scale = scale;
            
        }

        public static Transform Default()
        {
            return new Transform(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f));
        }

        public static Transform Create(Vector3 location, Vector3 rotationb, Vector3 scale)
        {
            return new Transform(location, rotationb, scale);
        }

    }
}
