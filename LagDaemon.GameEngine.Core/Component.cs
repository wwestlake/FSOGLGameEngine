using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagDaemon.GameEngine.Core
{
    public abstract class Component
    {
        protected GameObject parent;

        public Component(GameObject parent)
        {
            this.parent = parent;
        }

        public abstract void Initialize();
        public abstract void Update();
    }
}
